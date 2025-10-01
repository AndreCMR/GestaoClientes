using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using GestaoClientes.Application.Validations;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.DTOs.Requests;
using Moq;

namespace Testes.Validations;

public sealed class ClienteCriarRequestValidatorTests
{
    private static ClienteCriarRequest NovoValido() => new ClienteCriarRequest
    {
        Nome = "Maria",
        DataNascimento = new DateOnly(1990, 5, 10),
        Cpf = "52998224725", // CPF válido
        Email = "maria@teste.com",
        RendimentoAnual = 80000m,
        Endereco = new EnderecoRequest
        {
            Logradouro = "Rua A",
            Numero = "10",
            Bairro = "Centro",
            Cidade = "São Paulo",
            Estado = "SP",
            Cep = "01000-000"
        },
        DddTelefone = "11",
        NumeroTelefone = "999999999"
    };

    [Fact]
    public async Task Deve_validar_quando_request_for_valido_e_cpf_nao_existir()
    {
        // Arrange
        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.ExistePorCpfAsync("52998224725", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new ClienteCriarRequestValidator(repo.Object);

        // Act
        var result = await validator.TestValidateAsync(NovoValido());

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Deve_invalidar_quando_cpf_ja_cadastrado()
    {
        // Arrange
        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.ExistePorCpfAsync("52998224725", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new ClienteCriarRequestValidator(repo.Object);

        // Act
        var result = await validator.TestValidateAsync(NovoValido());

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf)
              .WithErrorMessage("CPF já cadastrado.");
    }

    [Fact]
    public async Task Deve_invalidar_quando_data_nascimento_for_no_futuro()
    {
        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.ExistePorCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new ClienteCriarRequestValidator(repo.Object);
        var req = NovoValido();
        req.DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        var result = await validator.TestValidateAsync(req);

        result.ShouldHaveValidationErrorFor(x => x.DataNascimento)
              .WithErrorMessage("Data de Nascimento deve ser no passado.");
    }

    [Fact]
    public async Task Deve_invalidar_quando_estado_invalido()
    {
        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.ExistePorCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new ClienteCriarRequestValidator(repo.Object);
        var req = NovoValido();
        req.Endereco!.Estado = "São Paulo"; 

        var result = await validator.TestValidateAsync(req);

        result.ShouldHaveValidationErrorFor("Endereco.Estado")
              .WithErrorMessage("Estado deve ser a sigla de 2 letras (ex: SP, RJ).");
    }
}

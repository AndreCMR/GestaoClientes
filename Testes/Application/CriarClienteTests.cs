using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GestaoClientes.Application.CasosDeUso.Clientes;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Dominio.Entidades;
using GestaoClientes.DTOs.Requests;
using Moq;

namespace Testes.Application;

public sealed class CriarClienteHandleTests
{
    private static ClienteCriarRequest NovoReq() => new ClienteCriarRequest
    {
        Nome = "  Ana  ",
        DataNascimento = new DateOnly(1995, 10, 10),
        Cpf = "111.222.333-44",
        Email = "   ana@teste.com   ",
        RendimentoAnual = 90000m,
        Endereco = new EnderecoRequest
        {
            Logradouro = "Rua A",
            Numero = "100",
            Complemento = null,
            Bairro = "Centro",
            Cidade = "São Paulo",
            Estado = "SP",
            Cep = "01000-000"
        },
        DddTelefone = " 11 ",
        NumeroTelefone = " 912345678 "
    };

    [Fact]
    public async Task Handle_deve_validar_mapear_persistir_e_retornar_id()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;

        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.CriarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var validator = new Mock<IValidator<ClienteCriarRequest>>();
        validator
                .Setup(v => v.ValidateAsync(It.IsAny<ClienteCriarRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

        var usecase = new CriarCliente(repo.Object, validator.Object);
        var req = NovoReq();

        // Act
        var id = await usecase.Handle(req, ct);

        // Assert
        id.Should().NotBe(Guid.Empty);

        repo.Verify(r => r.CriarAsync(It.Is<Cliente>(c =>
            c.Id != Guid.Empty &&
            c.Nome == "Ana" &&
            c.Cpf == "11122233344" &&           
            c.Email == "ana@teste.com" &&      
            c.RendimentoAnual == 90000m &&
            c.Endereco.Estado == "SP" &&
            c.DddTelefone == "11" &&
            c.NumeroTelefone == "912345678"
        ), ct), Times.Once);

        validator.Verify(v => v.ValidateAsync(It.IsAny<ClienteCriarRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.CriarAsync(It.IsAny<Cliente>(), ct), Times.Once);
        id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_deve_lancar_ValidationException_quando_request_invalida()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;

        var repo = new Mock<IClienteRepositorio>();

        var validator = new Moq.Mock<IValidator<ClienteCriarRequest>>();
        validator
        .Setup(v => v.ValidateAsync(
            It.IsAny<ClienteCriarRequest>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult(new[]
        {
        new ValidationFailure("Cpf", "CPF inválido.")
        }));

        var usecase = new CriarCliente(repo.Object, validator.Object);
        var req = NovoReq();

        // Act
        var act = async () => await usecase.Handle(req, ct);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
                 .WithMessage("*CPF inválido*");

        repo.Verify(r => r.CriarAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

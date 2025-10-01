using FluentAssertions;
using GestaoClientes.Application.CasosDeUso.Clientes;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Dominio.Entidades;
using Moq;
using Xunit;

namespace Testes.Application;

public sealed class ObterScoreClienteTests
{
    private static Cliente NovoCliente(decimal renda, DateOnly nasc) =>
        new Cliente(Guid.NewGuid(), "Ana", nasc, "11122233344",
            "ana@a.com", renda,
            new Endereco { Estado = "SP" }, "11", "999999999");

    [Theory]
    // renda baixa + idade baixa
    [InlineData(59000, 2005, 10, 1, 150)]
    // renda média + idade média
    [InlineData(60000, 1995, 10, 1, 350)]
    // renda alta + idade alta
    [InlineData(120001, 1970, 9, 30, 500)]
    public async Task Deve_calcular_score_conforme_faixas_de_renda_e_idade(
        decimal renda, int y, int m, int d, int esperado)
    {
        // Arrange
        var id = Guid.NewGuid();
        var cliente = NovoCliente(renda, new DateOnly(y, m, d));

        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        var usecase = new ObterScoreCliente(repo.Object);

        // Act
        var response = await usecase.Handle(id, null, CancellationToken.None);

        // Assert
        response.Score.Should().Be(esperado);
    }

    [Fact]
    public async Task Deve_lancar_quando_cliente_nao_encontrado()
    {
        // Arrange
        var id = Guid.NewGuid();
        var repo = new Mock<IClienteRepositorio>();
        repo.Setup(r => r.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cliente?)null);

        var usecase = new ObterScoreCliente(repo.Object);

        // Act
        var act = async () => await usecase.Handle(id, null, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Cliente não encontrado.");
    }
}

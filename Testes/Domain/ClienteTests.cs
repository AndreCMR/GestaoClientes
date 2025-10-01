using System;
using FluentAssertions;
using GestaoClientes.Dominio.Entidades;
using Xunit;

namespace Testes.Domain;

public sealed class ClienteTests
{
    private static Endereco EnderecoValido() => new Endereco
    {
        Logradouro = "Rua A",
        Numero = "100",
        Bairro = "Centro",
        Cidade = "São Paulo",
        Estado = "SP",
        Cep = "01000-000",
        Complemento = null
    };

    [Fact]
    public void Deve_criar_cliente_normalizando_campos_e_gerando_id_quando_vazio()
    {
        var cliente = new Cliente(
            id: Guid.Empty,
            nome: "  Ana  ",
            dataNascimento: new DateOnly(1995, 10, 10),
            cpf: "111.222.333-44",
            email: "   ana@teste.com   ",
            rendimentoAnual: 90000m,
            endereco: EnderecoValido(),
            dddTelefone: " 11 ",
            numeroTelefone: " 912345678 "
        );

        cliente.Id.Should().NotBe(Guid.Empty);
        cliente.Nome.Should().Be("Ana");
        cliente.Cpf.Should().Be("11122233344");
        cliente.Email.Should().Be("ana@teste.com");
        cliente.DddTelefone.Should().Be("11");
        cliente.NumeroTelefone.Should().Be("912345678");
        cliente.Endereco.Estado.Should().Be("SP");
    }

    [Fact]
    public void Deve_lancar_excecao_quando_estado_do_endereco_for_vazio()
    {
        var endereco = EnderecoValido();
        endereco.Estado = "";

        Action act = () => new Cliente(
            Guid.NewGuid(), "João", new DateOnly(2000, 1, 1),
            "529.982.247-25", "joao@teste.com", 50000m,
            endereco, "11", "999999999"
        );

        act.Should().Throw<ArgumentException>()
           .WithMessage("Estado é obrigatório no endereço.");
    }
}
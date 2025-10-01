using System.Text.RegularExpressions;

namespace GestaoClientes.Dominio.Entidades;

public sealed class Cliente
{
    public Guid Id { get; }
    public string Nome { get; private set; }
    public DateOnly DataNascimento { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public decimal RendimentoAnual { get; private set; }
    public Endereco Endereco { get; private set; }

    public string DddTelefone { get; private set; }
    public string NumeroTelefone { get; private set; }

    public Cliente(Guid id, string nome, DateOnly dataNascimento, string cpf,
                   string email, decimal rendimentoAnual, Endereco endereco,
                   string dddTelefone, string numeroTelefone)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Nome = nome.Trim();
        DataNascimento = dataNascimento;
        Cpf = Regex.Replace(cpf ?? "", @"\D", "");
        Email = email.Trim();
        RendimentoAnual = rendimentoAnual;

        if (string.IsNullOrWhiteSpace(endereco.Estado))
            throw new ArgumentException("Estado é obrigatório no endereço.");

        Endereco = new Endereco
        {
            Logradouro = endereco.Logradouro?.Trim() ?? string.Empty,
            Numero = endereco.Numero?.Trim() ?? string.Empty,
            Complemento = endereco.Complemento?.Trim(),
            Bairro = endereco.Bairro?.Trim() ?? string.Empty,
            Cidade = endereco.Cidade?.Trim() ?? string.Empty,
            Estado = endereco.Estado.Trim().ToUpperInvariant(),
            Cep = endereco.Cep?.Trim() ?? string.Empty
        };

        DddTelefone = dddTelefone.Trim();
        NumeroTelefone = numeroTelefone.Trim();
    }
}

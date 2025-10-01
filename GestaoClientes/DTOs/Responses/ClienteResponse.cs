namespace GestaoClientes.DTOs.Responses;

/// <summary>Resposta com dados do cliente e score.</summary>
public class ClienteResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal RendimentoAnual { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TelefoneDDD { get; set; } = string.Empty;
    public string TelefoneNumero { get; set; } = string.Empty;

    public int Score { get; set; }

    public string Classificacao { get; set; } = string.Empty;
}
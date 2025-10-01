namespace GestaoClientes.DTOs.Requests;

/// <summary>
/// Payload de criação de cliente (entrada da API).
/// </summary>
public sealed class ClienteCriarRequest
{
    public string Nome { get; set; } = default!;
    public DateOnly DataNascimento { get; set; }         
    public string Cpf { get; set; } = default!;
    public string Email { get; set; } = default!;
    public decimal RendimentoAnual { get; set; }
    public EnderecoRequest Endereco { get; set; } = new();
    public string DddTelefone { get; set; } = default!;  
    public string NumeroTelefone { get; set; } = default!;
}
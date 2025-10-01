namespace GestaoClientes.DTOs.Requests;

/// <summary>Dados de endereço do cliente.</summary>
/// <remarks>Apenas <b>Estado</b> é obrigatório para o desafio.</remarks>
public sealed class EnderecoRequest
{
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }

    public string Estado { get; set; } = default!;

    public string? Cep { get; set; }
}

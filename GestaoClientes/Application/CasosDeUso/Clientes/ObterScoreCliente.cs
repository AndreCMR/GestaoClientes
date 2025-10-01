using GestaoClientes.Application.Interfaces;
using GestaoClientes.DTOs.Responses;

namespace GestaoClientes.Application.CasosDeUso.Clientes;

public sealed class ObterScoreCliente(IClienteRepositorio _repositorio) : IObterScoreCliente
{
    public async Task<ClienteResponse> Handle(Guid id, DateOnly? hoje, CancellationToken ct)
    {
        var cliente = await _repositorio.ObterPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Cliente não encontrado.");

        var dataBase = hoje ?? DateOnly.FromDateTime(DateTime.Today);
        var idade = CalcularIdade(cliente.DataNascimento, dataBase);

        int score = 0;

        if (cliente.RendimentoAnual > 120_000m) score += 300;
        else if (cliente.RendimentoAnual >= 60_000m) score += 200;
        else score += 100;

        if (idade > 40) score += 200;
        else if (idade >= 25) score += 150;
        else score += 50;

        var classificacao =
            score >= 450 ? "Bom" :
            score >= 300 ? "Regular" : "Mau";

        return new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            DataNascimento = cliente.DataNascimento.ToDateTime(TimeOnly.MinValue),
            Cpf = cliente.Cpf,
            Email = cliente.Email,
            RendimentoAnual = cliente.RendimentoAnual,
            Estado = cliente.Endereco.Estado,
            TelefoneDDD = cliente.DddTelefone,
            TelefoneNumero = cliente.NumeroTelefone,
            Score = score,
            Classificacao = classificacao
        };
    }


    private static int CalcularIdade(DateOnly nascimento, DateOnly hoje)
    {
        var idade = hoje.Year - nascimento.Year;
        if (nascimento.AddYears(idade) > hoje) idade--;
        return idade;
    }

}

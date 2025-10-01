using GestaoClientes.DTOs.Responses;

namespace GestaoClientes.Application.Interfaces;
public interface IObterScoreCliente
{
    Task<ClienteResponse> Handle(Guid id, DateOnly? hoje, CancellationToken ct);
}

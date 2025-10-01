using GestaoClientes.DTOs.Requests;

namespace GestaoClientes.Application.Interfaces;

public interface ICriarCliente
{
    Task<Guid> Handle(ClienteCriarRequest request, CancellationToken ct);
}
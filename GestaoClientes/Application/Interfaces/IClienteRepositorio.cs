using GestaoClientes.Dominio.Entidades;

namespace GestaoClientes.Application.Interfaces;

public interface IClienteRepositorio
{
    Task<bool> ExistePorCpfAsync(string cpfSomenteDigitos, CancellationToken ct);
    Task CriarAsync(Cliente cliente, CancellationToken ct);
    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct);
}
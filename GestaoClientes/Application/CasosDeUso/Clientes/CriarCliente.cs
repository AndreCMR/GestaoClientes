using FluentValidation;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Dominio.Entidades;
using GestaoClientes.DTOs.Requests;

namespace GestaoClientes.Application.CasosDeUso.Clientes;

public sealed class CriarCliente(IClienteRepositorio _repositorio, IValidator<ClienteCriarRequest> _validator) : ICriarCliente
{  

    public async Task<Guid> Handle(ClienteCriarRequest request, CancellationToken ct)
    {
        var result = await _validator.ValidateAsync(request, ct);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
        var endereco = new Endereco
        {
            Logradouro = request.Endereco.Logradouro ?? string.Empty,
            Numero = request.Endereco.Numero ?? string.Empty,
            Complemento = request.Endereco.Complemento,
            Bairro = request.Endereco.Bairro ?? string.Empty,
            Cidade = request.Endereco.Cidade ?? string.Empty,
            Estado = request.Endereco.Estado,
            Cep = request.Endereco.Cep ?? string.Empty
        };

        var cliente = new Cliente(
            id: Guid.Empty,
            nome: request.Nome,
            dataNascimento: request.DataNascimento,
            cpf: request.Cpf,
            email: request.Email,
            rendimentoAnual: request.RendimentoAnual,
            endereco: endereco,
            dddTelefone: request.DddTelefone,
            numeroTelefone: request.NumeroTelefone
        );

        await _repositorio.CriarAsync(cliente, ct);
        return cliente.Id;
    }
}
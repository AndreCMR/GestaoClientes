using GestaoClientes.Application.Interfaces;
using GestaoClientes.DTOs.Requests;
using GestaoClientes.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoClientes.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    /// <summary>Cria um novo cliente e retorna o ID.</summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    /// {
    ///   "nome":"João da Silva",
    ///   "dataNascimento":"1989-04-15",
    ///   "cpf":"12345678901",
    ///   "email":"joao@email.com",
    ///   "rendimentoAnual":80000,
    ///   "endereco":{"estado":"SP"},
    ///   "dddTelefone":"11",
    ///   "numeroTelefone":"987654321"
    /// }
    /// </remarks>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar cliente", Description = "Cadastra um cliente e retorna o identificador.")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(
        [FromServices] ICriarCliente useCase,
        [FromBody] ClienteCriarRequest request,
        CancellationToken ct)
    {
        var id = await useCase.Handle(request, ct);
        return CreatedAtAction(nameof(ObterScore), new { id }, new { id });
    }

    /// <summary>Obtém score e classificação do cliente.</summary>
    /// <param name="id">Identificador do cliente.</param>
    [HttpGet("{id:guid}/score")]
    [SwaggerOperation(Summary = "Obter score do cliente", Description = "Calcula e retorna o score de confiança do cliente.")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteResponse>> ObterScore(
        [FromServices] IObterScoreCliente useCase,
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var response = await useCase.Handle(id, null, ct);
        return Ok(response);
    }
}

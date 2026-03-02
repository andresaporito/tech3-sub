using Microsoft.AspNetCore.Mvc;
using PedidoApi.Application;
using PedidoApi.Domain;

namespace PedidoApi.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _service;

    public PedidosController(PedidoService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoRequest request, CancellationToken ct)
    {
        try
        {
            var id = await _service.CriarAsync(request, ct);
            return CreatedAtAction(nameof(Criar), new { id }, new { id });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}
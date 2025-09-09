using ClienteService.Models;
using ClienteService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClienteService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly PedidoClient _pedidoClient;
        public ClientesController(PedidoClient pedidoClient) { _pedidoClient = pedidoClient; }

        [HttpPost("{clienteId}/pedidos")]
        public async Task<IActionResult> CrearPedidoParaCliente(int clienteId, [FromBody] PedidoDto pedido)
        {
            // 1) Obtener token (demo con credenciales hardcode). En producción almacenar y renovar.
            var token = await _pedidoClient.GetTokenAsync("test", "password");

            // 2) Llamar al servicio de pedidos
            var creado = await _pedidoClient.CreatePedidoAsync(pedido, token);

            // 3) Hacer algo con el resultado: relacionar con cliente en tu BD (no implementado aquí)
            return CreatedAtAction(nameof(ObtenerPedidoParaCliente), new { clienteId = clienteId, id = creado.Id }, creado);
        }

        [HttpGet("{clienteId}/pedidos/{id}")]
        public async Task<IActionResult> ObtenerPedidoParaCliente(int clienteId, int id)
        {
            var token = await _pedidoClient.GetTokenAsync("test", "password");
            var pedido = await _pedidoClient.GetPedidoAsync(id, token);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }
    }
}

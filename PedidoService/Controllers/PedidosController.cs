using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidoService.Models;
using PedidoService.Repositories;

namespace PedidoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepository _repo;
        public PedidosController(IPedidoRepository repo) { _repo = repo; }

        [HttpPost]
        public async Task<IActionResult> CrearPedido([FromBody] Pedido pedido)
        {
            var creado = await _repo.AddAsync(pedido);
            return CreatedAtAction(nameof(ObtenerPedido), new { id = creado.Id }, creado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPedido(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PedidoService.Data;
using PedidoService.Models;

namespace PedidoService.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidoDbContext _db;
        public PedidoRepository(PedidoDbContext db) { _db = db; }

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            _db.Pedidos.Add(pedido);
            await _db.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            return await _db.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}

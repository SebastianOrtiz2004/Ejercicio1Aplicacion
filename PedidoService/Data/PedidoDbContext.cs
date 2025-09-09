using Microsoft.EntityFrameworkCore;
using PedidoService.Models;

namespace PedidoService.Data
{
    public class PedidoDbContext : DbContext
    {
        public PedidoDbContext(DbContextOptions<PedidoDbContext> options)
            : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; } = null!;
    }
}

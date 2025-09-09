using PedidoService.Models;
namespace PedidoService.Repositories
{
    public interface IPedidoRepository
    {
        Task<Pedido> GetByIdAsync(int id);
        Task<Pedido> AddAsync(Pedido pedido);
    }
}

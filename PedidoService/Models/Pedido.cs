namespace PedidoService.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Producto { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}

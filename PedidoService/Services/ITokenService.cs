using PedidoService.Models;
namespace PedidoService.Services
{
public interface ITokenService
{
    string BuildToken(string username, string role);
}

}

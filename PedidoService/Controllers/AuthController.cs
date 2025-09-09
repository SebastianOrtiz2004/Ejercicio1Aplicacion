using Microsoft.AspNetCore.Mvc;
using PedidoService.Models;
using PedidoService.Services;

namespace PedidoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            // Demo simple: validar usuario fijo
            if (login.Username == "test" && login.Password == "password")
            {
                // Generar token con rol ApiClient
                var token = _tokenService.BuildToken(login.Username, "ApiClient");
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

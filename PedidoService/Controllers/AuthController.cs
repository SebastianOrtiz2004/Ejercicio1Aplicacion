using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public class LoginDto
        {
            [Required] public string Username { get; set; } = default!;
            [Required] public string Password { get; set; } = default!;
        }

        [HttpPost("login")]
        [AllowAnonymous] // importante: login sin auth
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = login.Username.Trim();
            var password = login.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { error = "username y password son requeridos" });

            // Genera un token válido para cualquier usuario (rol fijo ApiClient)
            var token = _tokenService.BuildToken(username, "ApiClient");
            return Ok(new { token });
        }
    }
}

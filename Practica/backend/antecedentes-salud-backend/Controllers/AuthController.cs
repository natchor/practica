using Microsoft.AspNetCore.Mvc;
using Biblioteca.Seguridad;
using Microsoft.AspNetCore.Cors;

namespace antecedentes_salud_backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FichaService _fichaService;

        public AuthController(FichaService fichaService)
        {
            _fichaService = fichaService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var isValid = await _fichaService.ValidateEmailAsync(request.Email, request.Password);
            if (!isValid)
            {
                return Unauthorized(new { message = "Correo o contraseña no válidos" });
            }

            // Aquí puedes agregar la lógica adicional para generar un token JWT o manejar la sesión del usuario

            return Ok(new { message = "Inicio de sesión exitoso" });
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { Message = "pong" });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
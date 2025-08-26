using Microsoft.AspNetCore.Mvc;
using DesafioBMG.DTOs;
using DesafioBMG.Services;

namespace DesafioBMG.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AuthService authService) : ControllerBase
    {
        private readonly AuthService _authService = authService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _authService.Login(request.Email, request.Password);

            if (token == null)
                return Unauthorized(new { message = "Email ou senha invalidos." });

            return Ok(new { token });
        }
    }
}

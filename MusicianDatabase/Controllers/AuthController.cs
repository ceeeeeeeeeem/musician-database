using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MusicianDatabase.Service.Interfaces;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            // Authenticate the user
            var user = await _authService.AuthenticateUserAsync(loginDto.Email, loginDto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate a JWT token
            var token = _authService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
    }
}

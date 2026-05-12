using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginModel dto)
        {
            if (dto.Username == "admin" &&
               dto.Password == "123")
            {
                var tokenService = new JWTService(_config);
                var token =
                    tokenService.CreateToken(dto.Username);

                return Ok(new { token });
            }

            return Unauthorized();
        }
        [HttpGet("ping")]
        [Authorize]
        public IActionResult ping()
        {
            return Ok("Ok");
        }

    }
}

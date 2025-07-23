using APIAuthentication.DTOs;
using APIAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIAuthentication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate user credentials (hardcoded for demo)
            if (request.Username == "test" && request.Password == "password")
            {
                var token = _tokenService.GenerateTokens(request.Username);
                return Ok(new TokenResponse
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.Expiration
                });
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] TokenResponse request)
        {
            if (_tokenService.ValidateRefreshToken(request.RefreshToken, "test"))
            {
                var newAccessToken = _tokenService.GenerateAccessToken("test");
                return Ok(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = request.RefreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(15)
                });
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok(new { Data = "This is secured data." });
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using APIAuthentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace APIAuthentication.Services
{
    public class TokenService
    {
        private readonly string _secretKey = "YourSuperSecretKeyForJWTEncryption";
        private readonly Dictionary<string, string> _refreshTokens = new();

        public Token GenerateTokens(string username)
        {
            var accessToken = GenerateAccessToken(username);
            var refreshToken = GenerateRefreshToken();

            _refreshTokens[refreshToken] = username;

            return new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(15)
            };
        }

        public string GenerateAccessToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool ValidateRefreshToken(string refreshToken, string username)
        {
            return _refreshTokens.TryGetValue(refreshToken, out var storedUsername) && storedUsername == username;
        }
    }
}

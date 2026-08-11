using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrintShop.Infrastructure
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _options;
        private readonly ICartRepository _cartRepository;

        public JwtService(IOptions<JwtOptions> options, ICartRepository cartRepository)
        {
            _options = options.Value;
            _cartRepository = cartRepository;
        }

        public async Task<string> GenerateToken(User user)
        {
            var cartId = await _cartRepository.GetIdByUserId(user.Id);

            Claim[] claims = 
                [
                    new("userId", user.Id.ToString()),
                    new("cartId", cartId.ToString())
                ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpireMins)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}

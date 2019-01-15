using Microsoft.Extensions.Options;
using MyIdeasPool.Core.Models;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyIdeasPool.WebApi.Security
{
	public class JwtTokenGenerator : ITokenGenerator
	{
		private readonly IOptions<JwtAuthentication> _jwtAuthentication;

		public JwtTokenGenerator(IOptions<JwtAuthentication> jwtAuthentication)
		{
			_jwtAuthentication = jwtAuthentication;
		}

		public TokenModel Generate(User user)
		{
			var token = new JwtSecurityToken(
							issuer: _jwtAuthentication.Value.ValidIssuer,
							audience: _jwtAuthentication.Value.ValidAudience,
							claims: new[]
							{
								new Claim(JwtRegisteredClaimNames.Sub, user.Email),
								new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
							},
							expires: DateTime.UtcNow.AddHours(1),
							notBefore: DateTime.UtcNow,
							signingCredentials: _jwtAuthentication.Value.SigningCredentials);

			return new TokenModel
			{
				Jwt = new JwtSecurityTokenHandler().WriteToken(token),
				RefreshToken = GenerateToken()
			};
		}

		private string GenerateToken(int size = 32)
		{
			var randomNumber = new byte[size];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}
	}
}

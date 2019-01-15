using MyIdeasPool.Core.Domain;
using MyIdeasPool.Core.Models;
using System.Threading.Tasks;

namespace MyIdeasPool.Core.Services
{
	public interface IUserService
	{
		User CurrentUser { get; }

		void SetCurrentUser(string username);

		Task<bool> IsValidToken(string token, TokenType type);

		Task AddToken(string token, TokenType type);

		Task RevokeToken(string token, TokenType type);

		Task<User> GetUser(string refreshToken);
	}
}

using MyIdeasPool.Core.Models;

namespace MyIdeasPool.Core.Services
{
	public interface IUserService
	{
		User CurrentUser { get; }

		void SetCurrentUser(string username);
	}
}

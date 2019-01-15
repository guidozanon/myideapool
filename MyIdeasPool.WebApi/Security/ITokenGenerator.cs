using MyIdeasPool.Core.Models;
using MyIdeasPool.WebApi.Models;

namespace MyIdeasPool.WebApi.Security
{
	public interface ITokenGenerator
	{
		TokenModel Generate(User user);
	}
}
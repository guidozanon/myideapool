using Microsoft.AspNetCore.Identity;

namespace MyIdeasPool.Core.Domain
{
	public class UserEntity  : IdentityUser
	{
		public string AvatarUrl { get; set; }
		public string Name { get; set; }

	}
}

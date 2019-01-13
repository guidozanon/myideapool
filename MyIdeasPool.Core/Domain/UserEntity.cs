using Microsoft.AspNetCore.Identity;

namespace MyIdeasPool.Core.Domain
{
	class UserEntity  : IdentityUser
	{
		public string AvatarUrl { get; set; }
	}
}

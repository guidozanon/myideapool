using System;

namespace MyIdeasPool.Core.Domain
{
	class UserTokenEntity
	{
		public Guid Id { get; set; }
		public string Token { get; set; }
		public TokenType Type { get; set; }
		public virtual UserEntity User { get; set; }
		public string UserId { get; set; }
	}

	public enum TokenType
	{
		Token,
		RefreshToken
	}
}

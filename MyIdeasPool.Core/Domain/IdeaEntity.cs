using System;

namespace MyIdeasPool.Core.Domain
{
	class IdeaEntity
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public int Ease { get; set; }
		public int Confidence { get; set; }
		public int Impact { get; set; }
		public DateTime CreatedAt { get; set; }
		public virtual UserEntity Owner { get; set; }

	}
}

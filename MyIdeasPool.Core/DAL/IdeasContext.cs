
using Microsoft.EntityFrameworkCore;
using MyIdeasPool.Core.Domain;

namespace MyIdeasPool.Core.DAL
{
	class IdeasContext : DbContext
	{
		public IdeasContext() : base()
		{

		}

		public virtual DbSet<IdeaEntity> Ideas { get; set; }
	}
}

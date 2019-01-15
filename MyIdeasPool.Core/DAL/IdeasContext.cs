
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyIdeasPool.Core.Domain;

namespace MyIdeasPool.Core.DAL
{
	class IdeasContext : IdentityDbContext<UserEntity>
	{
		public IdeasContext(DbContextOptions<IdeasContext> options) : base(options)
		{

		}

		public virtual DbSet<IdeaEntity> Ideas { get; set; }

	}
}

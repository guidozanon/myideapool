using Microsoft.EntityFrameworkCore;
using MyIdeasPool.Core.DAL;
using System.Threading.Tasks;

namespace MyIdeasPool.Core
{
	public interface IInstaller
	{
		Task Install();
	}

	class DbMigrationInstaller : IInstaller
	{
		private readonly IdeasContext _context;

		public DbMigrationInstaller(IdeasContext context)
		{
			_context = context;
		}

		public async Task Install()
		{
			await _context.Database.MigrateAsync();
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using MyIdeasPool.Core.DAL;
using MyIdeasPool.Core.Services;

namespace MyIdeasPool.Core
{
	public static class CoreRegistry
	{
		public static void AddCoreRegistry(this IServiceCollection services)
		{
			services.AddScoped<IdeasContext>();
			services.AddScoped<IIdeasService, IdeasService>();
		}
	}
}

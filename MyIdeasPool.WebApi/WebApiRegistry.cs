using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyIdeasPool.WebApi.Configuration;

namespace MyIdeasPool.WebApi
{
	public static class WebApiRegistry
	{
		private const string JwtAuthenticationKey = "JwtAuthentication";
		private const string GlobalConfigurationKey = "GlobalConfiguration";
		public static void AddWebApiRegistry(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtAuthentication>(configuration.GetSection(JwtAuthenticationKey));
			services.Configure<GlobalConfiguration>(configuration.GetSection(GlobalConfigurationKey));
		}
	}
}

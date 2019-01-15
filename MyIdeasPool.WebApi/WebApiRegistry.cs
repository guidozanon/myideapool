using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyIdeasPool.Core;
using MyIdeasPool.WebApi.Configuration;
using MyIdeasPool.WebApi.Security;

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

			services.AddTransient<IMapper>(provider =>
				new MapperConfiguration(m =>
					{
						m.AddProfile<WebApiMappingProfile>();
						m.AddProfile<CoreMappingProfile>();
					}
				).CreateMapper()
			);

			services.AddSingleton<JwtTokenGenerator>();
		}
	}
}

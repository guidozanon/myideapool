using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyIdeasPool.Core.Services;
using System.Threading.Tasks;

namespace MyIdeasPool.WebApi.Security
{
	public class CustomAuthMiddleware
	{
		public const string DefaultHeader = "X-Access-Token";

		private readonly RequestDelegate _next;


		public CustomAuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, IUserService userService)
		{
			IHeaderDictionary headers = context.Request.Headers;

			if (context.Request.HttpContext.User.Identity.IsAuthenticated)
			{
				userService.SetCurrentUser(context.Request.HttpContext.User.Identity.Name);
			}

			await _next(context);
		}
	}

	public static class CustomAuthMiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CustomAuthMiddleware>();
		}
	}
}

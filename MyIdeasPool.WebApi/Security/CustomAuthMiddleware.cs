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
			var token = context.Request.Headers[DefaultHeader];

			if (context.Request.HttpContext.User.Identity.IsAuthenticated)
			{
				userService.SetCurrentUser(context.Request.HttpContext.User.Identity.Name);

				if (!await userService.IsValidToken(token, Core.Domain.TokenType.Token))
				{
					context.Response.StatusCode = 401;
					return;
				}
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

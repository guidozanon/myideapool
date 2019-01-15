using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyIdeasPool.WebApi.Security
{
	public class CustomHeaderMiddleware
	{
		public const string DefaultHeader = "X-Access-Token";

		private readonly RequestDelegate _next;

		public CustomHeaderMiddleware(RequestDelegate next)
		{
			_next = next;

		}

		public async Task Invoke(HttpContext context)
		{
			IHeaderDictionary headers = context.Request.Headers;

			if (headers.ContainsKey(DefaultHeader))
			{
				if (headers.ContainsKey("Authorization"))
				{
					headers.Add("Authorization", "Bearer " + headers[DefaultHeader]);
				}
				else
				{
					headers["Authorization"] = "Bearer " + headers[DefaultHeader];
				}
			}

			await _next(context);
		}
	}

	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomHeaderMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CustomHeaderMiddleware>();
		}
	}
}

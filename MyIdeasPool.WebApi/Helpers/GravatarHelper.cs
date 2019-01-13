using System.Security.Cryptography;
using System.Text;

namespace MyIdeasPool.WebApi.Helpers
{
	public static class GravatarHelper
	{
		private const string BaseUrl = "https://www.gravatar.com/avatar/{0}";

		public static string GenerateImageUrl(this string email)
		{
			using (var hasher = MD5.Create())
			{
				var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLower()));

				return string.Format(BaseUrl, hash);
			}
		}
	}
}

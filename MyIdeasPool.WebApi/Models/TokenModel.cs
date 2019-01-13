using Newtonsoft.Json;

namespace MyIdeasPool.WebApi.Models
{
	public class TokenModel
	{
		[JsonProperty("kwt")]
		public string Jwt { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
	}
}

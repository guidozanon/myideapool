using Newtonsoft.Json;

namespace MyIdeasPool.WebApi.Models
{
	public class RefreshTokenModel
	{
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
	}
}

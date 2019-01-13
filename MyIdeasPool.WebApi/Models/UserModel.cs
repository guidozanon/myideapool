using Newtonsoft.Json;

namespace MyIdeasPool.WebApi.Models
{
	public class UserModel
	{
		[JsonProperty("email")]
		public string Email { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("avatar_Url")]
		public string AvatarUrl { get; set; }
	}
}

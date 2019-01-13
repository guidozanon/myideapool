using System.ComponentModel.DataAnnotations;

namespace MyIdeasPool.WebApi.Models
{
	public class SignupModel
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Password { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace MyIdeasPool.WebApi.Models
{
	public class SignupModel : LoginModel
	{
		[Required]
		public string Name { get; set; }
	}
}

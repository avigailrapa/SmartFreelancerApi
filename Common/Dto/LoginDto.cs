using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[MinLength(6)]
		public string Password { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
	public class UserDto
	{
		public int Id { get; set; }
		[Required]
		public string FullName { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		public int? FreelancerId { get; set; }


	}
}

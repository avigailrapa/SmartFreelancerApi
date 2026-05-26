using System.ComponentModel.DataAnnotations;
using Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.Dto
{
	public class BecomeFreelancerDto
	{
		public int UserId { get; set; }

		public int MainCategoryId { get; set; }
		public IFormFile? ImageFile { get; set; }

		[Required]
		public string Bio { get; set; }

		[Range(20.0, double.MaxValue, ErrorMessage = "Hourly rate must be greater than 10")]
		public decimal HourlyRate { get; set; }
		public ExperienceLevel ExperienceLevel { get; set; }

		public ICollection<int> SpecializationIds { get; set; } = [];
		public ICollection<int> SkillIds { get; set; } = [];
	}
}

using Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.Dto
{
	public class BecomeFreelancerDto
	{
		public int UserId { get; set; }

		public int MainCategoryId { get; set; }
		public IFormFile? ImageFile { get; set; }
		public string Bio { get; set; }

		public decimal HourlyRate { get; set; }
		public ExperienceLevel ExperienceLevel { get; set; }

		public ICollection<int> SpecializationIds { get; set; } = [];
		public ICollection<int> SkillIds { get; set; } = [];
	}
}

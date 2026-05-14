
using Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.Dto
{
	public class FreelancerDto
	{
		public int FreelancerId { get; set; }
		public string UserName { get; set; }
		public int MainCategoryId { get; set; }
		public string? MainCategoryName { get; set; }
		public IFormFile? ImageFile { get; set; }
		public byte[]? ArrImage { get; set; }
		public string Bio { get; set; }
		public int AvailableHours { get; set; }
		public decimal HourlyRate { get; set; }
		public double AverageStars { get; set; }

		public ExperienceLevel ExperienceLevel { get; set; }
		public FreelancerStatus Status { get; set; }
		public ICollection<int>? SpecializationIds { get; set; }
		public ICollection<string>? SpecializationNames { get; set; }
		public ICollection<int>? SkillIds { get; set; }
		public ICollection<string>? SkillNames { get; set; }



	}
}

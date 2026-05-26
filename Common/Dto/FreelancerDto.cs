
using System.ComponentModel.DataAnnotations;
using Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.Dto
{
	public class FreelancerDto
	{
		public int FreelancerId { get; set; }

		[Required]
		public string UserName { get; set; }
		public int MainCategoryId { get; set; }
		public string? MainCategoryName { get; set; }
		public IFormFile? ImageFile { get; set; }
		public byte[]? ArrImage { get; set; }

		[Required]
		public string Bio { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Available hours must be at least 1")]
		public int AvailableHours { get; set; }

		public DateTime AvailableUntil { get; set; }

		[Range(10.0, double.MaxValue, ErrorMessage = "Hourly rate must be greater than 10")]

		public decimal HourlyRate { get; set; }
		public double AverageStars { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }
		public RatingDto? LatestRating { get; set; }

		public ExperienceLevel ExperienceLevel { get; set; }
		public FreelancerStatus Status { get; set; }
		public ICollection<int>? SpecializationIds { get; set; }
		public ICollection<string>? SpecializationNames { get; set; }
		public ICollection<int>? SkillIds { get; set; }
		public ICollection<string>? SkillNames { get; set; }




	}
}

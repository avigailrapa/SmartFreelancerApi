using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
	public class CreateJobDto
	{
		[Required]
		public string Title { get; set; }
		public string Description { get; set; }


		[Range(1, int.MaxValue, ErrorMessage = "Required hours must be at least 1")]
		public int RequiredHours { get; set; }

		[Required]
		public DateTime Deadline { get; set; }

		[Required]
		[Range(1.0, double.MaxValue, ErrorMessage = "Max pay per hour must be greater than 1")]
		public decimal MaxPayPerHour { get; set; }

		public int MainCategoryId { get; set; }
		public ICollection<int> RequiredSkillIds { get; set; } = [];
		public int SpecialtyCategoryId { get; set; }

	}
}

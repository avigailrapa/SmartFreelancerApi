using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
	namespace Common.Dto
	{
		public class RatingCreateDto
		{
			public int FreelancerId { get; set; }

			[Range(1, 5, ErrorMessage = "Stars must be between 1 and 5")]
			public int Stars { get; set; }
			public string Comment { get; set; }
		}
	}
}


using Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.Dto
{
	public class FreelancerDto
	{
		public int FreelancerId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public IFormFile? ImageFile { get; set; }
		public byte[]? ArrImage { get; set; }
		public string Bio { get; set; }
		public int AvailableHours { get; set; }
		public int HourlyRate { get; set; }
		public ExperienceLevel ExperienceLevel { get; set; }
		public FreelancerStatus Status { get; set; }
		public ICollection<JobDto> JobsInProgress { get; set; } = new List<JobDto>();


	}
}

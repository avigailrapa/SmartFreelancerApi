using Common.Enums;

namespace Common.Dto
{
	public class JobDto
	{
		public int JobId { get; set; }
		public int ClientId { get; set; }
		public string ClientName { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public int RequiredHours { get; set; }
		public DateTime Deadline { get; set; }
		public int MaxPayPerHour { get; set; }

		public JobStatus Status { get; set; }

		public int? AssignedFreelancerId { get; set; }
		public string? AssignedFreelancer { get; set; }

	}
}

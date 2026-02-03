using Common.Enums;
namespace Repository.Entities
{
	public class Job
	{
		public int JobId { get; set; }
		public int ClientId { get; set; }
		public User Client { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public int RequiredHours { get; set; }
		public DateTime Deadline { get; set; }
		public int MaxPayPerHour { get; set; }

		public JobStatus Status { get; set; }

		public int? AssignedFreelancerId { get; set; }
		public Freelancer? AssignedFreelancer { get; set; }

		public ICollection<Category> RequiredSkills { get; set; } = new List<Category>();
	}
}

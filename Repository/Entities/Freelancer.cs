using Common.Enums;
namespace Repository.Entities
{

	public class Freelancer
	{
		public int FreelancerId { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }

		public string Image { get; set; }
		public string Bio { get; set; }
		public int AvailableHours { get; set; }
		public int HourlyRate { get; set; }

		public ExperienceLevel ExperienceLevel { get; set; }
		public FreelancerStatus Status { get; set; }

		public ICollection<Category> Skills { get; set; } = new List<Category>();
		public ICollection<Job> JobsInProgress { get; set; } = new List<Job>();
		public ICollection<Rating> RatingsReceived { get; set; } = new List<Rating>();
		public ICollection<Proposal> ProposalsSubmitted { get; set; } = new List<Proposal>();

	}
}

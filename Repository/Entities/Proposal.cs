using Common.Enums;

namespace Repository.Entities
{
	public class Proposal
	{
		public int Id { get; set; }

		public int FreelancerId { get; set; }

		public int JobId { get; set; }

		public int Price { get; set; }
		public int EstimatedHours { get; set; }
		public string Message { get; set; }

		public ProposalStatus Status { get; set; } = ProposalStatus.Pending;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public Freelancer Freelancer { get; set; }
		public Job Job { get; set; }
	}

	

}

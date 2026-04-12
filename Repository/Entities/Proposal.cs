using Common.Enums;

namespace Repository.Entities
{
    public class Proposal
    {
        public int Id { get; set; }
        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int EstimatedHours { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalEstimatedPrice => HourlyRate * EstimatedHours;
        public string Message { get; set; }

        public ProposalStatus Status { get; set; } = ProposalStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsClientInvite { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }


    }



}

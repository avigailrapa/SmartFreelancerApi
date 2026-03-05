using Common.Enums;

namespace Common.Dto
{
    public class ProposalDto
    {
        public int Id { get; set; }
        public int FreelancerId { get; set; }
        public string FreelancerName { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public decimal HourlyRate { get; set; }
        public int EstimatedHours { get; set; }
        public decimal TotalEstimatedPrice { get; set; }
        public string Message { get; set; }
        public ProposalStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}



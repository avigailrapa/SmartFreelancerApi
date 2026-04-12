namespace Common.Dto
{
    public class InviteProposalRequest
    {
        public int FreelancerId { get; set; }
        public int JobId { get; set; }
        public decimal HourlyRate { get; set; }
        public int EstimatedHours { get; set; }
        public string Message { get; set; }
    }
}
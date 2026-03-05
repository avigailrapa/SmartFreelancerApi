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
        public decimal MaxPayPerHour { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Open;
        public int? AssignedFreelancerId { get; set; }
        public Freelancer? AssignedFreelancer { get; set; }
        public int MainCategoryId { get; set; }
        public Category MainCategory { get; set; }
        public ICollection<Category> RequiredSkills { get; set; } = [];
        public ICollection<Proposal> Proposals { get; set; } = [];

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums;
namespace Repository.Entities
{

    public class Freelancer
    {
        public int FreelancerId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int MainCategoryId { get; set; }
        public Category MainCategory { get; set; }

        public string Image { get; set; }
        public string Bio { get; set; }
        public int AvailableHours { get; set; }
        public decimal HourlyRate { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public FreelancerStatus Status { get; set; }
        public ICollection<Category> Specializations { get; set; } = [];
        public ICollection<Category> Skills { get; set; } = [];
        public ICollection<Job> JobsInProgress { get; set; } = [];
        public ICollection<Rating> RatingsReceived { get; set; } = [];
        public ICollection<Proposal> ProposalsSubmitted { get; set; } = [];

        [NotMapped]
        public double AverageStars
        {
            get
            {
                if (RatingsReceived == null || RatingsReceived.Count == 0)
                    return 0;
                return Math.Round(RatingsReceived.Average(r => r.Stars), 1);
            }
        }

    }
}

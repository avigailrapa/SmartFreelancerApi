namespace Repository.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public Freelancer? FreelancerProfile { get; set; }
		public ICollection<Job> PostedJobs { get; set; } = new List<Job>();
		public ICollection<Rating> RatingsGiven { get; set; } = new List<Rating>();

	}
}

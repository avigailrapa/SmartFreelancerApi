namespace Common.Dto
{
	public class RatingDto
	{
		public int Id { get; set; }

		public int FreelancerId { get; set; }
		public string FreelancerName { get; set; }

		public int UserId { get; set; }
		public string UserName { get; set; }

		public int Stars { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

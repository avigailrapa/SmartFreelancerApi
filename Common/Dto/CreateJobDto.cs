namespace Common.Dto
{
	public class CreateJobDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public int RequiredHours { get; set; }
		public DateTime Deadline { get; set; }
		public decimal MaxPayPerHour { get; set; }
		public int MainCategoryId { get; set; }
		public ICollection<int> RequiredSkillIds { get; set; } = [];

	}
}

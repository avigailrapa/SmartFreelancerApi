namespace Common.Dto
{
	public class CategoryDto
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public int? ParentCategoryId { get; set; }
		public string? ParentCategory { get; set; }

	}

}

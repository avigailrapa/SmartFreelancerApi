using Common.Enums;

namespace Common.Dto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<CategoryDto> SubCategories { get; set; } = [];
        public CategoryType Type { get; set; }

    }

}

using Common.Enums;

namespace Repository.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = [];
        public ICollection<Job> Jobs { get; set; } = [];
        public ICollection<Freelancer> Freelancers { get; set; } = [];

    }
}

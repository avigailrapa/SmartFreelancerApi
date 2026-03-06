using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class CategoryRepository(IContext context) : IRepository<Category>
    {
        private readonly IContext ctx = context;

        public async Task<Category> AddItem(Category category)
        {
            await ctx.Categories.AddAsync(category);
            await ctx.Save();
            return category;
        }

        public async Task DeleteItem(int id)
        {
            var c = await ctx.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (c != null)
            {
                ctx.Categories.Remove(c);
            }
            await ctx.Save();
        }

        public async Task<List<Category>> GetAll()
        {
            return await ctx.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .ThenInclude(s => s.SubCategories)
                .ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await ctx.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> UpdateItem(int id, Category category)
        {
            var c = await ctx.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (c != null)
            {
                c.Name = category.Name;
                c.ParentCategoryId = category.ParentCategoryId;
                await ctx.Save();
            }
            return c;

        }
    }
}

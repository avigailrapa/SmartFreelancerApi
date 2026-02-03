using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
	internal class RatingRepository : IRepository<Rating>
	{
		private readonly IContext ctx;
		public RatingRepository(IContext context)
		{
			ctx = context;
		}
		public async Task<Rating> AddItem(Rating rating)
		{
			await ctx.Ratings.AddAsync(rating);
			await ctx.Save();
			return rating;
		}

		public async Task DeleteItem(int id)
		{
			var r = await ctx.Ratings.FirstOrDefaultAsync(x => x.Id == id);
			if (r != null)
			{
				ctx.Ratings.Remove(r);
			}
			await ctx.Save();
		}

		public async Task<List<Rating>> GetAll()
		{
			return await ctx.Ratings
			  .Include(r => r.User)
			  .Include(r => r.Freelancer)
			  .ToListAsync();
		}

		public async Task<Rating> GetById(int id)
		{
			return await ctx.Ratings
				.Include(r => r.User)
				.Include(r => r.Freelancer)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Rating> UpdateItem(int id, Rating rating)
		{
			var r = await ctx.Ratings.FirstOrDefaultAsync(x => x.Id == id);
			if (r != null)
			{
				r.Stars = rating.Stars;
				r.Comment = rating.Comment;
				r.FreelancerId = rating.FreelancerId;
				r.UserId = rating.UserId;
				await ctx.Save();
			}
			return r;
		}
	}
}

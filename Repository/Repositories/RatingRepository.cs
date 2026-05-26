using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
	public class RatingRepository(IContext context)
	{
		private readonly IContext ctx = context;

		public async Task<Rating> AddItem(Rating rating)
		{
			await ctx.Ratings.AddAsync(rating);
			await ctx.Save();
			return rating;
		}


	}
}

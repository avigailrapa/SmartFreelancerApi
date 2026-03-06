using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class FreelancerRepository(IContext context) : IRepository<Freelancer>
    {
        private readonly IContext ctx = context;

        public async Task<Freelancer> AddItem(Freelancer freelancer)
        {
            await ctx.Freelancers.AddAsync(freelancer);
            await ctx.Save();
            return freelancer;
        }

        public async Task DeleteItem(int id)
        {
            var f = await ctx.Freelancers.FirstOrDefaultAsync(x => x.FreelancerId == id);
            if (f != null)
            {
                ctx.Freelancers.Remove(f);
                await ctx.Save();

            }
        }

        public async Task<List<Freelancer>> GetAll()
        {
            return await ctx.Freelancers
                .Include(f => f.User)
                .Include(f => f.Skills)
                    .ThenInclude(s => s.ParentCategory)
                .Include(f => f.JobsInProgress)
                .Include(f => f.RatingsReceived)
                    .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<Freelancer?> GetById(int id)
        {
            return await ctx.Freelancers
                .Include(f => f.User)
                .Include(f => f.Skills)
                .ThenInclude(s => s.ParentCategory)
                .Include(f => f.JobsInProgress)
                .Include(f => f.RatingsReceived)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(x => x.FreelancerId == id);
        }

        public async Task<Freelancer> UpdateItem(int id, Freelancer freelancer)
        {
            var f = await ctx.Freelancers.FirstOrDefaultAsync(x => x.FreelancerId == id);
            if (f != null)
            {
                f.Image = freelancer.Image;
                f.AvailableHours = freelancer.AvailableHours;
                f.HourlyRate = freelancer.HourlyRate;
                f.Status = freelancer.Status;
                f.Bio = freelancer.Bio;
                f.ExperienceLevel = freelancer.ExperienceLevel;
                await ctx.Save();
            }
            return f;
        }
    }
}

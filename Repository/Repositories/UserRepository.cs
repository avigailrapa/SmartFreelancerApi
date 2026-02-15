using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
    public class UserRepository(IContext context) : IRepository<User>
    {
        private readonly IContext ctx = context;

        public async Task<User> AddItem(User user)
        {
            await ctx.Users.AddAsync(user);
            await ctx.Save();
            return user;
        }

        public async Task DeleteItem(int id)
        {
            var u = await ctx.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (u != null)
            {
                ctx.Users.Remove(u);
            }
            await ctx.Save();

        }

        public async Task<List<User>> GetAll()
        {
            return await ctx.Users
                .Include(u => u.FreelancerProfile)
                .Include(u => u.RatingsGiven)
                    .ThenInclude(r => r.Freelancer)
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await ctx.Users
                .Include(u => u.FreelancerProfile)
                .Include(u => u.RatingsGiven)
                    .ThenInclude(r => r.Freelancer)
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<User> UpdateItem(int id, User user)
        {
            var u = await ctx.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (u != null)
            {
                u.FullName = user.FullName;
                u.Email = user.Email;
                u.Password = user.Password;
                await ctx.Save();
            }
            return u;

        }
    }



}

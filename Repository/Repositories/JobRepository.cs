using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
	public class JobRepository : IRepository<Job>
	{
		private readonly IContext ctx;
		public JobRepository(IContext context)
		{
			ctx = context;
		}
		public async Task<Job> AddItem(Job job)
		{
			await ctx.Jobs.AddAsync(job);
			await ctx.Save();
			return job;
		}

		public async Task DeleteItem(int id)
		{
			var j = await ctx.Jobs.FirstOrDefaultAsync(x => x.JobId == id);
			if (j != null)
			{
				ctx.Jobs.Remove(j);
			}
			await ctx.Save();
		}

		public async Task<List<Job>> GetAll()
		{
			return await ctx.Jobs
			  .Include(j => j.Client)
			  .Include(j => j.AssignedFreelancer)
			  .Include(j => j.RequiredSkills)
			  .ToListAsync();
		}

		public async Task<Job> GetById(int id)
		{
			return await ctx.Jobs
			   .Include(j => j.Client)
			   .Include(j => j.AssignedFreelancer)
			   .Include(j => j.RequiredSkills)
			   .FirstOrDefaultAsync(j => j.JobId == id);
		}

		public async Task<Job> UpdateItem(int id, Job job)
		{
			var j = await ctx.Jobs
				.Include(j => j.RequiredSkills)
				.FirstOrDefaultAsync(j => j.JobId == id);

			if (j != null)
			{
				j.Title = job.Title;
				j.Description = job.Description;
				j.RequiredHours = job.RequiredHours;
				j.Deadline = job.Deadline;
				j.MaxPayPerHour = job.MaxPayPerHour;
				j.Status = job.Status;
				j.AssignedFreelancerId = job.AssignedFreelancerId;
				j.RequiredSkills.Clear();
				foreach (var skill in job.RequiredSkills)
				{
					j.RequiredSkills.Add(skill);
				}
				await ctx.Save();
			}
			return j;
		}
	}
}

using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;
using Repository.Interfaces;

namespace Repository.Repositories
{
	public class JobRepository(IContext context) : IJobRepository
	{
		private readonly IContext ctx = context;

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
			  .Include(j => j.MainCategory)
			  .ToListAsync();
		}

		public async Task<Job?> GetById(int id)
		{
			return await ctx.Jobs
			   .Include(j => j.Client)
			   .Include(j => j.AssignedFreelancer)
			   .Include(j => j.RequiredSkills)
			   .Include(j => j.MainCategory)
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

		public async Task<List<Job>> GetOpenJobs()
		{
			return await ctx.Jobs
				.Include(j => j.Client)
				.Include(j => j.RequiredSkills)
				.Where(j => j.Status == JobStatus.Open)
				.ToListAsync();
		}

		public async Task<List<Job>> GetByClientId(int clientId)
		{
			return await ctx.Jobs
				.Include(j => j.RequiredSkills)
				.Where(j => j.ClientId == clientId)
				.ToListAsync();
		}
	}
}

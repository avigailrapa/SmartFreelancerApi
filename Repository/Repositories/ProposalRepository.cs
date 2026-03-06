using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class ProposalRepository(IContext context) : IProposalRepository
    {
        private readonly IContext ctx = context;

        public async Task<Proposal> Add(Proposal proposal)
        {
            await ctx.Proposals.AddAsync(proposal);
            await ctx.Save();
            return proposal;
        }


        public async Task<bool> Exists(int freelancerId, int jobId)
        {
            return await ctx.Proposals
                .AnyAsync(p => p.FreelancerId == freelancerId && p.JobId == jobId);
        }

        public async Task<List<Proposal>> GetByFreelancerId(int freelancerId)
        {
            return await ctx.Proposals
                .Include(p => p.Job)
                .Where(p => p.FreelancerId == freelancerId)
                .ToListAsync();
        }

        public async Task<Proposal?> GetById(int id)
        {
            return await ctx.Proposals
                .Include(p => p.Freelancer)
                .Include(p => p.Job)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Proposal>> GetByJobId(int jobId)
        {
            return await ctx.Proposals
                .Include(p => p.Freelancer)
                .Where(p => p.JobId == jobId)
                .ToListAsync();
        }

        public async Task<List<Proposal>> GetByUser(int userId)
        {
            return await ctx.Proposals
                .Include(p => p.Job)
                .Where(p => p.Job.ClientId == userId)
                .ToListAsync();
        }

        public async Task RejectAllByJobExcept(int jobId, int acceptedProposalId)
        {
            var proposals = await ctx.Proposals
                .Where(p => p.JobId == jobId && p.Id != acceptedProposalId)
                .ToListAsync();
            foreach (var proposal in proposals)
            {
                proposal.Status = ProposalStatus.Rejected;
            }
            await ctx.Save();

        }

        public async Task<Proposal> Update(int id, Proposal proposal)
        {
            var p = await ctx.Proposals.FirstOrDefaultAsync(p => p.Id == id);
            if (p != null)
            {
                p.HourlyRate = proposal.HourlyRate;
                p.EstimatedHours = proposal.EstimatedHours;
                p.Message = proposal.Message;
                p.Status = proposal.Status;
                await ctx.Save();
            }
            return p;
        }


    }
}

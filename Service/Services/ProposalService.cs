using AutoMapper;
using Common.Dto;
using Common.Enums;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    internal class ProposalService(IProposalRepository repository, IMapper mapper) : IProposalService
    {
        private readonly IProposalRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<ProposalDto> ApproveProposal(int proposalId)
        {
            var proposal = await repository.GetById(proposalId) ?? throw new Exception("Proposal not found");

            if (proposal.Status != ProposalStatus.Pending)
                throw new Exception("Proposal cannot be approved");

            proposal.Status = ProposalStatus.Accepted;

            proposal.Job.Status = JobStatus.InProgress;
            proposal.Job.AssignedFreelancerId = proposal.FreelancerId;

            await repository.RejectAllByJobExcept(proposal.JobId, proposalId);

            await repository.Update(proposalId, proposal);

            return mapper.Map<ProposalDto>(proposal);

        }


        public async Task<ProposalDto> GetProposalById(int proposalId)
        {
            var proposal = await repository.GetById(proposalId) ?? throw new Exception("Proposal not found");
            return mapper.Map<ProposalDto>(proposal);
        }


        public async Task<List<ProposalDto>> GetProposalsByFreelancer(int freelancerId)
        {
            var proposal = await repository.GetByFreelancerId(freelancerId);
            return mapper.Map<List<ProposalDto>>(proposal);
        }

        public async Task<List<ProposalDto>> GetProposalsByJob(int jobId)
        {
            var proposal = await repository.GetByJobId(jobId);
            return mapper.Map<List<ProposalDto>>(proposal);
        }

        public async Task RejectProposal(int proposalId)
        {
            var proposal = await repository.GetById(proposalId) ?? throw new Exception("Proposal not found");

            if (proposal.Status == ProposalStatus.Accepted)
                throw new Exception("Accepted proposal cannot be rejected");

            proposal.Status = ProposalStatus.Rejected;
            await repository.Update(proposalId, proposal);

        }

        public async Task<ProposalDto> SendProposal(int freelancerId, int jobId, int price, int hours, string message)
        {
            if (await repository.Exists(freelancerId, jobId))
                throw new Exception("Proposal already exists for this job");


            var proposal = new Proposal
            {
                FreelancerId = freelancerId,
                JobId = jobId,
                Price = price,
                EstimatedHours = hours,
                Message = message,
                Status = ProposalStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await repository.Add(proposal);
            return mapper.Map<ProposalDto>(proposal);

        }
    }
}

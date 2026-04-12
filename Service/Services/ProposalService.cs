using AutoMapper;
using Common.Dto;
using Common.Enums;
using Common.Exceptions;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    internal class ProposalService(IProposalRepository repository, IJobRepository jobRepository, IMapper mapper) : IProposalService
    {
        private readonly IProposalRepository repository = repository;
        private readonly IJobRepository jobRepository = jobRepository;
        private readonly IMapper mapper = mapper;

        public async Task<ProposalDto> ApproveProposal(int proposalId)
        {
            var proposal = await repository.GetById(proposalId) ?? throw new NotFoundException("Proposal not found");

            if (proposal.Job.Status != JobStatus.Open)
                throw new ConflictException("Job is not open");

            if (proposal.Status != ProposalStatus.Pending)
                throw new ConflictException("Proposal cannot be approved");

            proposal.Status = ProposalStatus.Accepted;

            proposal.Job.Status = JobStatus.InProgress;
            proposal.Job.AssignedFreelancerId = proposal.FreelancerId;

            await repository.RejectAllByJobExcept(proposal.JobId, proposalId);

            await repository.Update(proposalId, proposal);

            return mapper.Map<ProposalDto>(proposal);

        }


        public async Task<ProposalDto> GetProposalById(int proposalId)
        {
            var proposal = await repository.GetById(proposalId) ?? throw new NotFoundException("Proposal not found");

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

        public async Task<List<ProposalDto>> GetProposalsByUser(int userId)
        {
            var proposal = await repository.GetByUser(userId);
            return mapper.Map<List<ProposalDto>>(proposal);
        }

        public async Task<ProposalDto> InviteFreelancer(int freelancerId, int jobId, decimal price, int hours, string message, int clientId, string clientName)
        {
            var job = await jobRepository.GetById(jobId) ?? throw new NotFoundException("Job not found");

            if (job.Status != JobStatus.Open)
                throw new ConflictException("Job is not open");

            if (await repository.Exists(freelancerId, jobId))
                throw new ConflictException("Proposal or invite already exists for this job");

            var proposal = new Proposal
            {
                FreelancerId = freelancerId,
                JobId = jobId,
                HourlyRate = price,
                EstimatedHours = hours,
                Message = message,
                Status = ProposalStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                IsClientInvite = true,
                ClientId = clientId,
                ClientName = clientName

            };

            await repository.Add(proposal);
            return mapper.Map<ProposalDto>(proposal);

        }

        public async Task RejectProposal(int proposalId)
        {
            var proposal = await repository.GetById(proposalId) ?? throw new NotFoundException("Proposal not found");


            if (proposal.Status == ProposalStatus.Accepted)
                throw new ConflictException("Accepted proposal cannot be rejected");

            proposal.Status = ProposalStatus.Rejected;
            await repository.Update(proposalId, proposal);

        }

        public async Task<ProposalDto> SendProposal(int freelancerId, int jobId, decimal price, int hours, string message)
        {
            if (await repository.Exists(freelancerId, jobId))
                throw new ConflictException("Proposal already exists for this job");

            var job = await jobRepository.GetById(jobId) ?? throw new NotFoundException("Job not found");

            if (job.Status != JobStatus.Open)
                throw new ConflictException("Cannot send proposal to a job that is not open");

            var proposal = new Proposal
            {
                FreelancerId = freelancerId,
                JobId = jobId,
                HourlyRate = price,
                EstimatedHours = hours,
                Message = message,
                Status = ProposalStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                IsClientInvite = false,

            };
            await repository.Add(proposal);
            return mapper.Map<ProposalDto>(proposal);

        }


    }
}

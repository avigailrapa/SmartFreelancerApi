using AutoMapper;
using Common.Dto;
using Common.Enums;
using Common.Exceptions;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
	internal class ProposalService(IProposalRepository repository, IJobRepository jobRepository, IMapper mapper, EmailService emailService, IFreelancerService freelancerService) : IProposalService
	{
		private readonly IProposalRepository repository = repository;
		private readonly IJobRepository jobRepository = jobRepository;
		private readonly IMapper mapper = mapper;
		private readonly EmailService emailService = emailService;
		private readonly IFreelancerService freelancerService = freelancerService;

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

			await freelancerService.DeductHoursAfterJobAccepted(proposal.FreelancerId, proposal.Job.RequiredHours);

			await repository.RejectAllByJobExcept(proposal.JobId, proposalId);

			await repository.Update(proposalId, proposal);

			return mapper.Map<ProposalDto>(proposal);

		}

		public async Task DeleteProposal(int proposalId)
		{
			var proposal = await repository.GetById(proposalId) ?? throw new NotFoundException("Proposal not found");
			if (proposal.Status != ProposalStatus.Pending) throw new ConflictException("Only pending proposals can be deleted");
			await repository.Delete(proposalId);

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

		public async Task RejectProposal(int proposalId)
		{
			var proposal = await repository.GetById(proposalId) ?? throw new NotFoundException("Proposal not found");


			if (proposal.Status == ProposalStatus.Accepted)
				throw new ConflictException("Accepted proposal cannot be rejected");

			proposal.Status = ProposalStatus.Rejected;
			await repository.Update(proposalId, proposal);

		}

		public async Task<ProposalDto> SendProposal(int clientId, int freelancerId, int jobId, decimal price, int hours, string message)
		{
			if (await repository.Exists(freelancerId, jobId))
				throw new ConflictException("Proposal already exists for this job");

			var job = await jobRepository.GetById(jobId) ?? throw new NotFoundException("Job not found");

			if (job.ClientId == clientId)
				throw new ConflictException("You cannot submit a proposal to your own job post");

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
				ClientId = job.ClientId,
				ClientName = job.Client.FullName ?? ""
			};

			var addedProposal = await repository.Add(proposal);

			var fullProposal = await repository.GetById(addedProposal.Id)
				?? throw new NotFoundException("Proposal not found after creation");

			try
			{
				await emailService.SendProposalNotification(
					fullProposal.Job.Client.Email,
					fullProposal.Job.Client.FullName ?? "",
					fullProposal.Job.Title,
					fullProposal.Freelancer.User.FullName ?? ""
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine("EMAIL ERROR:");
				Console.WriteLine(ex.Message);

				if (ex.InnerException != null)
					Console.WriteLine(ex.InnerException.Message);

				throw;
			}

			return mapper.Map<ProposalDto>(fullProposal);
		}


	}
}

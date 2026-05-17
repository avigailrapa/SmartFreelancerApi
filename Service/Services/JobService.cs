using AutoMapper;
using Common.Dto;
using Common.Enums;
using Common.Exceptions;
using Repository.Entities;
using Repository.interfaces;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
	public class JobService(IJobRepository repository, IRepository<Category> categoryRepository, IMapper mapper) : IJobService
	{
		private readonly IJobRepository repository = repository;
		private readonly IRepository<Category> categoryRepository = categoryRepository;
		private readonly IMapper mapper = mapper;

		public async Task<JobDto> AddItem(CreateJobDto createJob, int clientId)
		{
			var job = new Job
			{
				Title = createJob.Title,
				Description = createJob.Description,
				RequiredHours = createJob.RequiredHours,
				Deadline = createJob.Deadline,
				MaxPayPerHour = createJob.MaxPayPerHour,
				MainCategoryId = createJob.MainCategoryId,
				ClientId = clientId,
				Status = JobStatus.Open
			};
			if (createJob.RequiredSkillIds != null && createJob.RequiredSkillIds.Count > 0)
			{
				job.RequiredSkills = [];
				foreach (var id in createJob.RequiredSkillIds)
				{
					var skill = await categoryRepository.GetById(id) ?? throw new NotFoundException($"Skill {id} not found");
					job.RequiredSkills.Add(skill);
				}
			}

			var entity = await repository.AddItem(job);
			return mapper.Map<JobDto>(entity);
		}

		public async Task DeleteItem(int id)
		{
			var exists = await repository.GetById(id) ?? throw new KeyNotFoundException("Job not found");
			await repository.DeleteItem(id);
		}

		public async Task<List<JobDto>> GetAll()
		{
			var jobs = await repository.GetAll();
			return mapper.Map<List<JobDto>>(jobs);
		}

		public async Task<List<JobDto>> GetOpenJobs()
		{
			var jobs = await repository.GetOpenJobs();
			return mapper.Map<List<JobDto>>(jobs);
		}

		public async Task<JobDto> GetById(int id)
		{
			var job = await repository.GetById(id) ?? throw new NotFoundException("Job not found");

			return mapper.Map<JobDto>(job);
		}

		public async Task<JobDto> UpdateItem(int id, JobDto job)
		{
			var exists = await repository.GetById(id) ?? throw new KeyNotFoundException("Job not found");
			var updated = await repository.UpdateItem(id, mapper.Map<Job>(job));
			return mapper.Map<JobDto>(updated);
		}

		public async Task<List<JobDto>> GetByClientId(int? clientId)
		{
			if (clientId == null) return [];
			var jobs = await repository.GetByClientId(clientId.Value) ?? throw new NotFoundException("Jobs not found");
			return mapper.Map<List<JobDto>>(jobs);

		}

		public async Task CompleteJob(int jobId, int freelancerId)
		{
			var job = await repository.GetById(jobId) ?? throw new NotFoundException("Job not found"); ;
			if (job.AssignedFreelancerId != freelancerId) throw new UnauthorizedAccessException("You are not assigned to this job");
			if (job.Status != JobStatus.InProgress) throw new ConflictException("Job is not in progress");
			job.Status = JobStatus.Completed;
			await repository.UpdateItem(jobId, job);
		}

	}
}

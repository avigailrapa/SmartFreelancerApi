using AutoMapper;
using Common.Dto;
using Common.Exceptions;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class JobService(IJobRepository repository, IMapper mapper) : IJobService
    {
        private readonly IJobRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<JobDto> AddItem(JobDto job)
        {
            var entity = await repository.AddItem(mapper.Map<Job>(job));
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
            if (clientId == null)
                return [];
            var jobs = await repository.GetByClientId(clientId.Value) ?? throw new NotFoundException("Jobs not found");
            return mapper.Map<List<JobDto>>(jobs);

        }
    }
}

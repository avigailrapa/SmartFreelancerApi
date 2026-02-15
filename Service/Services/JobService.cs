using AutoMapper;
using Common.Dto;
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
            var job = await repository.GetById(id);
            return mapper.Map<JobDto>(job);
        }

        public async Task<JobDto> UpdateItem(int id, JobDto job)
        {
            var updated = await repository.UpdateItem(id, mapper.Map<Job>(job));
            return mapper.Map<JobDto>(updated);
        }






    }
}

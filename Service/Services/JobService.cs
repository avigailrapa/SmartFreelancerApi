using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class JobService : IService<JobDto>
    {
        private readonly IRepository<Job> repository;
        private readonly IMapper mapper;

        public JobService(IRepository<Job> repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

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

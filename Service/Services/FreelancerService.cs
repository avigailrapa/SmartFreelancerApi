using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class FreelancerService : IService<FreelancerDto>
    {
        private readonly IRepository<Freelancer> repository;
        private readonly IMapper mapper;

        public FreelancerService(IRepository<Freelancer> repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

		public async Task<FreelancerDto> AddItem(FreelancerDto freelancer)
		{
			var entity = mapper.Map<Freelancer>(freelancer);
            await repository.AddItem(entity);
            var saved = await repository.GetById(entity.FreelancerId);
            return mapper.Map<FreelancerDto>(saved);
		}


		public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<FreelancerDto>> GetAll()
        {
            var freelancers = await repository.GetAll();
            return mapper.Map<List<FreelancerDto>>(freelancers);
        }

        public async Task<FreelancerDto> GetById(int id)
        {
            var freelancer = await repository.GetById(id);
            return mapper.Map<FreelancerDto>(freelancer);
        }

        public async Task<FreelancerDto> UpdateItem(int id, FreelancerDto freelancer)
        {
            var updated = await repository.UpdateItem(id, mapper.Map<Freelancer>(freelancer));
            return mapper.Map<FreelancerDto>(updated);
        }
    }
}

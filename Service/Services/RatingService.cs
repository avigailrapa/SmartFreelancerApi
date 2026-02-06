using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class RatingService : IService<RatingDto>
    {
        private readonly IRepository<Rating> repository;
        private readonly IMapper mapper;

        public RatingService(IRepository<Rating> repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<RatingDto> AddItem(RatingDto rating)
        {
            var entity = await repository.AddItem(mapper.Map<Rating>(rating));
            return mapper.Map<RatingDto>(entity);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<RatingDto>> GetAll()
        {
            var ratings = await repository.GetAll();
            return mapper.Map<List<RatingDto>>(ratings);
        }

        public async Task<RatingDto> GetById(int id)
        {
            var rating = await repository.GetById(id);
            return mapper.Map<RatingDto>(rating);
        }

        public async Task<RatingDto> UpdateItem(int id, RatingDto rating)
        {
            var updated = await repository.UpdateItem(id, mapper.Map<Rating>(rating));
            return mapper.Map<RatingDto>(updated);
        }
    }
}

using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class CategoryService(IRepository<Category> repository, IMapper mapper) : IService<CategoryDto>
    {
        private readonly IRepository<Category> repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<CategoryDto> AddItem(CategoryDto category)
        {
            var entity = await repository.AddItem(mapper.Map<Category>(category));
            return mapper.Map<CategoryDto>(entity);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var categories = await repository.GetAll();
            return mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetById(int id)
        {
            var category = await repository.GetById(id);
            return mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateItem(int id, CategoryDto category)
        {
            var updated = await repository.UpdateItem(id, mapper.Map<Category>(category));
            return mapper.Map<CategoryDto>(updated);
        }
    }
}

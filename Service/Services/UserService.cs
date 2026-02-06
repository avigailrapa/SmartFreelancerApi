using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IService<UserDto>
    {
        private readonly IRepository<User> repository;
        private readonly IMapper mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<UserDto> AddItem(UserDto user)
        {
            var entity = await repository.AddItem(mapper.Map<User>(user));
            return mapper.Map<UserDto>(entity);
        }

        public async Task DeleteItem(int id)
        {
            await repository.DeleteItem(id);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await repository.GetAll();
            return mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await repository.GetById(id);
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateItem(int id, UserDto user)
        {
            var updated = await repository.UpdateItem(id, mapper.Map<User>(user));
            return mapper.Map<UserDto>(updated);
        }
    }
}

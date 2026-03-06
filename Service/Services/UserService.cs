using AutoMapper;
using Common.Dto;
using Common.Exceptions;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService(IRepository<User> repository, IMapper mapper) : IUserService
    {
        private readonly IRepository<User> repository = repository;
        private readonly IMapper mapper = mapper;


        public async Task DeleteItem(int id)
        {
            var user = await repository.GetById(id) ?? throw new NotFoundException("User not found");
            await repository.DeleteItem(id);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await repository.GetAll();
            return mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await repository.GetById(id) ?? throw new NotFoundException("User not found");
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateItem(int id, UserDto user)
        {
            var exists = await repository.GetById(id) ?? throw new NotFoundException("User not found");
            var updated = await repository.UpdateItem(id, mapper.Map<User>(user));
            return mapper.Map<UserDto>(updated);
        }




    }
}

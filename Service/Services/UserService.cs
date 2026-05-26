using AutoMapper;
using Common.Dto;
using Common.Exceptions;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
	public class UserService(IUserRepository repository, IMapper mapper) : IUserService
	{
		private readonly IUserRepository repository = repository;
		private readonly IMapper mapper = mapper;

		public async Task DeleteItem(int id)
		{
			_ = await repository.GetById(id) ?? throw new NotFoundException("User not found");
			await repository.DeleteItem(id);
		}

		public async Task<UserDto> GetById(int id)
		{
			var user = await repository.GetById(id) ?? throw new NotFoundException("User not found");
			return mapper.Map<UserDto>(user);
		}

		public async Task<UserDto> UpdateItem(int id, UserDto userDto)
		{
			var existingUser = await repository.GetById(id)
				?? throw new NotFoundException("User not found");

			existingUser.FullName = userDto.FullName;
			existingUser.Email = userDto.Email;

			var updated = await repository.UpdateItem(id, existingUser);
			return mapper.Map<UserDto>(updated);
		}
	}
}
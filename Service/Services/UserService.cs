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

        //public async Task<List<UserDto>> GetAll()
        //{
        //    var users = await repository.GetAll();
        //    return mapper.Map<List<UserDto>>(users);
        //}

        public async Task<UserDto> GetById(int id)
        {
            var user = await repository.GetById(id) ?? throw new NotFoundException("User not found");
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateItem(int id, UserDto userDto)
        {
            // שליפה מה-Repository - ודאי ש-GetById כולל Include ל-FreelancerProfile
            // שליפה מה-Repository
            var existingUser = await repository.GetById(id)
                ?? throw new NotFoundException("User not found");

            existingUser.FullName = userDto.FullName;
            existingUser.Email = userDto.Email;

            // עדכון פרופיל פרילנסר בצורה בטוחה
            if (existingUser.FreelancerProfile != null)
            {
                // עדכון רק אם נשלח ערך חדש ב-DTO
                if (userDto.FreelancerId.HasValue)
                {
                    existingUser.FreelancerProfile.FreelancerId = userDto.FreelancerId.Value;
                }
            }

            try
            {
                var updated = await repository.UpdateItem(id, existingUser);
                return mapper.Map<UserDto>(updated);
            }
            catch (Exception ex)
            {
                // מומלץ להשתמש ב-Logger מקצועי ולא רק ב-Console
                Console.WriteLine($"Error during update: {ex.Message}");
                throw;
            }
        }



    }
}

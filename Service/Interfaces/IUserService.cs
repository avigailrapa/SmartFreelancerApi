using Common.Dto;

namespace Service.Interfaces
{
    public interface IUserService
    {
        //Task<List<UserDto>> GetAll();
        Task<UserDto> GetById(int id);
        Task<UserDto> UpdateItem(int id, UserDto item);
        Task DeleteItem(int id);
    }
}

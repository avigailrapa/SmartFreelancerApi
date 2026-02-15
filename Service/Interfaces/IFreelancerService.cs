using Common.Dto;

namespace Service.Interfaces
{
    public interface IFreelancerService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> UpdateItem(int id, T item);
        Task DeleteItem(int id);
        Task<UserDto> BecomeFreelancer(int userId, FreelancerDto freelancerDto);

    }
}

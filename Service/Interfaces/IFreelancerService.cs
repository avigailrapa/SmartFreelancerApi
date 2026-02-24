using Common.Dto;

namespace Service.Interfaces
{
    public interface IFreelancerService
    {
        Task<List<FreelancerDto>> GetAll();
        Task<FreelancerDto> GetById(int id);
        Task<FreelancerDto> UpdateItem(int id, FreelancerDto item);
        Task DeleteItem(int id);
        Task<UserDto> BecomeFreelancer(int userId, FreelancerDto freelancerDto);

    }
}

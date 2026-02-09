using Common;
using Common.Dto;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Register(UserDto user);
        Task<UserDto> Login(Login login);
        Task<UserDto> BecomeFreelancer(int userId, FreelancerDto freelancer);
        string GenerateToken(UserDto user);
    }
}

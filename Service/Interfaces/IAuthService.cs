using Common.Dto;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Register(UserDto user);
        Task<UserDto> Login(LoginDto login);
        string GenerateToken(UserDto user, bool asFreelancer);
    }
}

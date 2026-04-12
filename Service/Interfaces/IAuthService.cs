using Common.Dto;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterDto register);
        Task<UserDto> Login(LoginDto login);
        string GenerateToken(UserDto user, bool asFreelancer);
        Task DeleteAccount(int userId);
    }
}

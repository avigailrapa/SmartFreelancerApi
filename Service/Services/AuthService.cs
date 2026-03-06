using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Common.Dto;
using Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;


namespace Service.Services
{
    internal class AuthService(IRepository<User> userRepository, IMapper mapper, IConfiguration configuration) : IAuthService
    {
        private readonly IRepository<User> userRepository = userRepository;
        private readonly IMapper mapper = mapper;
        private readonly IConfiguration configuration = configuration;

        public async Task<UserDto> Login(LoginDto login)
        {
            var users = await userRepository.GetAll();
            var user = users.FirstOrDefault(users => users.FullName == login.UserName && users.Password == login.Password) ?? throw new UnauthorizedException("Invalid username or password");


            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                FreelancerId = user.FreelancerProfile?.FreelancerId
            };
        }

        public async Task<UserDto> Register(UserDto user)
        {
            var existingUser = (await userRepository.GetAll())
                .FirstOrDefault(u => u.FullName == user.FullName && u.Email == user.Email);
            if (existingUser != null)
                throw new BadRequestException("User already exists");

            var createdUser = await userRepository.AddItem(mapper.Map<User>(user));

            return new UserDto
            {
                Id = createdUser.Id,
                FullName = createdUser.FullName,
                Email = createdUser.Email,
                FreelancerId = null
            };

        }

        public string GenerateToken(UserDto u, bool asFreelancer = false)
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
               new("UserId", u.Id.ToString()),
               new(ClaimTypes.Role, "User")
            };

            if (asFreelancer && u.FreelancerId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Freelancer"));
                claims.Add(new Claim("FreelancerId", u.FreelancerId.Value.ToString()));
            }


            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}



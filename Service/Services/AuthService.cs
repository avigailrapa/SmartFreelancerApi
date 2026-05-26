using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Dto;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
	internal class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
	{
		private readonly IUserRepository userRepository = userRepository;
		private readonly IConfiguration configuration = configuration;

		public async Task<UserDto> Login(LoginDto login)
		{
			if (string.IsNullOrWhiteSpace(login?.Email) || string.IsNullOrWhiteSpace(login?.Password))
				throw new UnauthorizedException("Invalid email or password");

			var emailNormalized = login.Email.Trim().ToLowerInvariant();

			var user = await userRepository.GetByEmail(emailNormalized)
					   ?? throw new UnauthorizedException("Invalid email or password");

			var hasher = new PasswordHasher<User>();

			var result = hasher.VerifyHashedPassword(
				user,
				user.Password,
				login.Password
			);

			if (result != PasswordVerificationResult.Success)
				throw new UnauthorizedException("Invalid email or password");

			return new UserDto
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				FreelancerId = user.FreelancerProfile?.FreelancerId
			};
		}

		public async Task<UserDto> Register(RegisterDto user)
		{
			if (user == null)
				throw new BadRequestException("Invalid registration data");

			if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
				throw new BadRequestException("Email and password are required");

			var emailNormalized = user.Email.Trim().ToLowerInvariant();

			const int MinPasswordLength = 6;
			if (user.Password.Length < MinPasswordLength)
				throw new BadRequestException($"Password must be at least {MinPasswordLength} characters long");

			var existingUser = (await userRepository.GetAll())
				.FirstOrDefault(u => u.Email.Trim().ToLowerInvariant() == emailNormalized);

			if (existingUser != null)
				throw new BadRequestException("User already exists");

			var newUser = new User
			{
				FullName = user.FullName?.Trim(),
				Email = emailNormalized
			};

			var hasher = new PasswordHasher<User>();
			newUser.Password = hasher.HashPassword(newUser, user.Password);

			var createdUser = await userRepository.AddItem(newUser);

			return new UserDto
			{
				Id = createdUser.Id,
				FullName = createdUser.FullName,
				Email = createdUser.Email
			};
		}



		public string GenerateToken(UserDto u)
		{
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
			var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
			{
			  new(ClaimTypes.NameIdentifier, u.Id.ToString()),
			  new("UserId", u.Id.ToString()),
			  new(ClaimTypes.Role, "User")
			};


			if (u.FreelancerId.HasValue)
			{
				claims.Add(new Claim(ClaimTypes.Role, "Freelancer"));
				claims.Add(new Claim("FreelancerId", u.FreelancerId.Value.ToString()));
			}

			var token = new JwtSecurityToken(
				configuration["Jwt:Issuer"],
				configuration["Jwt:Audience"],
				claims,
				expires: DateTime.UtcNow.AddDays(7),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}
}



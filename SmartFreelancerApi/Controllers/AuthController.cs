using Common.Dto;
using Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		private readonly IAuthService authService = authService;

		// POST: api/<AuthController>/login

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto login, [FromQuery] bool asFreelancer = false)
		{
			var user = await authService.Login(login);

			if (asFreelancer && user.FreelancerId == null)
				throw new BadRequestException("User is not a freelancer");

			var token = authService.GenerateToken(user);
			return Ok(new { Token = token, User = user });
		}


		// POST: api/<AuthController>/register

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto register)
		{

			var newUser = await authService.Register(register);
			var token = authService.GenerateToken(newUser);
			return Ok(new { Token = token, User = newUser });
		}

		// DELETE: api/<AuthController>/delete

		[Authorize]
		[HttpDelete("delete")]
		public async Task<IActionResult> DeleteAccount()
		{
			var userIdClaim = (User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value) ?? throw new UnauthorizedException("Invalid token");
			int userId = int.Parse(userIdClaim);
			await authService.DeleteAccount(userId);

			return Ok(new { Message = "User account deleted successfully" });
		}



	}









}

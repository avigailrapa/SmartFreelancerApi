using Common.Dto;
using Common.Exceptions;
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
        public async Task<IActionResult> Login([FromBody] LoginDto login, bool asFreelancer = false)
        {
            var user = await authService.Login(login);

            if (asFreelancer && user.FreelancerId == null)
                throw new BadRequestException("User is not a freelancer");

            var token = authService.GenerateToken(user, asFreelancer);
            return Ok(new { Token = token, User = user });
        }


        // POST: api/<AuthController>/register

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {

            var newUser = await authService.Register(userDto);
            var token = authService.GenerateToken(userDto, false);
            return Ok(new { Token = token, User = userDto });
        }


    }









}

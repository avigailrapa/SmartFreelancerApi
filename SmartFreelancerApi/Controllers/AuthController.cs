using Common.Dto;
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
            try
            {
                var user = await authService.Login(login);

                if (asFreelancer && user.FreelancerId == null)
                    return BadRequest("User is not a freelancer");

                var token = authService.GenerateToken(user, asFreelancer);
                return Ok(new { Token = token, User = user });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }


        // POST: api/<AuthController>/register

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                var newUser = await authService.Register(userDto);
                var token = authService.GenerateToken(userDto, false);
                return Ok(new { Token = token, User = userDto });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }









}

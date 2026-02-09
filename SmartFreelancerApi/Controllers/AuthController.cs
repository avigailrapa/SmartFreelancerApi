using Common;
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
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var user = await authService.Login(login);
                var token = authService.GenerateToken(user);
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
                var token = authService.GenerateToken(userDto);
                return Ok(new { Token = token, User = userDto });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IService<UserDto> service;
		private IConfiguration configuration;
		private readonly IsExist<UserDto> isExist;

		public UserController(IService<UserDto> service, IConfiguration configuration, IsExist<UserDto> isExist)
        {
            this.service= service;
            this.configuration = configuration;
            this.isExist = isExist;
		}
		[HttpPost("login")]
		public IActionResult Login([FromBody] Login login)
		{
			UserDto u = isExist.Exist(login);
			if (u != null)
				return Ok(new { Token = GenerateToken(u) });
			return Unauthorized("User does not exist");
		}


		// GET: api/<UserController>
		[HttpGet]
        public async Task<List<UserDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<UserDto> Get(int id)
        {
            return await service.GetById(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<UserDto> Post([FromBody] UserDto user)
        {
            return await service.AddItem(user);
		}

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<UserDto> Put(int id, [FromBody] UserDto user)
        {
            return await service.UpdateItem(id, user);
		}

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
		}
		private string GenerateToken(UserDto u)
		{
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
			var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
			var claims = new[] {
			new Claim(ClaimTypes.Name,u.FullName),
			new Claim(ClaimTypes.Role,"user"),
		    new Claim("IsFreelancer", u.FreelancerId != null ? "true" : "false")

			 };
			var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(15),
				signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

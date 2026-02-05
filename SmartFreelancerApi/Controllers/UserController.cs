using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IService<UserDto> service;
        public UserController(IService<UserDto> service)
        {
            this.service= service;
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
    }
}

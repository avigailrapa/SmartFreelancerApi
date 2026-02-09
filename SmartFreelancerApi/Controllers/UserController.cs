using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IService<UserDto> service, IConfiguration configuration) : ControllerBase
    {
        private readonly IService<UserDto> service = service;

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

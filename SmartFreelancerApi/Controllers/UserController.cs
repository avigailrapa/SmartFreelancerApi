using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {
        private readonly IUserService service = service;

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<UserDto>> Get() => await service.GetAll();


        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<UserDto> Get(int id) => await service.GetById(id);


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<UserDto> Put(int id, [FromBody] UserDto user) => await service.UpdateItem(id, user);


        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id) => await service.DeleteItem(id);


    }
}

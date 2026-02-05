using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IService<RatingDto> service;
        public RatingController(IService<RatingDto> service)
        {
            this.service = service;
		}
        // GET: api/<RatingController>
        [HttpGet]
        public async Task<List<RatingDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<RatingController>/5
        [HttpGet("{id}")]
        public async Task<RatingDto> Get(int id)
        {
            return await service.GetById(id);
        }

        // POST api/<RatingController>
        [HttpPost]
        public async Task<RatingDto> Post([FromBody] RatingDto rating)
        {
            return await service.AddItem(rating);
		}

        // PUT api/<RatingController>/5
        [HttpPut("{id}")]
        public async Task<RatingDto> Put(int id, [FromBody] RatingDto rating)
        {
            return await service.UpdateItem(id, rating);
		}

        // DELETE api/<RatingController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
		}
    }
}

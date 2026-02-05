using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
		private readonly IService<JobDto> service;
        public JobController(IService<JobDto> service)
        {
            this.service = service;
		}

        // GET: api/<ValuesController1>
        [HttpGet]
        public async Task<List<JobDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<ValuesController1>/5
        [HttpGet("{id}")]
        public async Task<JobDto> Get(int id)
        {
            return await service.GetById(id);
        }

        // POST api/<ValuesController1>
        [HttpPost]
        public async Task<JobDto> Post([FromBody] JobDto job)
        {
            return await service.AddItem(job);
		}

        // PUT api/<ValuesController1>/5
        [HttpPut("{id}")]
        public async Task<JobDto> Put(int id, [FromBody] JobDto job)
        {
            return await service.UpdateItem(id, job);
		}

        // DELETE api/<ValuesController1>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
		}
    }
}

using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SmartFreelancerApi.Extensions;

namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController(IJobService service) : ControllerBase
    {
        private readonly IJobService service = service;


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
        [Authorize(Roles = "User")]

        public async Task<IActionResult> Post([FromBody] JobDto job)
        {
            job.ClientId = User.GetUserId() ?? throw new UnauthorizedAccessException();
            var addedJob = await service.AddItem(job);

            return Ok(addedJob);
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

        // GET api/<ValuesController1>/open
        [HttpGet("open")]
        public async Task<List<JobDto>> GetOpenJobs()
        {
            return await service.GetOpenJobs();
        }
    }
}

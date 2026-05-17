using Common.Dto;
using Common.Exceptions;
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


		// GET: api/job
		[HttpGet]
		public async Task<List<JobDto>> Get() => await service.GetAll();

		// GET api/job/5
		[HttpGet("{id}")]
		public async Task<JobDto> Get(int id) => await service.GetById(id);


		// POST api/job
		[HttpPost]
		[Authorize(Roles = "User")]

		public async Task<JobDto> Post([FromBody] CreateJobDto job)
		{
			int clientId = User.GetUserId() ?? throw new UnauthorizedAccessException();
			return await service.AddItem(job, clientId);

		}

		// PUT api/job/5
		[HttpPut("{id}")]
		public async Task<JobDto> Put(int id, [FromBody] JobDto job) => await service.UpdateItem(id, job);

		// DELETE api/job/5
		[HttpDelete("{id}")]
		public async Task Delete(int id) => await service.DeleteItem(id);


		// GET api/job/open
		[HttpGet("open")]
		public async Task<List<JobDto>> GetOpenJobs() => await service.GetOpenJobs();

		[Authorize]
		[HttpGet("my-jobs")]
		public async Task<List<JobDto>> GetMyJobs() => await service.GetByClientId(User.GetUserId());
		[HttpPut("{jobId}/complete")]
		public async Task CompleteJob(int jobId)
		{
			var freelancerId = User.GetFreelancerId() ?? throw new UnauthorizedException("Freelancer not found");
			await service.CompleteJob(jobId, freelancerId);
		}


	}

}

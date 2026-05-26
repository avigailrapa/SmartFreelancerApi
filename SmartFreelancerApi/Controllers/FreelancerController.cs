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
	public class FreelancerController(IFreelancerService service, IAuthService authService, IUserService userService) : ControllerBase
	{
		private readonly IAuthService authService = authService;
		private readonly IFreelancerService service = service;
		private readonly IUserService userService = userService;


		// GET: api/<FreelancerController>
		[HttpGet]
		public async Task<List<FreelancerDto>> Get() => await service.GetAll();


		// GET api/<FreelancerController>/5
		[HttpGet("{id}")]
		public async Task<FreelancerDto> Get(int id) => await service.GetById(id);


		// POST: api/<FreelancerController>/become-freelancer/5
		[Authorize(Roles = "User")]
		[HttpPost("become-freelancer")]
		public async Task<IActionResult> BecomeFreelancer([FromForm] BecomeFreelancerDto freelancerDto)
		{
			var userId = User.GetUserId() ?? throw new UnauthorizedException("User is not logged in");

			var updatedUser = await service.BecomeFreelancer(userId, freelancerDto);
			var token = authService.GenerateToken(updatedUser);

			return Ok(new { Token = token, User = updatedUser });
		}



		// PUT api/<FreelancerController>
		[HttpPut]
		public async Task<FreelancerDto> Put([FromBody] FreelancerDto freelancer)
		{
			return await service.UpdateItem(User.GetFreelancerId().Value, freelancer);
		}

		//// PUT api/<FreelancerController>/availability
		//[HttpPut("availability")]
		//public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityDto dto)
		//{
		//	var freelancerId = User.GetUserId();

		//	await service.UpdateAvailability((int)freelancerId, dto);

		//	return NoContent();

		//}




	}
}

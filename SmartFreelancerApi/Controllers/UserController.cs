using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SmartFreelancerApi.Extensions;


namespace SmartFreelancerApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(IUserService service) : ControllerBase
	{
		private readonly IUserService service = service;


		// PUT api/<UserController>
		[HttpPut]
		public async Task<UserDto> Put([FromBody] UserDto user)
		{
			return await service.UpdateItem(User.GetUserId().Value, user);
		}


		// DELETE api/<UserController>
		[HttpDelete]
		public async Task Delete(int id) => await service.DeleteItem(User.GetUserId().Value);


	}
}

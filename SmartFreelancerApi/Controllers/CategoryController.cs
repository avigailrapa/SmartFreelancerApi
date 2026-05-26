using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController(IService<CategoryDto> service) : ControllerBase
	{
		private readonly IService<CategoryDto> service = service;

		// GET: api/<CategoryController>
		[HttpGet]
		public async Task<List<CategoryDto>> Get() => await service.GetAll();


		// GET api/<CategoryController>/5
		[HttpGet("{id}")]
		public async Task<CategoryDto> Get(int id) => await service.GetById(id);


	}
}

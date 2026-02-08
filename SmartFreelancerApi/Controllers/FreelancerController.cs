using System.Text;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController : ControllerBase
    {
        private readonly IService<FreelancerDto> service;
        public FreelancerController(IService<FreelancerDto> service)
        {
            this.service = service;
		}
        // GET: api/<FreelancerController>
        [HttpGet]
        public async Task<List<FreelancerDto>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<FreelancerController>/5
        [HttpGet("{id}")]
        public async Task<FreelancerDto> Get(int id)
        {
            return await service.GetById(id);
        }

        // POST api/<FreelancerController>
        [HttpPost]
        public async Task<FreelancerDto> Post([FromForm] FreelancerDto freelancer)
        {
			if (freelancer.ImageFile != null)
			{
				var fileName = Guid.NewGuid() + Path.GetExtension(freelancer.ImageFile.FileName);
				var path = Path.Combine(Environment.CurrentDirectory, "Images/", fileName);

				using var fs = new FileStream(path, FileMode.Create);
				await freelancer.ImageFile.CopyToAsync(fs);

				freelancer.ArrImage = Encoding.UTF8.GetBytes(fileName);
			}

            return await service.AddItem(freelancer);
		}

		// PUT api/<FreelancerController>/5
		[HttpPut("{id}")]
        public async Task<FreelancerDto> Put(int id, [FromBody] FreelancerDto freelancer)
        {
            return await service.UpdateItem(id, freelancer);
		}

        // DELETE api/<FreelancerController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await service.DeleteItem(id);
		}
    }
}

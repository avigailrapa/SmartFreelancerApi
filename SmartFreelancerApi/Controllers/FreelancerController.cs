using System.Text;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController(IService<FreelancerDto> service, IAuthService authService) : ControllerBase
    {
        private readonly IService<FreelancerDto> service = service;
        private readonly IAuthService authService = authService;


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

        // POST: api/<FreelancerController>/become-freelancer/5
        [HttpPost("become-freelancer/{userId}")]
        public async Task<IActionResult> BecomeFreelancer(int userId, [FromForm] FreelancerDto freelancerDto)
        {
            try
            {
                if (freelancerDto.ImageFile != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(freelancerDto.ImageFile.FileName);
                    var path = Path.Combine(Environment.CurrentDirectory, "Images/", fileName);
                    using var fs = new FileStream(path, FileMode.Create);
                    await freelancerDto.ImageFile.CopyToAsync(fs);

                    freelancerDto.ArrImage = Encoding.UTF8.GetBytes(fileName);
                }

                var updatedUser = await authService.BecomeFreelancer(userId, freelancerDto);

                var token = authService.GenerateToken(updatedUser);
                return Ok(new { Token = token, User = updatedUser });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

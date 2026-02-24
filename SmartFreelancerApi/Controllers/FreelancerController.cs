using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SmartFreelancerApi.Extensions;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController(IFreelancerService service, IAuthService authService) : ControllerBase
    {
        private readonly IAuthService authService = authService;
        private readonly IFreelancerService service = service;


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
        [Authorize(Roles = "User")]
        [HttpPost("become-freelancer")]
        public async Task<IActionResult> BecomeFreelancer([FromForm] FreelancerDto freelancerDto)
        {
            try
            {
                var userId = User.GetUserId();
                if (userId == null)
                    return Unauthorized();

                var updatedUser = await service.BecomeFreelancer(userId.Value, freelancerDto);
                var token = authService.GenerateToken(updatedUser, true);

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

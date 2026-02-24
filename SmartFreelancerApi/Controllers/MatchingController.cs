using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SmartFreelancerApi.Extensions;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController(IMatchingService matching) : ControllerBase
    {
        private readonly IMatchingService matchingService = matching;

        // GET: api/<MatchingController>/{freelancerId}/optimal-jobs

        [HttpGet("optimal-jobs")]
        public async Task<ActionResult<List<JobDto>>> GetOptimalJobsForFreelancer()
        {
            try
            {
                var freelancerId = User.GetFreelancerId();
                if (freelancerId == null) return Unauthorized();

                var optimalJobs = await matchingService.GetOptimalJobsForFreelancer(freelancerId.Value);
                return Ok(optimalJobs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}

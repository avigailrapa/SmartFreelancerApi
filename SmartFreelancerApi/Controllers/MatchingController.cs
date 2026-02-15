using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController(IMatchingService matching) : ControllerBase
    {
        private readonly IMatchingService matchingService = matching;

        // GET: api/MatchingController/{freelancerId}/optimal-jobs

        [HttpGet("{freelancerId}/optimal-jobs")]
        public async Task<ActionResult<List<JobDto>>> GetOptimalJobsForFreelancer(int freelancerId)
        {
            try
            {
                var optimalJobs = await matchingService.GetOptimalJobsForFreelancer(freelancerId);
                return Ok(optimalJobs);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}

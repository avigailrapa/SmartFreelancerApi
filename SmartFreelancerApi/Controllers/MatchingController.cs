using Common.Dto;
using Common.Exceptions;
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
            var freelancerId = User.GetFreelancerId() ?? throw new UnauthorizedException("User is not logged in");
            return await matchingService.GetOptimalJobsForFreelancer(freelancerId);
        }

    }
}

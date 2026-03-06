using Common.Dto;
using Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using SmartFreelancerApi.Extensions;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController(RatingService service) : ControllerBase
    {
        private readonly RatingService service = service;

        // POST api/<RatingController>
        [Authorize(Roles = "User")]

        [HttpPost]
        public async Task<RatingDto> Post([FromBody] RatingDto rating)
        {
            rating.UserId = User.GetUserId() ?? throw new UnauthorizedException("User is not logged in");
            return await service.AddRating(rating);
        }
    }

}
using System.Security.Claims;

namespace SmartFreelancerApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int id) ? id : null;
        }

        public static int? GetFreelancerId(this ClaimsPrincipal user)
        {
            var freelancerIdClaim = user.FindFirst("FreelancerId")?.Value;
            return int.TryParse(freelancerIdClaim, out int id) ? id : null;
        }
    }
}

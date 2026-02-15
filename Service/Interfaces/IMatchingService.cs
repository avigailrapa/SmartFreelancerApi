using Common.Dto;

namespace Service.Interfaces
{
    public interface IMatchingService
    {
        public Task<List<JobDto>> GetOptimalJobsForFreelancer(int freelancerId);
    }
}

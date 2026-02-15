using Common.Dto;

namespace Service.Interfaces
{
    public interface IJobService : IService<JobDto>
    {
        public Task<List<JobDto>> GetOpenJobs();
    }
}

using Repository.Entities;
using Repository.interfaces;

namespace Repository.Interfaces
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<List<Job>> GetOpenJobs();
        Task<List<Job>> GetByClientId(int clientId);

    }
}

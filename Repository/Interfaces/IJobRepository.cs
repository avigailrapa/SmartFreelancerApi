using Repository.Entities;

namespace Repository.Interfaces
{
	public interface IJobRepository : IRepository<Job>
	{
		Task<List<Job>> GetOpenJobs();
		Task<List<Job>> GetByClientId(int clientId);
		Task<List<Job>> GetByFreelancerId(int freelancerId);
		Task MarkAsCompleted(int jobId);


	}
}

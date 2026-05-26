using Common.Dto;

namespace Service.Interfaces
{
	public interface IJobService
	{
		Task<List<JobDto>> GetAll();
		Task<JobDto> GetById(int id);
		Task DeleteItem(int id);
		public Task<List<JobDto>> GetOpenJobs();
		public Task<List<JobDto>> GetByClientId(int? clientId);

		public Task<List<JobDto>> GetByFreelancerId(int? freelancerId);
		public Task<JobDto> AddItem(CreateJobDto createJob, int clientId);
		Task<JobDto> MarkAsCompleted(int jobId, int freelancerId);
	}
}

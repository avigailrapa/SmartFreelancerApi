using Common.Dto;

namespace Service.Interfaces
{
	public interface IJobService
	{
		Task<List<JobDto>> GetAll();
		Task<JobDto> GetById(int id);
		Task<JobDto> UpdateItem(int id, JobDto item);
		Task DeleteItem(int id);
		public Task<List<JobDto>> GetOpenJobs();
		public Task<List<JobDto>> GetByClientId(int? clientId);
		public Task<JobDto> AddItem(CreateJobDto createJob, int clientId);
		public Task CompleteJob(int jobId, int freelancerId);

	}
}

using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IProposalRepository
    {
        Task<Proposal?> GetById(int id);
        Task<Proposal> Add(Proposal proposal);
        Task<Proposal> Update(int id, Proposal proposal);
        Task<List<Proposal>> GetByJobId(int jobId);
        Task<List<Proposal>> GetByFreelancerId(int freelancerId);
        Task<bool> Exists(int freelancerId, int jobId);
        Task RejectAllByJobExcept(int jobId, int acceptedProposalId);
        Task<List<Proposal>> GetByUser(int userId);


    }
}
using Common.Dto;

namespace Service.Interfaces
{
    public interface IProposalService
    {
        Task<ProposalDto> SendProposal(int freelancerId, int jobId, int price, int hours, string message);
        Task<ProposalDto> ApproveProposal(int proposalId);
        Task RejectProposal(int proposalId);
        Task<List<ProposalDto>> GetProposalsByJob(int jobId);
        Task<List<ProposalDto>> GetProposalsByFreelancer(int freelancerId);
        Task<ProposalDto> GetProposalById(int proposalId);
    }


}

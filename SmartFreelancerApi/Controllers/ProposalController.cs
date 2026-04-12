using Common.Dto;
using Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using SmartFreelancerApi.Extensions;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController(IProposalService service) : ControllerBase
    {
        private readonly IProposalService service = service;


        // GET api/<ProposalController>/5
        [HttpGet("{id}")]
        public async Task<ProposalDto> GetById(int id) => await service.GetProposalById(id);


        // GET api/<ProposalController>/my-proposals
        [Authorize(Roles = "Freelancer")]
        [HttpGet("my-proposals")]
        public async Task<List<ProposalDto>> GetMyProposals()
        {
            var freelancerId = User.GetFreelancerId() ?? throw new UnauthorizedException("User is not logged in");
            return await service.GetProposalsByFreelancer(freelancerId);
        }

        // GET api/<ProposalController>/job/5
        [HttpGet("job/{jobId}")]
        public async Task<List<ProposalDto>> GetByJob(int jobId) => await service.GetProposalsByJob(jobId);


        // GET api/<ProposalController>/user/5
        [Authorize(Roles = "User")]
        [HttpGet("my-jobs/proposals")]
        public async Task<List<ProposalDto>> GetAllUserProposals()
        {
            var userId = User.GetUserId() ?? throw new UnauthorizedException("User is not logged in"); ;
            return await service.GetProposalsByUser(userId);
        }

        // POST api/ProposalController/send
        [Authorize(Roles = "Freelancer")]
        [HttpPost("send")]
        public async Task<ProposalDto> SendProposal([FromBody] ProposalDto proposalDto)
        {
            var freelancerId = User.GetFreelancerId() ?? throw new UnauthorizedException("User is not logged in"); ;
            return await service.SendProposal(freelancerId, proposalDto.JobId, proposalDto.HourlyRate, proposalDto.EstimatedHours, proposalDto.Message);
        }


        // POST api/ProposalController/5/approve
        [HttpPost("{proposalId}/approve")]
        public async Task<ProposalDto> ApproveProposal(int proposalId) => await service.ApproveProposal(proposalId);


        // POST api/proposal/5/reject
        [HttpPost("{proposalId}/reject")]
        public async Task RejectProposal(int proposalId) => await service.RejectProposal(proposalId);

        // POST api/proposal/invite
        [Authorize(Roles = "User")]
        [HttpPost("invite")]
        public async Task<ProposalDto> InviteFreelancer([FromBody] InviteProposalRequest request)
        {
            var clientId = User.GetUserId() ?? throw new UnauthorizedException("User is not logged in");
            var clientName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown";
            return await service.InviteFreelancer(request.FreelancerId, request.JobId, request.HourlyRate, request.EstimatedHours, request.Message, clientId, clientName);
        }



    }

}

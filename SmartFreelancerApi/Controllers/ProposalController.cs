using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController(IProposalService service, IAuthService authService) : ControllerBase
    {

        private readonly IAuthService authService = authService;
        private readonly IProposalService service = service;


        // GET api/<ProposalController>/5
        [HttpGet("{id}")]
        public async Task<ProposalDto> GetById(int id)
        {
            return await service.GetProposalById(id);
        }

        // GET api/<ProposalController>/freelancer/5
        [HttpGet("freelancer/{freelancerId}")]
        public async Task<List<ProposalDto>> GetByFreelancer(int freelancerId)
        {
            return await service.GetProposalsByFreelancer(freelancerId);
        }

        // GET api/<ProposalController>/job/5
        [HttpGet("job/{jobId}")]
        public async Task<List<ProposalDto>> GetByJob(int jobId)
        {
            return await service.GetProposalsByJob(jobId);
        }

        // POST api/ProposalController/send?freelancerId=5&jobId=10
        [HttpPost("send")]
        public async Task<IActionResult> SendProposal([FromBody] ProposalDto proposalDto)
        {
            try
            {
                var proposal = await service.SendProposal(proposalDto.FreelancerId, proposalDto.JobId, proposalDto.Price, proposalDto.EstimatedHours, proposalDto.Message);
                return Ok(proposal);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // POST api/ProposalController/5/approve
        [HttpPost("{proposalId}/approve")]
        public async Task<IActionResult> ApproveProposal(int proposalId)
        {
            try
            {
                var proposal = await service.ApproveProposal(proposalId);
                return Ok(proposal);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        // POST api/proposal/5/reject
        [HttpPost("{proposalId}/reject")]
        public async Task RejectProposal(int proposalId)
        {
            await service.RejectProposal(proposalId);
        }

    }

}

using Common.Dto;
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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var proposal = await service.GetProposalById(id);
                return Ok(proposal);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET api/<ProposalController>/my-proposals
        [Authorize(Roles = "Freelancer")]
        [HttpGet("my-proposals")]
        public async Task<IActionResult> GetMyProposals()
        {
            try
            {
                var freelancerId = User.GetFreelancerId();
                if (freelancerId == null)
                    return Unauthorized();
                var proposals = await service.GetProposalsByFreelancer(freelancerId.Value);
                return Ok(proposals);


            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        // GET api/<ProposalController>/job/5
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJob(int jobId)
        {
            var proposals = await service.GetProposalsByJob(jobId);
            return Ok(proposals);

        }

        // GET api/<ProposalController>/user/5
        [Authorize(Roles = "User")]
        [HttpGet("my-jobs/proposals")]
        public async Task<IActionResult> GetAllUserProposals()
        {
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized();

            var proposals = await service.GetProposalsByUser(userId.Value);
            return Ok(proposals);

        }

        // POST api/ProposalController/send
        [Authorize(Roles = "Freelancer")]
        [HttpPost("send")]
        public async Task<IActionResult> SendProposal([FromBody] ProposalDto proposalDto)
        {
            try
            {
                var freelancerId = User.GetFreelancerId();
                if (freelancerId == null)
                    return Unauthorized();

                var proposal = await service.SendProposal(freelancerId.Value, proposalDto.JobId, proposalDto.Price, proposalDto.EstimatedHours, proposalDto.Message);
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
        public async Task<IActionResult> RejectProposal(int proposalId)
        {
            try
            {
                await service.RejectProposal(proposalId);
                return Ok(new { message = "Proposal rejected successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }

}

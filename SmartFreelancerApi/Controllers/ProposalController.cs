using Microsoft.AspNetCore.Mvc;


namespace SmartFreelancerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        // GET: api/<ProposalController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProposalController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProposalController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProposalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProposalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

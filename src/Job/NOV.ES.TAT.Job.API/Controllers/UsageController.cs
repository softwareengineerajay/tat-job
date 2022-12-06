using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Common.Exception;
using NOV.ES.TAT.Job.API.Application.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;
using System.Net;

namespace NOV.ES.TAT.Job.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsageController : Controller
    {

        private readonly IQueryBus queryBus;

        public UsageController(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }
        [HttpGet]
        [Route("{jobNumber:int}")]
        [ProducesResponseType(typeof(UsageDetailsView), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<UsageDetailsView>>> GetUsageDetailsByJobNumber([FromRoute] int jobNumber)
        {
            GetUsageDetailsByJobNumberQuery getUsageByJobIdQuery = new GetUsageDetailsByJobNumberQuery(jobNumber);
            var result = await queryBus.Send<GetUsageDetailsByJobNumberQuery, IEnumerable<UsageDetailsView>>(getUsageByJobIdQuery);

            if (result == null)
                return NotFound($"Usage Details with jobNumber:{jobNumber} not found");

            return Ok(result);
        }
    }
}

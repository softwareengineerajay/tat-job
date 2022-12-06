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
    public class FieldTransferController : Controller
    {

        private readonly IQueryBus queryBus;

        public FieldTransferController(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }
        [HttpGet]
        [Route("{jobNumber:int}")]
        [ProducesResponseType(typeof(FieldTransferSlipDetailsView), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<FieldTransferSlipDetailsView>>> GetFieldTransferDetailsByJobNumber([FromRoute] int jobNumber)
        {
            GetFieldTransferDetailsByJobNumberQuery getFieldByJobNumberQuery = new GetFieldTransferDetailsByJobNumberQuery(jobNumber);
            var result = await queryBus.Send<GetFieldTransferDetailsByJobNumberQuery, IEnumerable<FieldTransferSlipDetailsView>>(getFieldByJobNumberQuery);

            if (result == null)
                return NotFound($"Field Transfer Details with jobNumber:{jobNumber} not found");

            return Ok(result);
        }
    }
}

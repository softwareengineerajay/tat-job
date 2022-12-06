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
    //[Authorize]
    public class SalesOrderController : Controller
    {

        private readonly IQueryBus queryBus;

        public SalesOrderController(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }
        [HttpGet]
        [Route("{jobNumber:int}")]
        [ProducesResponseType(typeof(SalesOrderDetailsView), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<SalesOrderDetailsView>>> GetUsageDetailsByJobNumber([FromRoute] int jobNumber)
        {
            GetSalesOrderDetailsByJobNumberQuery getSalesByJobIdQuery = new GetSalesOrderDetailsByJobNumberQuery(jobNumber);
            var result = await queryBus.Send<GetSalesOrderDetailsByJobNumberQuery, IEnumerable<SalesOrderDetailsView>>(getSalesByJobIdQuery);

            if (result == null)
                return NotFound($"Sales Order Details with jobNumber:{jobNumber} not found");

            return Ok(result);
        }
    }
}

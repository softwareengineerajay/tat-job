using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Common.Exception;
using NOV.ES.TAT.Job.API.Application.Commands;
using NOV.ES.TAT.Job.API.Application.Queries;
using NOV.ES.TAT.Job.API.Constant;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Infrastructure.Helper;
using System.Net;

namespace NOV.ES.TAT.Job.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = ConstantsProperty.MIX_AUTH_SCHEME)]
    public class NovJobsController : ControllerBase
    {
        private readonly IQueryBus queryBus;
        private readonly ICommandBus commandBus;

        public NovJobsController(ICommandBus commandBus,
            IQueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        /// <summary>
        /// This method returns service name.
        /// </summary>
        /// <returns>ServiceName</returns>
        [HttpGet]
        [Route("ServiceName")]
        //[FeatureToggle("ADD_CTS")]
        public IActionResult ServiceName()
        {
            return Ok("NovJob API Service.");
        }

        /// <summary>
        /// This method returns all NovJobs 
        /// </summary>
        /// <param name="pagingParameters">Paging</param>
        /// <returns>list of NovJobs</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NovJobDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<NovJobDto>>> GetNovJobs([FromQuery] Paging pagingParameters)
        {
            GetPaginationNovJobsQuery getPaginationNovJobsQuery
                = new GetPaginationNovJobsQuery(HttpContext.Request.Query.Count == 0 ? null : pagingParameters);

            PagedResult<NovJobDto> result = await queryBus.Send<GetPaginationNovJobsQuery
                , PagedResult<NovJobDto>>(getPaginationNovJobsQuery);

            if (result == null || !result.Any())
            {
                List<NovJobDto> NovJobDtos = new List<NovJobDto>();
                result = new PagedResult<NovJobDto>(NovJobDtos, NovJobDtos.Count, null);
            }

            return Ok(result.Items);
        }

        /// <summary>
        ///  This method returns NovJob as per id.
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>NovJob as per Id</returns>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(NovJobDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<NovJobDto>> GetNovJobById([FromRoute] int id)
        {
            GetNovJobByIdQuery getNovJobByIdQuery = new GetNovJobByIdQuery(id);
            var result = await queryBus.Send<GetNovJobByIdQuery, NovJobDto>(getNovJobByIdQuery);

            if (result == null)
                return NotFound($"NovJob with id:{id} not found");

            return Ok(result);
        }

        /// <summary>
        /// This method creates NovJob based on requested data.
        /// </summary>
        /// <param name="CreateNovJobCommand">CreateNovJobCommand</param>
        /// <returns>OK Result</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateNovJob([FromBody] CreateNovJobCommand createNovJobCommand
            , [FromHeader(Name = "x-requestid")] string requestId)
        { //Todo
//            if (!int.TryParse(requestId, out int requestGuid) && requestGuid == default)
//#pragma warning disable S1854 // Unused assignments should be removed
//                requestGuid = 0;//;Guid.NewGuid();
//#pragma warning restore S1854 // Unused assignments should be removed

            ContentResult result = await commandBus.Send<CreateNovJobCommand, ContentResult>(createNovJobCommand);


            if (result.StatusCode == (int)HttpStatusCode.BadRequest)
                return BadRequest(JsonConvert.DeserializeObject(result.Content));

            return Ok();
        }


        /// <summary>
        ///  This method returns NovJob as per jobId.
        /// </summary>
        /// <param name="jobNumber">int</param>
        /// <returns>NovJob as per JobId</returns>
        [HttpGet]
        [Route("Job/{jobNumber:int}")]
        [ProducesResponseType(typeof(NovJobDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<NovJobDto>>> GetNovJobByJobNumber([FromRoute] int jobNumber)
        {
            GetNovJobByJobNumberQuery getNovJobByJobIdQuery = new GetNovJobByJobNumberQuery(jobNumber);
            var result = await queryBus.Send<GetNovJobByJobNumberQuery, IEnumerable<NovJobDto>>(getNovJobByJobIdQuery);

            if (result == null)
                return NotFound($"NovJob with jobNumber:{jobNumber} not found");

            return Ok(result);
        }

        [HttpPost]
        [Route("search")]
        [ProducesResponseType(typeof(IEnumerable<NovJobDetailsView>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<NovJobDetailsView>>> GetNovJobDetailsSearch([FromBody] JobSearchRequest searchRequest)
        {
            if (searchRequest.ExportToExcel)
                searchRequest.PagingParameters.PageSize = ConstantsProperty.PageSize;
            GetPaginationNovJobDetailsQuery getPaginationNovJobDetailsQuery = new GetPaginationNovJobDetailsQuery(searchRequest);
            var result = await queryBus.Send<GetPaginationNovJobDetailsQuery, PagedResult<NovJobDetailsView>>(getPaginationNovJobDetailsQuery);
            if (result == null)
                return NotFound($"NovJobDetails cannot found with given input.");

            return Ok(result);
        }

        [HttpPost]
        [Route("search/inline")]
        [ProducesResponseType(typeof(IEnumerable<InlineSearchResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<InlineSearchResult>>> GetNovJobDetailslineSearch([FromBody] JobSearchRequest searchRequest)
        {
            GetPaginationNovJobDetailsInlineSearchQuery getPaginationNovJobDetailsInlineSearchQuery
               = new GetPaginationNovJobDetailsInlineSearchQuery(searchRequest);
            var result = await queryBus.Send<GetPaginationNovJobDetailsInlineSearchQuery
                , InlineSearchResult>(getPaginationNovJobDetailsInlineSearchQuery);
            if (result == null)
                return NotFound($"NovJobDetails cannot found with given input.");

            return Ok(result);
        }
    }
}

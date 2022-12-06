using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOV.ES.Framework.Core.CQRS.Commands;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Common.Exception;
using NOV.ES.TAT.Job.API.Application.Queries;
using NOV.ES.TAT.Job.DTOs;
using System.Net;

namespace NOV.ES.TAT.Job.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobSnapShotsController : ControllerBase
    {
        private readonly IQueryBus queryBus;

        public JobSnapShotsController(IQueryBus queryBus)
        {
            this.queryBus = queryBus;
        }

        /// <summary>
        /// This method returns service name.
        /// </summary>
        /// <returns>ServiceName</returns>
        [HttpGet]
        [Route("ServiceName")]
        public IActionResult ServiceName()
        {
            return Ok("JobSnapShot API Service.");
        }

        /// <summary>
        /// This method returns all JobSnapShots 
        /// </summary>
        /// <param name="pagingParameters">Paging</param>
        /// <returns>list of JobSnapShots</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobSnapShotDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<JobSnapShotDto>>> GetJobSnapShots([FromQuery] Paging pagingParameters)
        {
            GetPaginationJobSnapShotsQuery getPaginationJobSnapShotsQuery
                = new GetPaginationJobSnapShotsQuery(HttpContext.Request.Query.Count == 0 ? null : pagingParameters);

            PagedResult<JobSnapShotDto> result = await queryBus.Send<GetPaginationJobSnapShotsQuery
                , PagedResult<JobSnapShotDto>>(getPaginationJobSnapShotsQuery);

            if (result == null || !result.Any())
            {
                List<JobSnapShotDto> JobSnapShotDtos = new List<JobSnapShotDto>();
                result = new PagedResult<JobSnapShotDto>(JobSnapShotDtos, JobSnapShotDtos.Count, null);
            }
           return Ok(result.Items);
        }

        /// <summary>
        ///  This method returns JobSnapShot as per id.
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>JobSnapShot as per Id</returns>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(JobSnapShotDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<JobSnapShotDto>> GetJobSnapShotById([FromRoute] int id)
        {
            GetJobSnapShotByIdQuery getJobSnapShotByIdQuery = new GetJobSnapShotByIdQuery(id);
            var result = await queryBus.Send<GetJobSnapShotByIdQuery, JobSnapShotDto>(getJobSnapShotByIdQuery);

            if (result == null)
                return NotFound($"JobSnapShot with id:{id} not found");

            return Ok(result);
        }

        
    }
}

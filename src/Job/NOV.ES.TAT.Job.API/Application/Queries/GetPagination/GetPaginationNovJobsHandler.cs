using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.API.Helper;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationNovJobsHandler : IQueryHandler<GetPaginationNovJobsQuery, PagedResult<NovJobDto>>
    {
        private readonly IMapper mapper;
        private readonly INovJobService jobService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetPaginationNovJobsHandler(IMapper mapper,
            INovJobService jobService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.jobService = jobService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<PagedResult<NovJobDto>> Handle(GetPaginationNovJobsQuery request,
            CancellationToken cancellationToken)
        {
            var novJobs = jobService.GetNovJobs(request.PagingParameters);
            var result = mapper.Map<PagedResult<NovJob>, PagedResult<NovJobDto>>(novJobs);
            PagingHelper.AddPagingMetadata<NovJobDto>(result, httpContextAccessor);
            return Task.FromResult(result);
        }
    }
}

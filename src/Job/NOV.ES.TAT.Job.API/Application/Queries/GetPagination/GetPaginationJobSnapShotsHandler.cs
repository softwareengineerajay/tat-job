using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.API.Helper;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationJobSnapShotsHandler : IQueryHandler<GetPaginationJobSnapShotsQuery, PagedResult<JobSnapShotDto>>
    {
        private readonly IMapper mapper;
        private readonly IJobSnapShotService jobService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetPaginationJobSnapShotsHandler(IMapper mapper,
            IJobSnapShotService jobService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.jobService = jobService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<PagedResult<JobSnapShotDto>> Handle(GetPaginationJobSnapShotsQuery request,
            CancellationToken cancellationToken)
        {
            var jobSnapShots = jobService.GetJobSnapShots(request.PagingParameters);
            var result = mapper.Map<PagedResult<JobSnapShot>, PagedResult<JobSnapShotDto>>(jobSnapShots);
            PagingHelper.AddPagingMetadata<JobSnapShotDto>(result, httpContextAccessor);
            return Task.FromResult(result);
        }
    }
}

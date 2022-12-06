using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.API.Helper;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationNovJobDetailsHandler : IQueryHandler<GetPaginationNovJobDetailsQuery, PagedResult<NovJobDetailsView>>
    {
        private readonly INovJobDetailsService jobDetailsService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetPaginationNovJobDetailsHandler(INovJobDetailsService jobDetailsService
            ,IHttpContextAccessor httpContextAccessor)
        {
            this.jobDetailsService = jobDetailsService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<PagedResult<NovJobDetailsView>> Handle(GetPaginationNovJobDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var novJobresult = jobDetailsService.GetNovJobDetailsSearch(request.SearchRequest.PagingParameters, request.SearchRequest);
            PagingHelper.AddPagingMetadata(novJobresult, httpContextAccessor);
            return Task.FromResult(novJobresult);
        }
    }
}

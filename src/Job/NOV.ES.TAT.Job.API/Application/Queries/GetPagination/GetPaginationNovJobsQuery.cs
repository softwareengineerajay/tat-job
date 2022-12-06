using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.DTOs;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationNovJobsQuery : IQuery<PagedResult<NovJobDto>>
    {
        public Paging PagingParameters { get; private set; }

        public GetPaginationNovJobsQuery(Paging pagingParameters)
        {
            this.PagingParameters = pagingParameters;
        }
    }
}

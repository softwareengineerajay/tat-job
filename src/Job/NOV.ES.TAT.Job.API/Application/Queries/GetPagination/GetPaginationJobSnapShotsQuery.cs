using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.DTOs;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationJobSnapShotsQuery : IQuery<PagedResult<JobSnapShotDto>>
    {
        public Paging PagingParameters { get; private set; }

        public GetPaginationJobSnapShotsQuery(Paging pagingParameters)
        {
            this.PagingParameters = pagingParameters;
        }
    }
}

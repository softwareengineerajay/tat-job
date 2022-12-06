using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure.Helper;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationNovJobDetailsQuery : IQuery<PagedResult<NovJobDetailsView>>
    {
        public JobSearchRequest SearchRequest { get; private set; }
        public GetPaginationNovJobDetailsQuery(JobSearchRequest searchRequest)
        {
            this.SearchRequest = searchRequest;
        }
    }
}

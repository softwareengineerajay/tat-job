using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Infrastructure.Helper;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetPaginationNovJobDetailsInlineSearchQuery : IQuery<InlineSearchResult>
    {
        public JobSearchRequest SearchRequest { get; private set; }

        public GetPaginationNovJobDetailsInlineSearchQuery(JobSearchRequest searchRequest)
        {
            this.SearchRequest = searchRequest;
        }
    }
}

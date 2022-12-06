using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure.Helper;

namespace NOV.ES.TAT.Job.Interfaces
{
    public interface INovJobDetailsService
    {
        PagedResult<NovJobDetailsView> GetNovJobDetailsSearch(Paging pagingParameters, JobSearchRequest searchRequest);
        InlineSearchResult GetNovJobDetailsInlineSearch(Paging pagingParameters, JobSearchRequest searchRequest);

    }
}

using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure.Helper;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public interface INovJobDetailsQueryRepository
        : IReadRepository<NovJobDetailsView>
    {
        IQueryable<NovJobDetailsView> GetNovJobDetails(JobSearchRequest searchRequest);
    }
}

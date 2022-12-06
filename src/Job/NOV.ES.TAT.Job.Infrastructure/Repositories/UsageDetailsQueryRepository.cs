using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class UsageDetailsQueryRepository :
        GenericReadRepository<UsageDetailsView>,
        IUsageDetailsQueryRepository
    {
        

        public UsageDetailsQueryRepository(JobDBContext context)
            : base(context)
        {           
        }

    }
}

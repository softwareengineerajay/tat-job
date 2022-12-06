using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class JobSnapShotQueryRepository :
        GenericReadRepository<JobSnapShot>,
        IJobSnapShotQueryRepository
    {
      
        public JobSnapShotQueryRepository(JobDBContext context)
            : base(context)
        {
            
        }

    }
}

using NOV.ES.Framework.Core.Data.Repositories;
using NOV.ES.TAT.Job.Domain;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class JobSnapShotCommandRepository
        : GenericWriteRepository<JobSnapShot>
        , IJobSnapShotCommandRepository
    {
        private readonly JobDBContext jobDBContext;

        public JobSnapShotCommandRepository(JobDBContext context)
            : base(context)
        {
            this.jobDBContext = context;
        }

        public int SaveChanges()
        {
            return jobDBContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return jobDBContext.SaveChangesAsync(cancellationToken);
        }
    }
}

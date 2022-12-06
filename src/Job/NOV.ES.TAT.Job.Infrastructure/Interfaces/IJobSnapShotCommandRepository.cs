using NOV.ES.Framework.Core.Data.Repositories;
using NOV.ES.TAT.Job.Domain;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public interface IJobSnapShotCommandRepository
        : IWriteRepository<JobSnapShot>
    {
        public int SaveChanges();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

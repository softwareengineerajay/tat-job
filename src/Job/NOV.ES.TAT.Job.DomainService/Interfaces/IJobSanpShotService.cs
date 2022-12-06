using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;

namespace NOV.ES.TAT.Job.Interfaces
{
    public interface IJobSnapShotService
    {
        PagedResult<JobSnapShot> GetJobSnapShots(Paging pagingParameters);
        JobSnapShot GetJobSnapShotById(int id);
        
    }
}

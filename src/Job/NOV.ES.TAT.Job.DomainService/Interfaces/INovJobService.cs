using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;

namespace NOV.ES.TAT.Job.Interfaces
{
    public interface INovJobService
    {
        PagedResult<NovJob> GetNovJobs(Paging pagingParameters);
        NovJob GetNovJobById(int id);
        List<NovJob> GetNovJobByJobNumber(int jobNumber);
        bool CreateNovJob(NovJob job);
    }
}

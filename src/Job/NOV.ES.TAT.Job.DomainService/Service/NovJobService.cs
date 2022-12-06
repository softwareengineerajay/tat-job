using NOV.ES.Framework.Core.Data;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.Service
{
    public class NovJobService : INovJobService
    {
        private readonly INovJobQueryRepository jobQueryRepository;
        private readonly INovJobCommandRepository jobCommandRepository;
        public NovJobService(INovJobQueryRepository jobQueryRepository
            , INovJobCommandRepository jobCommandRepository)
        {
            this.jobQueryRepository = jobQueryRepository;
            this.jobCommandRepository = jobCommandRepository;
        }
        public NovJob GetNovJobById(int id)
        {
            var filter = PredicateBuilder.Create<NovJob>(x => x.Id == id && x.IsActive);
            return jobQueryRepository.Get(filter, null, null).FirstOrDefault();
        }
        public PagedResult<NovJob> GetNovJobs(Paging pagingParameters)
        {
            var filter = PredicateBuilder.Create<NovJob>(x => x.Id != default && x.IsActive);
            var result = jobQueryRepository.Get(filter, null, null);
            return PagingExtensions.GetPagedResult(result, pagingParameters);
        }

        public bool CreateNovJob(NovJob job)
        {
            jobCommandRepository.Create(job);
            jobCommandRepository.SaveChanges();
            return true;
        }
        public List<NovJob> GetNovJobByJobNumber(int jobNumber)
        {
            var filter = PredicateBuilder.Create<NovJob>(x => x.JobNumber == jobNumber && x.IsActive);
            return jobQueryRepository.Get(filter, null, null).ToList();
        }
    }
}

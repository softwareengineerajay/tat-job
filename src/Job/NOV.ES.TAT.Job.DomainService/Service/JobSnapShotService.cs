using NOV.ES.Framework.Core.Data;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DomainService;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.Service
{
    public class JobSnapShotService : IJobSnapShotService
    {
        private readonly IJobSnapShotQueryRepository jobSnapShotQueryRepository;

        public JobSnapShotService(IJobSnapShotQueryRepository jobSnapShotQueryRepository)
        {
            this.jobSnapShotQueryRepository = jobSnapShotQueryRepository;
        }
        public JobSnapShot GetJobSnapShotById(int id)
        {
            var filter = PredicateBuilder.Create<JobSnapShot>(x => x.Id == id && x.IsActive);
            return jobSnapShotQueryRepository.Get(filter, null, null).FirstOrDefault();
        }
        public PagedResult<JobSnapShot> GetJobSnapShots(Paging pagingParameters)
        {
            var filter = PredicateBuilder.Create<JobSnapShot>(x => x.Id != default && x.IsActive);
            var result = jobSnapShotQueryRepository.Get(filter, null, null);
            return PagingExtensions.GetPagedResult(result, pagingParameters);
        }
       
    }
}

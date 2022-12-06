using NOV.ES.Framework.Core.Data;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.Service
{
    public class UsageDetailsService : IUsageDetailsService
    {
        private readonly IUsageDetailsQueryRepository UsageQueryRepository;
        public UsageDetailsService(IUsageDetailsQueryRepository UsageQueryRepository)
        {
            this.UsageQueryRepository = UsageQueryRepository;
        }

        public List<UsageDetailsView> GetUsageDetailsByJobNumber(int jobNumber)
        {
            var filter = PredicateBuilder.Create<UsageDetailsView>(x => x.JobNumber == jobNumber && x.IsActive);
            return UsageQueryRepository.Get(filter, null, null).ToList();
        }
    }
}

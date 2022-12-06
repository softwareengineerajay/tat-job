using NOV.ES.Framework.Core.Data;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.Service
{
    public class SalesOrderDetailsService : ISalesOrderDetailsService
    {
        private readonly ISalesOrderDetailsQueryRepository SalesQueryRepository;
        public SalesOrderDetailsService(ISalesOrderDetailsQueryRepository SalesQueryRepository)
        {
            this.SalesQueryRepository = SalesQueryRepository;
        }

        public List<SalesOrderDetailsView> GetSalesOrderDetailsByJobNumber(int jobNumber)
        {
            var filter = PredicateBuilder.Create<SalesOrderDetailsView>(x => x.JobNumber == jobNumber && x.IsActive);
            return SalesQueryRepository.Get(filter, null, null).ToList();
        }
    }
}

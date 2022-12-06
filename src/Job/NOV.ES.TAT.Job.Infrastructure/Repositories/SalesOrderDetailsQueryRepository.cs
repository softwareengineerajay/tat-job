using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class SalesOrderDetailsQueryRepository :
        GenericReadRepository<SalesOrderDetailsView>,
        ISalesOrderDetailsQueryRepository
    {
        

        public SalesOrderDetailsQueryRepository(JobDBContext context)
            : base(context)
        {           
        }

    }
}

using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;


namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetSalesOrderDetailsByJobNumberQuery : IQuery<IEnumerable<SalesOrderDetailsView>>
    {
        public int JobNumber { get; private set; }

        public GetSalesOrderDetailsByJobNumberQuery(int jobNumber)
        {
            this.JobNumber = jobNumber;
        }
    }
}

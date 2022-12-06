using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;


namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetUsageDetailsByJobNumberQuery : IQuery<IEnumerable<UsageDetailsView>>
    {
        public int JobNumber { get; private set; }

        public GetUsageDetailsByJobNumberQuery(int jobNumber)
        {
            this.JobNumber = jobNumber;
        }
    }
}

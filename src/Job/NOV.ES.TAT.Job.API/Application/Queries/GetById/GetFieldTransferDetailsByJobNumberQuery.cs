using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;


namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetFieldTransferDetailsByJobNumberQuery : IQuery<IEnumerable<FieldTransferSlipDetailsView>>
    {
        public int JobNumber { get; private set; }

        public GetFieldTransferDetailsByJobNumberQuery(int jobNumber)
        {
            this.JobNumber = jobNumber;
        }
    }
}

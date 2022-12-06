using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.DTOs;


namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetNovJobByJobNumberQuery : IQuery<IEnumerable<NovJobDto>>
    {
        public int JobNumber { get; private set; }

        public GetNovJobByJobNumberQuery(int jobNumber)
        {
            this.JobNumber = jobNumber;
        }
    }
}

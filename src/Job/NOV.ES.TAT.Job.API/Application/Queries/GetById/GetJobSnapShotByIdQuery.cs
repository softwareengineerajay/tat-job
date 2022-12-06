using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.DTOs;


namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetJobSnapShotByIdQuery : IQuery<JobSnapShotDto>
    {
        public int Id { get; private set; }

        public GetJobSnapShotByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}

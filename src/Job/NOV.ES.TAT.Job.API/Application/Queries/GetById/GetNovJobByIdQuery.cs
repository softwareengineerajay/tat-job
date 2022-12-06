using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.DTOs;


namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetNovJobByIdQuery : IQuery<NovJobDto>
    {
        public int Id { get; private set; }

        public GetNovJobByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}

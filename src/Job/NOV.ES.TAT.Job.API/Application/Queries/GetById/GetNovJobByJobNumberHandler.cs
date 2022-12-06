using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetNovJobByJobNumberHandler : IQueryHandler<GetNovJobByJobNumberQuery, IEnumerable<NovJobDto>>
    {
        private readonly IMapper mapper;
        private readonly INovJobService jobService;

        public GetNovJobByJobNumberHandler(IMapper mapper,
            INovJobService jobService)
        {
            this.mapper = mapper;
            this.jobService = jobService;
        }

        public Task<IEnumerable<NovJobDto>> Handle(GetNovJobByJobNumberQuery request, CancellationToken cancellationToken)
        {
            if (!IsValidRequest(request))
                throw new ArgumentException("Value can not be null or Empty");

            var docStatus = jobService.GetNovJobByJobNumber(request.JobNumber);
            var result = mapper.Map<IEnumerable<NovJob>, IEnumerable<NovJobDto>>(docStatus);
            return Task.FromResult(result);
        }
        private static bool IsValidRequest(GetNovJobByJobNumberQuery request)
        {
            if (request != null && request.JobNumber > 0)
                return true;            
            return false;
        }
    }
}

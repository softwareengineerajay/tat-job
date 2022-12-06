using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetNovJobByIdHandler : IQueryHandler<GetNovJobByIdQuery, NovJobDto>
    {
        private readonly IMapper mapper;
        private readonly INovJobService jobService;

        public GetNovJobByIdHandler(IMapper mapper,
            INovJobService jobService)
        {
            this.mapper = mapper;
            this.jobService = jobService;
        }

        public Task<NovJobDto> Handle(GetNovJobByIdQuery request, CancellationToken cancellationToken)
        {
            if (!IsValidRequest(request))
                throw new ArgumentException("Value can not be null or Empty");

            var docStatus = jobService.GetNovJobById(request.Id);
            var result = mapper.Map<NovJob, NovJobDto>(docStatus);
            return Task.FromResult(result);
        }
        private static bool IsValidRequest(GetNovJobByIdQuery request)
        {
            if (request != null && request.Id != default)
                return true;
            return false;
        }
    }
}

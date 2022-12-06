using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetJobSnapShotByIdHandler : IQueryHandler<GetJobSnapShotByIdQuery, JobSnapShotDto>
    {
        private readonly IMapper mapper;
        private readonly IJobSnapShotService jobService;

        public GetJobSnapShotByIdHandler(IMapper mapper,
            IJobSnapShotService jobService)
        {
            this.mapper = mapper;
            this.jobService = jobService;
        }

        public Task<JobSnapShotDto> Handle(GetJobSnapShotByIdQuery request, CancellationToken cancellationToken)
        {
            if (!IsValidRequest(request))
                throw new ArgumentException("Value can not be null or Empty");

            var jobSnapShot = jobService.GetJobSnapShotById(request.Id);
            var result = mapper.Map<JobSnapShot, JobSnapShotDto>(jobSnapShot);
            return Task.FromResult(result);
        }
        private static bool IsValidRequest(GetJobSnapShotByIdQuery request)
        {
            if (request != null && request.Id != default)
                return true;
            return false;
        }
    }
}

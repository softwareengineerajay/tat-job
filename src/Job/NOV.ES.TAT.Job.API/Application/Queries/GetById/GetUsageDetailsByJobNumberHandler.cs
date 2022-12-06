using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetUsageDetailsByJobNumberHandler : IQueryHandler<GetUsageDetailsByJobNumberQuery, IEnumerable<UsageDetailsView>>
    {
        private readonly IMapper mapper;
        private readonly IUsageDetailsService usageService;

        public GetUsageDetailsByJobNumberHandler(IMapper mapper,
            IUsageDetailsService usageService)
        {
            this.mapper = mapper;
            this.usageService = usageService;
        }

        public Task<IEnumerable<UsageDetailsView>> Handle(GetUsageDetailsByJobNumberQuery request, CancellationToken cancellationToken)
        {
            if (!IsValidRequest(request))
                throw new ArgumentException("Value can not be null or Empty");

            var usages = usageService.GetUsageDetailsByJobNumber(request.JobNumber);
            var result = mapper.Map<IEnumerable<UsageDetailsView>, IEnumerable<UsageDetailsView>>(usages);

            return Task.FromResult(result);
        }
        private static bool IsValidRequest(GetUsageDetailsByJobNumberQuery request)
        {
            if (request != null && request.JobNumber > 0)
                return true;            
            return false;
        }
    }
}

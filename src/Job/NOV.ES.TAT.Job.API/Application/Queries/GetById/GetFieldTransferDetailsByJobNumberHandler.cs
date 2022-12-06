using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetFieldTransferDetailsByJobNumberHandler : IQueryHandler<GetFieldTransferDetailsByJobNumberQuery, IEnumerable<FieldTransferSlipDetailsView>>
    {
        private readonly IMapper mapper;
        private readonly IFieldTransferDetailsService fieldTransferService;

        public GetFieldTransferDetailsByJobNumberHandler(IMapper mapper,
            IFieldTransferDetailsService fieldTransferService)
        {
            this.mapper = mapper;
            this.fieldTransferService = fieldTransferService;
        }

        public Task<IEnumerable<FieldTransferSlipDetailsView>> Handle(GetFieldTransferDetailsByJobNumberQuery request, CancellationToken cancellationToken)
        {
            if (!IsValidRequest(request))
                throw new ArgumentException("Value can not be null or Empty");

            var fieldslip = fieldTransferService.GetFieldTransferDetailsByJobNumber(request.JobNumber);
            var result = mapper.Map<IEnumerable<FieldTransferSlipDetailsView>, IEnumerable<FieldTransferSlipDetailsView>>(fieldslip);

            return Task.FromResult(result);
        }
        private static bool IsValidRequest(GetFieldTransferDetailsByJobNumberQuery request)
        {
            if (request != null && request.JobNumber > 0)
                return true;            
            return false;
        }
    }
}

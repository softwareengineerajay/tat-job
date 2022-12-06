using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
    public class GetSalesOrderDetailsByJobNumberHandler : IQueryHandler<GetSalesOrderDetailsByJobNumberQuery, IEnumerable<SalesOrderDetailsView>>
    {
        private readonly IMapper mapper;
        private readonly ISalesOrderDetailsService salesOrderService;

        public GetSalesOrderDetailsByJobNumberHandler(IMapper mapper,
            ISalesOrderDetailsService salesOrderService)
        {
            this.mapper = mapper;
            this.salesOrderService = salesOrderService;
        }

        public Task<IEnumerable<SalesOrderDetailsView>> Handle(GetSalesOrderDetailsByJobNumberQuery request, CancellationToken cancellationToken)
        {
            if (!IsValidRequest(request))
                throw new ArgumentException("Value can not be null or Empty");

            var saless = salesOrderService.GetSalesOrderDetailsByJobNumber(request.JobNumber);
            var result = mapper.Map<IEnumerable<SalesOrderDetailsView>, IEnumerable<SalesOrderDetailsView>>(saless);

            return Task.FromResult(result);
        }
        private static bool IsValidRequest(GetSalesOrderDetailsByJobNumberQuery request)
        {
            if (request != null && request.JobNumber > 0)
                return true;            
            return false;
        }
    }
}

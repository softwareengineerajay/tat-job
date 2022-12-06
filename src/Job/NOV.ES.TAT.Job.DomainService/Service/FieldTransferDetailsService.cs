using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.Service
{
    public class FieldTransferDetailsService : IFieldTransferDetailsService
    {
        private readonly IFieldTransferDetailsQueryRepository FieldTransferQueryRepository;
        public FieldTransferDetailsService(IFieldTransferDetailsQueryRepository FieldTransferQueryRepository)
        {
            this.FieldTransferQueryRepository = FieldTransferQueryRepository;
        }
         public List<FieldTransferSlipDetailsView> GetFieldTransferDetailsByJobNumber(int jobNumber)
        {
            var result = FieldTransferQueryRepository.GetFieldTransferDetailsByJobNumber(jobNumber);
            return result.ToList();
        }
    }
}

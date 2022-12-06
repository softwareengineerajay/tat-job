using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class FieldTransferDetailsQueryRepository :
        GenericReadRepository<FieldTransferSlipDetailsView>,
        IFieldTransferDetailsQueryRepository
    {

        public readonly JobDBContext dbContext;
        public FieldTransferDetailsQueryRepository(JobDBContext context)
            : base(context)
        {
            dbContext = context;
        }
        public IQueryable<FieldTransferSlipDetailsView> GetFieldTransferDetailsByJobNumber(int jobNumber)
        {
            var result = dbContext.FieldTransferSlipDetailsView.Where(s => s.IsActive && (s.FromErpJobNumber == jobNumber || s.ToErpJobNumber == jobNumber)).AsQueryable()
                        .Select(s => new FieldTransferSlipDetailsView
                        {
                            FieldTransferNumber = s.FieldTransferNumber,
                            TransferDate = s.TransferDate,
                            SourceType = (s.FromErpJobNumber == jobNumber) ? "From" : (s.ToErpJobNumber == jobNumber) ? "To" : null,
                            FromErpJobNumber = s.FromErpJobNumber,
                            ToErpJobNumber = s.ToErpJobNumber,
                            FromCompanyCode = s.ToCompanyCode,
                            FromCompanyName = s.FromCompanyName,
                            ToCompanyCode = s.ToCompanyCode,
                            ToCompanyName = s.ToCompanyName,
                            FromCcCode = s.FromCcCode,
                            FromCcName = s.FromCcName,
                            ToCcCode = s.ToCcCode,
                            ToCcName = s.ToCcName,
                            FromCustomerCode = s.FromCustomerCode,
                            FromCustomerName = s.FromCustomerName,
                            ToCustomerCode = s.ToCustomerCode,
                            ToCustomerName = s.ToCustomerName,
                            FromRigName = s.FromRigName,
                            ToRigName = s.ToRigName,
                            WellName = s.WellName,
                            ContractorName = s.ContractorName,
                            IsActive = s.IsActive
                        });


            return result;
        }
    }
}

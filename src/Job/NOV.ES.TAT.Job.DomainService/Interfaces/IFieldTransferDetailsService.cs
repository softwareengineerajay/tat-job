using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Interfaces
{
    public interface IFieldTransferDetailsService
    {
        List<FieldTransferSlipDetailsView> GetFieldTransferDetailsByJobNumber(int jobNumber);
      
    }
}

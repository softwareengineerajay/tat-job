using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Interfaces
{
    public interface ISalesOrderDetailsService
    {
        List<SalesOrderDetailsView> GetSalesOrderDetailsByJobNumber(int jobNumber);
      
    }
}

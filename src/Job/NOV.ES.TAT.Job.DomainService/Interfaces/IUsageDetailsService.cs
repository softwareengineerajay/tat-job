using NOV.ES.TAT.Job.Domain.ReadModel;

namespace NOV.ES.TAT.Job.Interfaces
{
    public interface IUsageDetailsService
    {
        List<UsageDetailsView> GetUsageDetailsByJobNumber(int jobNumber);
      
    }
}

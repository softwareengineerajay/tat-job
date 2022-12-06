using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class NovJobQueryRepository :
        GenericReadRepository<NovJob>,
        INovJobQueryRepository
    {
        

        public NovJobQueryRepository(JobDBContext context)
            : base(context)
        {           
        }

    }
}

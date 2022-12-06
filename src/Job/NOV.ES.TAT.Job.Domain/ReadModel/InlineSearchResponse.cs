using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.ES.TAT.Job.Domain.ReadModel
{
    public class InlineSearchResponse
    {
        public List<LookupData> Companies { get; set; }
        public List<LookupData> BusinessUnits { get; set; }
        public List<LookupData> Customers { get; set; }
        public List<LookupDataRigWell> Rigs { get; set; }
        public List<LookupData> NovJobs { get; set; }
    }
}

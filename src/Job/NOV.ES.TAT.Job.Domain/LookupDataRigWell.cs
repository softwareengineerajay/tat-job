using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.ES.TAT.Job.Domain
{
    public class LookupDataRigWell
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string DependentKey { get; set; }
    }
}

using NOV.ES.Framework.Core.DTOs;

namespace NOV.ES.TAT.Job.DTOs
{
    public class NovJobDto : BaseModel<int>
    {
        public int JobNumber { get; set; } // Required MDM.Job
        public int ModuleId { get; set; } // Required CT.Id, Usage.ID, SalesOrderId
        public string ModuleKey { get; set; } // Required CT/Usage/Billing
        public int BusinessUnit { get; set; }
        public Guid CorpRigId { get; set; }
        public int Customer { get; set; }
        public long? CorpWellSiteId { get; set; }

    }
}

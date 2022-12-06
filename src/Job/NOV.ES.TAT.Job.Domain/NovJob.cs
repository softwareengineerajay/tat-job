using NOV.ES.Framework.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOV.ES.TAT.Job.Domain
{
    [Table("NovJob", Schema = "job")]

    public class NovJob : BaseEntity<int>
    {
        [Required]
        public int JobNumber { get; set; } // Required MDM.Job
        [Required]
        public int ModuleId { get; set; } // Required CT.Id, Usage.ID, SalesOrderId
        [MaxLength(100)]
        [Required]
        public string ModuleKey { get; set; } // Required CT/Usage/Billing
        public int? BusinessUnit { get; set; }
        public Guid? CorpRigId { get; set; }
        public int? Customer { get; set; }
        public long? CorpWellSiteId { get; set; }

    }
}

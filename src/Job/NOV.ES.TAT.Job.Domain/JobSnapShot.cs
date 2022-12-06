using NOV.ES.Framework.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOV.ES.TAT.Job.Domain
{
    [Table("JobSnapShot", Schema = "job")]
    public class JobSnapShot : BaseEntity<int>
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int JobId { get; set; }        
        public string PreviousData { get; set; }
        public string CurrentData { get; set; }
        public string ChangedData { get; set; }
        [ForeignKey("JobId")]
        public virtual NovJob Job { get; set; }    

    }
}

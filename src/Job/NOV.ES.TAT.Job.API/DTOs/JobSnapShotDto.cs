using NOV.ES.Framework.Core.DTOs;

namespace NOV.ES.TAT.Job.DTOs
{
    public class JobSnapShotDto : BaseModel<int>
    {
        public int EventId { get; set; } 
        public int JobId { get; set; } 
        public string PreviousData { get; set; }
        public string CurrentData { get; set; }
        public string ChangedData { get; set; }
        
    }
}

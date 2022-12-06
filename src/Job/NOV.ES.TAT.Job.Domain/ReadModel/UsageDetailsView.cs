using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOV.ES.TAT.Job.Domain.ReadModel
{
    [Keyless]    
    [NotMapped]
    public class UsageDetailsView
    {
        public int Id { get; set; }
        public int UsageNumber { get; set; }
        public string ItemName { get; set; }
        public string ItemClassName { get; set; }
        public string SerialNumber { get; set; }
        public string RevenueBuCode { get; set; }
        public string RevenueBuName { get; set; }
        public string SendingBuCode { get; set; }
        public string SendingBuName { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public int JobNumber { get; set; }
        public bool IsActive { get; set; }
        public string LastBillingInfo { get; set; }
    }
}

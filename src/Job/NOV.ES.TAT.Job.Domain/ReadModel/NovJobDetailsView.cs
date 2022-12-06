using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.ES.TAT.Job.Domain.ReadModel
{
    [Keyless]
    [NotMapped]
    public class NovJobDetailsView
    {
        public int Id { get; set; }
        public int NovJobNumber  { get; set; }
        public string JobDescription { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int? BusinessUnitId { get; set; }  
        public string RevenueBuCode { get; set; }
        public string RevenueBuName { get; set; }
        public int? CustomerId { get; set; }
        public Int64? CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Guid? CorpRigId { get; set; }
        public string ContractorName { get; set; }
        public string RigName { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string JobStatus { get; set; }
        public bool IsActive { get; set; }
        
    }
}

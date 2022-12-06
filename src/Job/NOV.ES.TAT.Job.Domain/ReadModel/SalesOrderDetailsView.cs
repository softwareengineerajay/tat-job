using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOV.ES.TAT.Job.Domain.ReadModel
{
    [Keyless]
    [NotMapped]
    public class SalesOrderDetailsView
    {
        public int Id { get; set; }
        public int SalesOrderNumber { get; set; }
        public DateTime? SalesOrderDate { get; set; }
        public string FromRevenueCC { get; set; }
        public string RigName { get; set; }
        public string SalesZone { get; set; }
        public string SalesPerson { get; set; }
        public string Currency { get; set; }
        public int ItemNumber { get; set; }
        public int CreditItems { get; set; }
        public int SalesOrderCount { get; set; }
        public string SalesOrderType { get; set; }
        public string InvoiceNumber { get; set; }
        public int JobNumber { get; set; }
        public bool IsActive { get; set; }
    }
}

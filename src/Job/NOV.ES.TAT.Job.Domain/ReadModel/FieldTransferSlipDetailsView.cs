using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOV.ES.TAT.Job.Domain.ReadModel
{
    [Keyless]
    public class FieldTransferSlipDetailsView
    {
        public Int64 Id { get; set; }
        public int FieldTransferNumber { get; set; }
        public DateTime? TransferDate { get; set; }
        public string SourceType { get; set; }
        public int? FromErpJobNumber { get; set; }
        public int? ToErpJobNumber { get; set; }
        public string FromCompanyCode { get; set; }
        public string FromCompanyName { get; set; }
        public string ToCompanyCode { get; set; }
        public string ToCompanyName { get; set; }
        public string FromCcCode { get; set; }
        public string FromCcName { get; set; }
        public string ToCcCode { get; set; }
        public string ToCcName { get; set; }
        public Int64? FromCustomerCode { get; set; }
        public string FromCustomerName { get; set; }
        public Int64? ToCustomerCode { get; set; }
        public string ToCustomerName { get; set; }
        public string FromRigName { get; set; }
        public string ToRigName { get; set; }
        public string WellName { get; set; }
        public string ContractorName { get; set; }
        public bool IsActive { get; set; }
    }
}

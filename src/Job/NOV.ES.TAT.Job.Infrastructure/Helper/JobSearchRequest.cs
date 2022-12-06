using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.ES.TAT.Job.Infrastructure.Helper
{
    public class JobSearchRequest
    {
        public enum DateInputType { PlannedStartDate, ActualStartDate, PlannedEndDate, ActualEndDate }
        public enum JobStatusInputType { Open, Closed, Both }

        public Paging PagingParameters { get; set; }
        public DateInputType SelectedDateInputType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public JobStatusInputType selectedJobStatusInputType { get; set; }
        public List<string> RevenueBuList { get; set; }
        public List<string> CustomerList { get; set; }
        public List<string> CompanyList { get; set; }
        public List<string> RigList { get; set; }
        public string JobNo { get; set; }
        public List<string> JobList { get; set; }
        public string JobDescription { get; set; }
        public bool ExportToExcel { get; set; }

    }
}

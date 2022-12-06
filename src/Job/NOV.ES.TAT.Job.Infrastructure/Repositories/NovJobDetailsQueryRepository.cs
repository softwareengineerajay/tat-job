using Microsoft.EntityFrameworkCore;
using NOV.ES.Framework.Core.Data;
using NOV.ES.Framework.Core.Data.Repository;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure.Helper;
using System.Linq.Expressions;

namespace NOV.ES.TAT.Job.Infrastructure
{
    public class NovJobDetailsQueryRepository :
        GenericReadRepository<NovJobDetailsView>,
        INovJobDetailsQueryRepository
    {

        public readonly JobDBContext dbContext;
        public NovJobDetailsQueryRepository(JobDBContext context)
            : base(context)
        {
            dbContext = context;
        }

        public IQueryable<NovJobDetailsView> GetNovJobDetails(JobSearchRequest searchRequest)
        {
            return dbContext.NovJobDetailsView.Where(CreateJobSearchPredicate(searchRequest));
        }

        private static Expression<Func<NovJobDetailsView, bool>> CreateJobSearchPredicate(JobSearchRequest searchRequest)
        {
            var filter = PredicateBuilder.Create<NovJobDetailsView>(x => x.IsActive);

            if (searchRequest != null)
            {
                filter = AddFilterCriteriaByDateSelection(searchRequest, filter);
                filter = AddFilterCriteriaByJobStatus(searchRequest, filter);
                filter = AddFilterCriteriaByJobDesc(searchRequest, filter);
                filter = AddFilterCriteriaByRevenueBU(searchRequest, filter);
                filter = AddFilterCriteriaByCustomer(searchRequest, filter);
                filter = AddFilterCriteriaByRig(searchRequest, filter);
                filter = AddFilterCriteriaByJob(searchRequest, filter);
                filter = AddFilterCriteriaByJobList(searchRequest, filter);
                filter = AddFilterByCompany(searchRequest, filter);

            }
            return filter;
        }
        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByJob(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.JobNo != null && searchRequest.JobNo != "")
            {
                filter = filter.And(x => EF.Functions.Like(x.NovJobNumber.ToString(), searchRequest.JobNo.LikeHelper()));
            }
            return filter;
        }


        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByJobList(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.JobList != null && searchRequest.JobList.Count > 0)
            {
                filter = filter.And(x => searchRequest.JobList.Contains(x.NovJobNumber.ToString()));
            }
            return filter;
        }

        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByCustomer(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.CustomerList != null  && searchRequest.CustomerList.Count > 0)
            {
                filter = filter.And(x => searchRequest.CustomerList.Contains(x.CustomerId.ToString()));
            }

            return filter;
        }

        private static Expression<Func<NovJobDetailsView, bool>> AddFilterByCompany(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.CompanyList != null && searchRequest.CompanyList.Count > 0)
            {
                filter = filter.And(x => searchRequest.CompanyList.Contains(x.CompanyId.ToString()));
            }

            return filter;
        }
        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByRig(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.RigList != null && searchRequest.RigList.Count > 0)
            {
                filter = filter.And(x => searchRequest.RigList.Contains(x.CorpRigId.ToString()));
            }

            return filter;
        }
        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByRevenueBU(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.RevenueBuList != null)
            {
                filter = filter.And(x => searchRequest.RevenueBuList.Contains(x.BusinessUnitId.ToString()));
            }

            return filter;
        }
        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByJobStatus(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.selectedJobStatusInputType == JobSearchRequest.JobStatusInputType.Open)
            {
                filter = filter.And(x => (x.JobStatus == "Open"));
            }
            else if (searchRequest.selectedJobStatusInputType == JobSearchRequest.JobStatusInputType.Closed)
            {
                filter = filter.And(x => x.JobStatus == "Closed");
            }
               else if (searchRequest.selectedJobStatusInputType == JobSearchRequest.JobStatusInputType.Both)
            {
                filter = filter.And(x => x.JobStatus == "Closed" || x.JobStatus == "Open");
            }

            return filter;
        }

        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByJobDesc(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (!string.IsNullOrEmpty(searchRequest.JobDescription))
            {
                filter = filter.And(x => EF.Functions.Like(x.JobDescription, searchRequest.JobDescription.LikeHelper()));
            }
            return filter;
        }

        private static Expression<Func<NovJobDetailsView, bool>> AddFilterCriteriaByDateSelection(JobSearchRequest searchRequest, Expression<Func<NovJobDetailsView, bool>> filter)
        {
            if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.PlannedStartDate)
            {
                filter = filter.And(x => ((DateTime)x.PlannedStartDate).Date >= searchRequest.DateFrom.Date && ((DateTime)x.PlannedStartDate).Date<= searchRequest.DateTo.Date);
            }
            else if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo == DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.PlannedStartDate)
            {
                filter = filter.And(x => ((DateTime)x.PlannedStartDate).Date >= searchRequest.DateFrom.Date);
            }
            else if (searchRequest.DateFrom == DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.PlannedStartDate)
            {
                filter = filter.And(x => ((DateTime)x.PlannedStartDate).Date <= searchRequest.DateTo.Date);
            }

            if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.PlannedEndDate)
            {
                filter = filter.And(x => x.PlannedEndDate.Value.Date >= searchRequest.DateFrom.Date && x.PlannedEndDate.Value.Date <= searchRequest.DateTo.Date);
            }
            else if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo == DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.PlannedEndDate)
            {
                filter = filter.And(x => x.PlannedEndDate.Value.Date >= searchRequest.DateFrom.Date);
            }
            else if (searchRequest.DateFrom == DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.PlannedEndDate)
            {
                filter = filter.And(x => x.PlannedEndDate.Value.Date <= searchRequest.DateTo.Date);
            }

            if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.ActualStartDate)
            {
                filter = filter.And(x => x.ActualStartDate.Value.Date >= searchRequest.DateFrom.Date && x.ActualStartDate.Value.Date <= searchRequest.DateTo.Date);
            }
            else if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo == DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.ActualStartDate)
            {
                filter = filter.And(x => x.ActualStartDate.Value.Date >= searchRequest.DateFrom.Date);
            }
            else if (searchRequest.DateFrom == DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.ActualStartDate)
            {
                filter = filter.And(x => x.ActualStartDate.Value.Date <= searchRequest.DateTo.Date);
            }

            if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.ActualEndDate)
            {
                filter = filter.And(x => x.ActualEndDate.Value.Date >= searchRequest.DateFrom.Date && x.ActualEndDate.Value.Date <= searchRequest.DateTo.Date);
            }
            else if (searchRequest.DateFrom != DateTime.MinValue && searchRequest.DateTo == DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.ActualEndDate)
            {
                filter = filter.And(x => x.ActualEndDate.Value.Date >= searchRequest.DateFrom.Date);
            }
            else if (searchRequest.DateFrom == DateTime.MinValue && searchRequest.DateTo != DateTime.MinValue
                        && searchRequest.SelectedDateInputType == JobSearchRequest.DateInputType.ActualEndDate)
            {
                filter = filter.And(x => x.ActualEndDate.Value.Date <= searchRequest.DateTo.Date);
            }

            return filter;
        }
    }
}

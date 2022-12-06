using Microsoft.EntityFrameworkCore;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.Infrastructure;
using NOV.ES.TAT.Job.Infrastructure.Helper;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.Service
{
    public class NovJobDetailsService : INovJobDetailsService
    {
        private readonly INovJobDetailsQueryRepository jobDetailsQueryRepository;
        public NovJobDetailsService(INovJobDetailsQueryRepository jobDetailsQueryRepository)
        {
            this.jobDetailsQueryRepository = jobDetailsQueryRepository;
        }

        public PagedResult<NovJobDetailsView> GetNovJobDetailsSearch(Paging pagingParameters, JobSearchRequest searchRequest)
        {
            var result = jobDetailsQueryRepository.GetNovJobDetails(searchRequest);
            return PagingExtensions.GetPagedResult(result, pagingParameters);
        }

        public InlineSearchResult GetNovJobDetailsInlineSearch(Paging pagingParameters, JobSearchRequest searchRequest)
        {
            InlineSearchResult result = new InlineSearchResult();
            result.InlineSearchResponse = new InlineSearchResponse();

            IQueryable<NovJobDetailsView> searchQuery = jobDetailsQueryRepository.GetNovJobDetails(searchRequest);

            result.InlineSearchResponse.Companies = searchQuery.GroupBy(x => new { x.CompanyId, x.CompanyCode, x.CompanyName })
                .Select(group => new LookupData()
                {
                    Key = (int)(group.Key.CompanyId == null ? default : group.Key.CompanyId),
                    Code = group.Key.CompanyCode,
                    Value = group.Key.CompanyName
                }).ToListAsync().Result;

            result.InlineSearchResponse.BusinessUnits = searchQuery.GroupBy(x => new { x.BusinessUnitId, x.RevenueBuCode, x.RevenueBuName })
                .Select(group => new LookupData()
                {
                    Key = (int)(group.Key.BusinessUnitId == null ? default : group.Key.BusinessUnitId),
                    Code = group.Key.RevenueBuCode,
                    Value = group.Key.RevenueBuName
                }).ToListAsync().Result;

            result.InlineSearchResponse.Customers = searchQuery.GroupBy(x => new { x.CustomerId, x.CustomerCode, x.CustomerName })
                .Select(group => new LookupData()
                {
                    Key = (int)(group.Key.CustomerId == null ? default : group.Key.CustomerId),
                    Code = group.Key.CustomerCode.ToString(),
                    Value = group.Key.CustomerName
                }).ToListAsync().Result;

            result.InlineSearchResponse.Rigs = searchQuery.GroupBy(x => new { x.CorpRigId, x.RigName, x.ContractorName })
                .Select(group => new LookupDataRigWell()
                {
                    Key = (group.Key.CorpRigId == null ?default : group.Key.CorpRigId).ToString(),
                    Value = group.Key.ContractorName,
                    Code = group.Key.RigName

                }).ToListAsync().Result;

            result.InlineSearchResponse.NovJobs = searchQuery.GroupBy(x => new { x.NovJobNumber }).Select(group => new LookupData()
            {
                Id = group.Key.NovJobNumber,
            }).ToListAsync().Result;

            return result;
        }

    }
}

using AutoMapper;
using NOV.ES.Framework.Core.CQRS.Queries;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.API.Helper;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.Domain.ReadModel;
using NOV.ES.TAT.Job.DTOs;
using NOV.ES.TAT.Job.Infrastructure.Helper;
using NOV.ES.TAT.Job.Interfaces;

namespace NOV.ES.TAT.Job.API.Application.Queries
{
        public class GetPaginationNovJobDetailsInlineSearchHandler
        : IQueryHandler<GetPaginationNovJobDetailsInlineSearchQuery, InlineSearchResult>
        {
            private readonly INovJobDetailsService jobDetailsService;
            private readonly IHttpContextAccessor httpContextAccessor;

            public GetPaginationNovJobDetailsInlineSearchHandler(INovJobDetailsService jobDetailsService
                , IHttpContextAccessor httpContextAccessor)
            {
                this.jobDetailsService = jobDetailsService;
                this.httpContextAccessor = httpContextAccessor;
            }
            public Task<InlineSearchResult> Handle(GetPaginationNovJobDetailsInlineSearchQuery request,
              CancellationToken cancellationToken)
            {
                var result = jobDetailsService.GetNovJobDetailsInlineSearch(request.SearchRequest.PagingParameters, request.SearchRequest);
                PagingHelper.AddPagingMetadata(result.NovJobDetails, httpContextAccessor);
                return Task.FromResult(result);
            }
        }

    
}

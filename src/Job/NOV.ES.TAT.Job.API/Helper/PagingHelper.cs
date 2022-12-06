using NOV.ES.Framework.Core.Pagination;
namespace NOV.ES.TAT.Job.API.Helper
{
    public static class PagingHelper
    {
        public static void AddPagingMetadata<TDto>(PagedResult<TDto> result, IHttpContextAccessor httpContextAccessor)
        {
            if (result != null && result.Paging != null)
            {
                httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-PageCount", result.TotalNumberOfPages.ToString());
                httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-TotalRecordCount", result.TotalNumberOfItems.ToString());
                httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-PageIndex", result.Paging.PageIndex.ToString());
                httpContextAccessor.HttpContext.Response.Headers.Add("X-Paging-PageSize", result.Paging.PageSize.ToString());
            }
        }
    }
}



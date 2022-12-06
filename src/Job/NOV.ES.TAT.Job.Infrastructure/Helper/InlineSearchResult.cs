using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOV.ES.TAT.Job.Infrastructure.Helper
{
    public class InlineSearchResult
    {
        public PagedResult<NovJobDetailsView> NovJobDetails { get; set; }
        public InlineSearchResponse InlineSearchResponse { get; set; }
    }
}

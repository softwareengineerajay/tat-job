using AutoMapper;
using NOV.ES.Framework.Core.Pagination;
using NOV.ES.TAT.Job.Domain;
using NOV.ES.TAT.Job.DTOs;

namespace NOV.ES.TAT.Job.API.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {            
            CreateMap<PagedResult<NovJob>, PagedResult<NovJobDto>>();
            CreateMap<NovJob, NovJobDto>();
            CreateMap<NovJobDto, NovJob>();
            CreateMap<PagedResult<JobSnapShot>, PagedResult<JobSnapShotDto>>();
            CreateMap<JobSnapShot, JobSnapShotDto>();
            CreateMap<JobSnapShotDto, JobSnapShot>();

        }
    }
}

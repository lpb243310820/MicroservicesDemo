using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Lpb.WebPortal.MultiTenancy;

namespace Lpb.WebPortal.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}

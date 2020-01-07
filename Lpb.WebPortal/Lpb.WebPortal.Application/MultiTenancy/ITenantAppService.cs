using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Lpb.WebPortal.MultiTenancy.Dto;

namespace Lpb.WebPortal.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}


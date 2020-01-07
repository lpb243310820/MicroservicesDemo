using System.Threading.Tasks;
using Abp.Application.Services;
using Lpb.WebPortal.Authorization.Accounts.Dto;

namespace Lpb.WebPortal.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}

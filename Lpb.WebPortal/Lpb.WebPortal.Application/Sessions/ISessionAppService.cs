using System.Threading.Tasks;
using Abp.Application.Services;
using Lpb.WebPortal.Sessions.Dto;

namespace Lpb.WebPortal.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}

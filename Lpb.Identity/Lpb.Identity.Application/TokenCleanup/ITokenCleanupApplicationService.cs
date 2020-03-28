using Abp.Application.Services;
using System.Threading.Tasks;

namespace Lpb.Identity.TokenCleanup
{
    public interface ITokenCleanupAppService : IApplicationService
    {
        Task TokenCleanup();

    }
}

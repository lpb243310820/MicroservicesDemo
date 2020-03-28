using Abp.Domain.Repositories;
using Abp.Timing;
using System.Linq;
using System.Threading.Tasks;

namespace Lpb.Identity.TokenCleanup
{
    public class TokenCleanupAppService : IdentityAppServiceBase, ITokenCleanupAppService
    {
        private readonly IRepository<PersistedGrantEntity, string> _persistedGrantRepository;

        public TokenCleanupAppService(IRepository<PersistedGrantEntity, string> persistedGrantRepository)
        {
            _persistedGrantRepository = persistedGrantRepository;
        }

        public async Task TokenCleanup()
        {
            await _persistedGrantRepository.DeleteAsync(x => x.Expiration.HasValue && x.Expiration.Value < Clock.Now);
        }

    }
}



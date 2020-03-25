using Microsoft.EntityFrameworkCore;

namespace Lpb.Identity
{
    public interface IAbpPersistedGrantDbContext
    {
        DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
    }
}

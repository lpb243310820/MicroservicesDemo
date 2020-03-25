using Abp.EntityFrameworkCore;
using Lpb.Identity.EntityMapper;
using Microsoft.EntityFrameworkCore;

namespace Lpb.Identity.EntityFrameworkCore
{
    public class IdentityDbContext : AbpDbContext, IAbpPersistedGrantDbContext
    {
        //Add DbSet properties for your entities...
        public DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePersistedGrantEntity();

        }
    }
}

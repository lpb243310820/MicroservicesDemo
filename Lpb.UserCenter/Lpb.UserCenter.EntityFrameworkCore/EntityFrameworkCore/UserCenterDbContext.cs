using Abp.EntityFrameworkCore;
using Lpb.UserCenter.EntityMapper;
using Microsoft.EntityFrameworkCore;

namespace Lpb.UserCenter.EntityFrameworkCore
{
    public class UserCenterDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...
        //public DbSet<Test> Tests { get; set; }

        public UserCenterDbContext(DbContextOptions<UserCenterDbContext> options) 
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new TestCfg());

        }
    }
}

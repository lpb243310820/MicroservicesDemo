using Microsoft.EntityFrameworkCore;

namespace Lpb.UserCenter.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<UserCenterDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for UserCenterDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}

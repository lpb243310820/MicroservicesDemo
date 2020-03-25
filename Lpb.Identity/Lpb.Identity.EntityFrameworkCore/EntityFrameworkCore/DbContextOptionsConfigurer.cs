using Microsoft.EntityFrameworkCore;

namespace Lpb.Identity.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<IdentityDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for TestDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}

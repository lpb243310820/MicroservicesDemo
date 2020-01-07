using Microsoft.EntityFrameworkCore;

namespace Lpb.Service1.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<Service1DbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for Service1DbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}

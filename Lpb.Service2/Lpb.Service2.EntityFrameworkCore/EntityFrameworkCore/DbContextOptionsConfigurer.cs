using Microsoft.EntityFrameworkCore;

namespace Lpb.Service2.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<Service2DbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for Service2DbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}

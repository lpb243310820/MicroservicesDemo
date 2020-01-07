using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Lpb.WebPortal.EntityFrameworkCore
{
    public static class WebPortalDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WebPortalDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WebPortalDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

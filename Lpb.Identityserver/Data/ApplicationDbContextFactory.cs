using IdentityServer4.EntityFramework.Options;
using Lpb.Identityserver.Data.Configuration;
using Lpb.Identityserver.Data.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lpb.Identityserver.Data
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantSqlDbContext>
    {
        public PersistedGrantSqlDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantSqlDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            builder.UseSqlServer(configuration.GetConnectionString("Default"));

            return new PersistedGrantSqlDbContext(builder.Options, new OperationalStoreOptions());
        }
    }
}
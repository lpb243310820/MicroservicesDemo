using Lpb.Service1.Configuration;
using Lpb.Service1.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lpb.Service1.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class Service1DbContextFactory : IDesignTimeDbContextFactory<Service1DbContext>
    {
        public Service1DbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<Service1DbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(Service1Consts.ConnectionStringName)
            );

            return new Service1DbContext(builder.Options);
        }
    }
}
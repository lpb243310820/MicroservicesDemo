using Lpb.Service2.Configuration;
using Lpb.Service2.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lpb.Service2.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class Service2DbContextFactory : IDesignTimeDbContextFactory<Service2DbContext>
    {
        public Service2DbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<Service2DbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(Service2Consts.ConnectionStringName)
            );

            return new Service2DbContext(builder.Options);
        }
    }
}
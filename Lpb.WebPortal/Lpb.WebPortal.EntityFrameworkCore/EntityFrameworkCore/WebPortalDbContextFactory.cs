using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Lpb.WebPortal.Configuration;
using Lpb.WebPortal.Web;

namespace Lpb.WebPortal.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class WebPortalDbContextFactory : IDesignTimeDbContextFactory<WebPortalDbContext>
    {
        public WebPortalDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WebPortalDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            WebPortalDbContextConfigurer.Configure(builder, configuration.GetConnectionString(WebPortalConsts.ConnectionStringName));

            return new WebPortalDbContext(builder.Options);
        }
    }
}

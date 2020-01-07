using Lpb.UserCenter.Configuration;
using Lpb.UserCenter.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Lpb.UserCenter.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class UserCenterDbContextFactory : IDesignTimeDbContextFactory<UserCenterDbContext>
    {
        public UserCenterDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserCenterDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(UserCenterConsts.ConnectionStringName)
            );

            return new UserCenterDbContext(builder.Options);
        }
    }
}
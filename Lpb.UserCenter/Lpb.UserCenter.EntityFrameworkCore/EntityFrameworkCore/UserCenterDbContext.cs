using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lpb.UserCenter.EntityFrameworkCore
{
    public class UserCenterDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public UserCenterDbContext(DbContextOptions<UserCenterDbContext> options) 
            : base(options)
        {

        }
    }
}

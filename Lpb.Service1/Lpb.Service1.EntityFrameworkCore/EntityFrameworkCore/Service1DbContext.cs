using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lpb.Service1.EntityFrameworkCore
{
    public class Service1DbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public Service1DbContext(DbContextOptions<Service1DbContext> options) 
            : base(options)
        {

        }
    }
}

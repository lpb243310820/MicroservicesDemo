using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lpb.Service2.EntityFrameworkCore
{
    public class Service2DbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public Service2DbContext(DbContextOptions<Service2DbContext> options) 
            : base(options)
        {

        }
    }
}

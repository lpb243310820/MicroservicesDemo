using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Lpb.WebPortal.Authorization.Roles;
using Lpb.WebPortal.Authorization.Users;
using Lpb.WebPortal.MultiTenancy;

namespace Lpb.WebPortal.EntityFrameworkCore
{
    public class WebPortalDbContext : AbpZeroDbContext<Tenant, Role, User, WebPortalDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public WebPortalDbContext(DbContextOptions<WebPortalDbContext> options)
            : base(options)
        {
        }
    }
}

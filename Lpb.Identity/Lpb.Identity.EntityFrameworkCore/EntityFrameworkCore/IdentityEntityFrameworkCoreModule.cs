using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;

namespace Lpb.Identity.EntityFrameworkCore
{
    [DependsOn(
        typeof(IdentityCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class IdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //设置时区
            Clock.Provider = ClockProviders.Local;//Clock.Now=DateTime.UtcNow
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IdentityEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
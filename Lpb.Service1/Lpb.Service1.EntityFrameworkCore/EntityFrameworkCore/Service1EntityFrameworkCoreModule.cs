using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lpb.Service1.EntityFrameworkCore
{
    [DependsOn(
        typeof(Service1CoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class Service1EntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service1EntityFrameworkCoreModule).GetAssembly());
        }
    }
}
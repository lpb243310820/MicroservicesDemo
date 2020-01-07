using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lpb.Service2.EntityFrameworkCore
{
    [DependsOn(
        typeof(Service2CoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class Service2EntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service2EntityFrameworkCoreModule).GetAssembly());
        }
    }
}
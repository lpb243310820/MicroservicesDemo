using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lpb.Service2
{
    [DependsOn(
        typeof(Service2CoreModule), 
        typeof(AbpAutoMapperModule))]
    public class Service2ApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service2ApplicationModule).GetAssembly());
        }
    }
}
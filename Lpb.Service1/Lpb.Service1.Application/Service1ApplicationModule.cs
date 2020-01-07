using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lpb.Service1
{
    [DependsOn(
        typeof(Service1CoreModule), 
        typeof(AbpAutoMapperModule))]
    public class Service1ApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service1ApplicationModule).GetAssembly());
        }
    }
}
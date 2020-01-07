using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lpb.UserCenter
{
    [DependsOn(
        typeof(UserCenterCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class UserCenterApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(UserCenterApplicationModule).GetAssembly());
        }
    }
}
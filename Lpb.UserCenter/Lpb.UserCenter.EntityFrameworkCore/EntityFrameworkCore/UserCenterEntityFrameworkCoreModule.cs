using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Lpb.UserCenter.EntityFrameworkCore
{
    [DependsOn(
        typeof(UserCenterCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class UserCenterEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(UserCenterEntityFrameworkCoreModule).GetAssembly());
        }
    }
}
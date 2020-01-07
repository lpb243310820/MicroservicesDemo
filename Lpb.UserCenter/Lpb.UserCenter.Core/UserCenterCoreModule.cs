using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.UserCenter.Localization;

namespace Lpb.UserCenter
{
    public class UserCenterCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            UserCenterLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(UserCenterCoreModule).GetAssembly());
        }
    }
}
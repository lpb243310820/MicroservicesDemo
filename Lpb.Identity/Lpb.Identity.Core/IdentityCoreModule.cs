using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Lpb.Identity.Localization;

namespace Lpb.Identity
{
    public class IdentityCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = false;

            IdentityLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IdentityCoreModule).GetAssembly());
        }
    }
}
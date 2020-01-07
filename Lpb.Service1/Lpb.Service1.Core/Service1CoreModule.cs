using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.Service1.Localization;

namespace Lpb.Service1
{
    public class Service1CoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            Service1LocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service1CoreModule).GetAssembly());
        }
    }
}
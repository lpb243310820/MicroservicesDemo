using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.Service2.Localization;

namespace Lpb.Service2
{
    public class Service2CoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            Service2LocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service2CoreModule).GetAssembly());
        }
    }
}
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.WebPortal.Authorization;

namespace Lpb.WebPortal
{
    [DependsOn(
        typeof(WebPortalCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class WebPortalApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WebPortalAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WebPortalApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}

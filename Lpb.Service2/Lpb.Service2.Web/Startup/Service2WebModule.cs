using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.Service2.Configuration;
using Lpb.Service2.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;

namespace Lpb.Service2.Web.Startup
{
    [DependsOn(
        typeof(Service2ApplicationModule), 
        typeof(Service2EntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class Service2WebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Service2WebModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(Service2Consts.ConnectionStringName);

            Configuration.Navigation.Providers.Add<Service2NavigationProvider>();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(Service2ApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service2WebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(Service2WebModule).Assembly);
        }
    }
}
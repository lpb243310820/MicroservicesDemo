using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.Service1.Configuration;
using Lpb.Service1.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;

namespace Lpb.Service1.Web.Startup
{
    [DependsOn(
        typeof(Service1ApplicationModule), 
        typeof(Service1EntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class Service1WebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Service1WebModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(Service1Consts.ConnectionStringName);

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(Service1ApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(Service1WebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(Service1WebModule).Assembly);
        }
    }
}
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.UserCenter.Configuration;
using Lpb.UserCenter.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;

namespace Lpb.UserCenter.Web.Startup
{
    [DependsOn(
        typeof(UserCenterApplicationModule), 
        typeof(UserCenterEntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class UserCenterWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public UserCenterWebModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(UserCenterConsts.ConnectionStringName);

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(UserCenterApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(UserCenterWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(UserCenterWebModule).Assembly);
        }
    }
}
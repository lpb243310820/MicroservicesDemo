using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Lpb.Identity.Configuration;
using Lpb.Identity.EntityFrameworkCore;
using Lpb.Identity.Web.BackgroundWorker;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using System;

namespace Lpb.Identity.Web.Startup
{
    [DependsOn(
        typeof(IdentityApplicationModule),
        typeof(IdentityEntityFrameworkCoreModule),
        typeof(AbpRedisCacheModule),
        typeof(AbpAspNetCoreModule))]
    public class IdentityWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public IdentityWebModule(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(IdentityConsts.ConnectionStringName);

            //配置使用Redis缓存
            Configuration.Caching.UseRedis(options =>
            {
                options.ConnectionString = _appConfiguration["RedisCache:ConnectionString"];
                options.DatabaseId = _appConfiguration.GetValue<int>("RedisCache:DatabaseId");
            });

            //配置所有Cache的默认过期时间为365天
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(_appConfiguration.GetValue<int>("RedisCache:DefaultSlidingExpireTime"));
                //cache.DefaultAbsoluteExpireTime = TimeSpan.FromDays(_appConfiguration.GetValue<int>("RedisCache:DefaultSlidingExpireTime") * 50);
            });

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(IdentityApplicationModule).GetAssembly()
                );

            //启用后台任务
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(IdentityWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(IdentityWebModule).Assembly);

            //后台工人
            var workManager = IocManager.Resolve<IdentityBackgroundWorker>();
            workManager.Start();
        }
    }
}
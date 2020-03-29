using Abp.AspNetCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using System;

namespace Lpb.Gateway.Web.Startup
{
    [DependsOn(
        typeof(AbpRedisCacheModule),
        typeof(AbpAspNetCoreModule))]
    public class GatewayWebModule : AbpModule
    {
        private readonly IConfiguration _appConfiguration;

        public GatewayWebModule(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(GatewayConsts.ConnectionStringName);

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
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GatewayWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(GatewayWebModule).Assembly);
        }
    }
}
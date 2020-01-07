using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PollyHttpClient
{
    public static class PollyConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //注册 ResilientHttpClientFactory，为什么是单例？？？？
            services.AddSingleton(typeof(ResilientHttpClientFactory), p =>
            {
                var logger = p.GetRequiredService<ILogger<ResilientHttpClient>>();
                var httpContextAccesser = p.GetRequiredService<IHttpContextAccessor>();
                var retryCount = configuration.GetValue<int>("Polly:retryCount");
                var exceptionsAllowedBeforeBreaking = configuration.GetValue<int>("Polly:exceptionsAllowedBeforeBreaking");

                return new ResilientHttpClientFactory(logger, httpContextAccesser, retryCount, exceptionsAllowedBeforeBreaking);
            });
            //全局注册单例IHttpClient，为什么是单例？？？？
            services.AddSingleton<IHttpClient>(p => p.GetService<ResilientHttpClientFactory>().CreateResilientHttpClient());
        }
    }
}
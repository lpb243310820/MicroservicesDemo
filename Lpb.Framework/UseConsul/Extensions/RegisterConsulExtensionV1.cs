using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;

namespace UseConsul.Extensions
{
    /// <summary>
    /// 注册服务到Consul（自动注册）。
    /// </summary>
    public static class RegisterConsulExtensionV1
    {
        /// <summary>
        /// 添加consul
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            //配置consul注册地址
            services.AddOptions();
            services.Configure<ServiceRegisterOptions>(configuration.GetSection("ServiceRegister"));

            //配置consul客户端
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                var consulOptions = sp.GetRequiredService<IOptions<ServiceRegisterOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(consulOptions.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(consulOptions.Consul.HttpEndPoint);
                }
            }));

            return services;
        }

        /// <summary>
        /// 使用consul
        /// 默认的健康检查接口格式是 http://host:port/HealthCheck
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            IApplicationLifetime appLife = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            IOptions<ServiceRegisterOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ServiceRegisterOptions>>();

            //TODO: 这两句代码，必须使用Microsoft.NETCore.App 2.1 的程序集
            var features = app.Properties["server.Features"] as FeatureCollection;
            //var port = new Uri(features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault()).Port; //获取端口
            var port = serviceOptions.Value.ServicePort; //获取配置文件的端口

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"application port is :{port}");

            var addressIpv4Hosts = NetworkInterface.GetAllNetworkInterfaces()
                .OrderByDescending(c => c.Speed)
                .Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);

            foreach (var item in addressIpv4Hosts)
            {
                //这是ipv4的ip地址
                var firstIpV4Address = item.GetIPProperties().UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault().ToString();

                var serviceId = $"{serviceOptions.Value.ServiceName}_{firstIpV4Address}:{port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(10), //健康检查间隔10s，原来是30s
                    HTTP = $"{Uri.UriSchemeHttp}://{firstIpV4Address}:{port}/HealthCheck" //这个是默认健康检查接口
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = firstIpV4Address.ToString(),
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                //当服务停止后想consul发送的请求
                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"health check service:{httpCheck.HTTP}");
            }

            return app;
        }
    }
}

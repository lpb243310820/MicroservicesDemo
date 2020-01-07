using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Collections.Generic;

namespace CapRabbitMq
{
    public static class CapConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            //注册cap之前一定要注册服务
            //services.AddTransient<ISubscriberService, SubscriberService>();

            services.AddCap(x =>
            {
                //如果你使用的 EF 进行数据操作，你需要添加如下配置：
                //x.UseEntityFramework<WeixinDbContext>();  //可选项，你不需要再次配置 x.UseMySql 了

                //如果你使用的 MongoDB，你可以添加如下配置：
                //x.UseMySql(_appConfiguration["CapDbUrl"]);
                x.UseMySql(configuration["Cap:DbUrl"]);
                //x.UseMongoDB(_appConfiguration["MongoDBUrl"]);  //注意，仅支持MongoDB 4.0+集群

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = configuration["CAP:RabbitMq:HostName"];
                    if (!string.IsNullOrEmpty(configuration["CAP:RabbitMq:VirtualHost"])) cfg.VirtualHost = configuration["CAP:RabbitMq:VirtualHost"];
                    cfg.Port = configuration.GetValue<int>("CAP:RabbitMq:Port");
                    cfg.UserName = configuration["CAP:RabbitMq:UserName"];
                    cfg.Password = configuration["CAP:RabbitMq:Password"];
                    cfg.ExchangeName = configuration["CAP:RabbitMq:ExchangeName"];

                    if (configuration.GetValue<bool>("CAP:RabbitMq:Aliyun"))
                    {
                        cfg.ConnectionFactoryOptions = opt =>
                        {
                            opt.TopologyRecoveryEnabled = true;
                            opt.AuthMechanisms = new List<AuthMechanismFactory>() { new AliyunMechanismFactory() };
                        };
                    }
                }); // RabbitMQ
                //rabbitmq --> queue name
                x.DefaultGroup = configuration["CAP:RabbitMq:QueueName"];

                // 注册 Dashboard
                x.UseDashboard();

                // 注册节点到 Consul
                x.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = configuration["CAP:Discovery:ServerHostName"];
                    d.DiscoveryServerPort = configuration.GetValue<int>("CAP:Discovery:ServerPort");
                    d.CurrentNodeHostName = configuration["CAP:Discovery:CurrentNodeHostName"];
                    d.CurrentNodePort = configuration.GetValue<int>("CAP:Discovery:CurrentNodePort");
                    d.NodeId = configuration["CAP:Discovery:NodeId"];
                    d.NodeName = configuration["CAP:Discovery:NodeName"];
                });

                // Below settings is just for demo
                x.FailedRetryCount = 2;
                x.FailedRetryInterval = 5;
            });
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using ServiceDiscovery.Customer;
using ServiceDiscovery.DataCenter;
using ServiceDiscovery.Diagnose;
using ServiceDiscovery.Doctor;
using ServiceDiscovery.IdentityServer;
using ServiceDiscovery.IoT;
using ServiceDiscovery.Message;
using ServiceDiscovery.Pay;
using ServiceDiscovery.UserCenter;
using ServiceDiscovery.Weixin;

namespace ServiceDiscovery
{
    public static class ServiceConfigurer
    {
        public static void Configure(IServiceCollection services)
        {
            DeviceConfigurer.Configure(services);

            //注册服务
            services
                .AddTransient<ICustomerService, CustomerService>()
                .AddTransient<IUpdateDataCenterService, UpdateDataCenterService>()
                .AddTransient<IDiagnoseService, DiagnoseService>()
                .AddTransient<IDoctorService, DoctorService>()
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IIoTService, IoTService>()
                .AddTransient<IMessageService, MessageService>()
                .AddTransient<IPayService, PayService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IWeixinService, WeixinService>()
                ;
        }
    }
}

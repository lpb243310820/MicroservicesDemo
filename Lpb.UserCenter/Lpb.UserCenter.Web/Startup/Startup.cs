using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using CapRabbitMq;
using Castle.Facilities.Logging;
using Lpb.UserCenter.Configuration;
using Lpb.UserCenter.EntityFrameworkCore;
using Lpb.UserCenter.Web.Subscriber;
using Lpb.UserCenter.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PollyHttpClient;
using System;
using System.Linq;
using UseConsul;

namespace Lpb.UserCenter.Web.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        private readonly IConfiguration _appConfiguration;
        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Configure DbContext
            services.AddAbpDbContext<UserCenterDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddNewtonsoftJson();

            //注册Polly
            PollyConfigurer.Configure(services, _appConfiguration);

            //注册cap之前一定要注册服务
            services.AddTransient<ISubscriberService, SubscriberService>();
            //注册CAP，RabbitMQ
            CapConfigurer.Configure(services, _appConfiguration);

            //配置consul注册
            services.AddConsul(_appConfiguration);
            //配置consul发现
            services.AddConsulServiceDiscovery(_appConfiguration);

            //添加jwt认证授权
            AuthConfigurer.Configure(services, _appConfiguration);

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "用户中心", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<UserCenterWebModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            //添加jwt认证授权
            app.UseAuthentication();

            //启动Consul
            app.UseConsul();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "用户中心");
                options.InjectBaseUrl(_appConfiguration["App:ServerRootAddress"]);
            }); //URL: /swagger
        }
    }
}

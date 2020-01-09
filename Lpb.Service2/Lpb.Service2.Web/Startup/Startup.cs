using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Lpb.Service2.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Lpb.Service2.Configuration;
using System.Linq;
using Abp.Extensions;
using Lpb.Service2.Web.Swagger;
using Microsoft.OpenApi.Models;
using PollyHttpClient;
using UseConsul;
using CapRabbitMq;
using Lpb.Service2.Web.Subscriber;

namespace Lpb.Service2.Web.Startup
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
            services.AddAbpDbContext<Service2DbContext>(options =>
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
                        //.WithOrigins(
                        //    // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                        //    _appConfiguration["App:CorsOrigins"]
                        //        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        //        .Select(o => o.RemovePostFix("/"))
                        //        .ToArray()
                        //)
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        //.AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Service2", Version = "v1" });
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
            return services.AddAbp<Service2WebModule>(options =>
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
                options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "Service2");
                options.InjectBaseUrl(_appConfiguration["App:ServerRootAddress"]);
            }); //URL: /swagger
        }
    }
}

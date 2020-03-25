using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Castle.Facilities.Logging;
using IdentityServer4.Services;
using Lpb.Identity.Configuration;
using Lpb.Identity.EntityFrameworkCore;
using Lpb.Identity.Web.Authentication;
using Lpb.Identity.Web.PersistedGrantStore;
using Lpb.Identity.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UseConsul;

namespace Lpb.Identity.Web.Startup
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
            services.AddAbpDbContext<IdentityDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            //配置consul注册
            services.AddConsul(_appConfiguration);

            // Ids4发布在正式环境时，需要设置IIS加载用户配置
            // 应用程序池--->高级设置--->加载用户配置文件设置为True
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var certPath = Path.Combine(basePath, _appConfiguration["Certificates:CertPath"]);

            //注入IPersistedGrantStore的实现，用于存储AuthorizationCode和RefreshToken等等，默认实现是存储在内存中，
            //如果服务重启那么这些数据就会被清空了，因此可实现IPersistedGrantStore将这些数据写入到数据库或者NoSql(Redis)中
            //services.AddSingleton<IPersistedGrantStore, MyPersistedGrantStore>();

            //参考文章：https://aspnetboilerplate.com/Pages/Documents/Zero/Identity-Server
            //在IdentityRegistrar.Register（services）之后添加了services.AddIdentityServer（）
            services.AddIdentityServer()
                //.AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(certPath, _appConfiguration["Certificates:Password"]))
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(_appConfiguration))
                .AddExtensionGrantValidator<CustomerAuthCodeValidator>()
                .AddExtensionGrantValidator<DoctorAuthCodeValidator>()
                .AddExtensionGrantValidator<PortalAuthCodeValidator>()
                .AddAbpPersistedGrants<IdentityDbContext>()
                //.AddPersistedGrantStore<MyPersistedGrantStore>()
                //.AddAbpIdentityServer<User>();
                //.AddTestUsers(IdentityServerConfig.GetUsers())
                ;

            services.AddTransient<IProfileService, ProfileService>();

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

            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ids4服务", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                ////使用相对路径获取应用程序所在目录
                //var commentsApplicationFileName = typeof(IdentityApplicationModule).Assembly.GetName().Name + ".XML";
                //options.IncludeXmlComments(Path.Combine(basePath, commentsApplicationFileName));

                var commentsWebFileName = typeof(IdentityWebModule).Assembly.GetName().Name + ".XML";
                options.IncludeXmlComments(Path.Combine(basePath, commentsWebFileName));
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<IdentityWebModule>(options =>
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
                options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "Identity");
                options.InjectBaseUrl(_appConfiguration["App:ServerRootAddress"]);
            }); //URL: /swagger
        }
    }
}

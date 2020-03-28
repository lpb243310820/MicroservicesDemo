using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Lpb.Identityserver.Authentication;
using Lpb.Identityserver.Data;
using Lpb.Identityserver.PersistedGrantStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using UseConsul;

namespace Lpb.Identityserver
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //��������ַ���
            services.AddDbContext<PersistedGrantSqlDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddOptions();
            services.AddControllers();

            //����consulע��
            services.AddConsul(Configuration);

            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            basePath = Path.Combine(basePath, Configuration["Certificates:CertPath"]);

            //ע��IPersistedGrantStore��ʵ�֣����ڴ洢AuthorizationCode��RefreshToken�ȵȣ�Ĭ��ʵ���Ǵ洢���ڴ��У�
            //�������������ô��Щ���ݾͻᱻ����ˣ���˿�ʵ��IPersistedGrantStore����Щ����д�뵽���ݿ����NoSql(Redis)��
            //services.AddSingleton<IPersistedGrantStore, MyPersistedGrantStore>();
            var connectionString = Configuration["ConnectionStrings:Default"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                //.AddDeveloperSigningCredential()
                //.AddSigningCredential(new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), Configuration["Certificates:CertPath"]), Configuration["Certificates:Password"]))
                .AddSigningCredential(new X509Certificate2(basePath, Configuration["Certificates:Password"]))
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(Configuration))
                .AddExtensionGrantValidator<CustomerAuthCodeValidator>()
                .AddExtensionGrantValidator<DoctorAuthCodeValidator>()
                .AddExtensionGrantValidator<PortalAuthCodeValidator>()
                //.AddPersistedGrantStore<MyPersistedGrantStore>()
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore<PersistedGrantSqlDbContext>(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                })
                ;

            services.AddTransient<IProfileService, ProfileService>();

            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "��Ȩ����", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net();

            //IdentityServer
            app.UseIdentityServer();

            //����Consul
            app.UseConsul();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "��Ȩ����");
            }); //URL: /swagger

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

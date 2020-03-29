using Abp.Extensions;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Ocelot.Cache.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Errors.Middleware;
using Ocelot.Headers.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using Ocelot.Provider.Polly;
using Ocelot.RateLimit.Middleware;
using Ocelot.Request.Middleware;
using Ocelot.Requester.Middleware;
using Ocelot.RequestId.Middleware;
using Ocelot.Responder.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lpb.Gateway
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
            services.AddControllers();

            string address = Configuration["Authority:Address"];
            int port = Configuration.GetValue<int>("Authority:Port");
            int JwtValidationClockSkew = Configuration.GetValue<int>("Authority:JwtValidationClockSkew");

            var authenticationServiceAProviderKey = "ServiceAKey";//5001
            var authenticationServiceBProviderKey = "ServiceBKey";//5002

            Action<IdentityServerAuthenticationOptions> optionsA = o =>
            {
                o.Authority = $"http://{address}:{port}"; //IdentityServer的Ocelot设置端口
                o.RequireHttpsMetadata = false;
                o.ApiName = "api_a";
                o.SupportedTokens = SupportedTokens.Both;
                o.JwtValidationClockSkew = TimeSpan.FromSeconds(JwtValidationClockSkew);
                o.ApiSecret = "secret";
            };
            Action<IdentityServerAuthenticationOptions> optionsB = o =>
            {
                o.Authority = $"http://{address}:{port}"; //IdentityServer的Ocelot设置端口
                o.RequireHttpsMetadata = false;
                o.ApiName = "api_b";
                o.SupportedTokens = SupportedTokens.Both;
                o.JwtValidationClockSkew = TimeSpan.FromSeconds(JwtValidationClockSkew);
                o.ApiSecret = "secret";
            };
            
            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationServiceAProviderKey, optionsA)
                .AddIdentityServerAuthentication(authenticationServiceBProviderKey, optionsB)
                ;

            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("Ocelot.json").Build())
                .AddPolly();

            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new Info { Title = "网关服务", Version = "v1" });
            //    options.DocInclusionPredicate((docName, description) => true);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var args = new List<string>
            {
                "api_a",
                "api_b"
            };
            //// Enable middleware to serve generated Swagger as a JSON endpoint
            //app.UseSwagger();
            //app.UseSwaggerUI(options =>
            //{
            //    foreach (var item in args)
            //    {
            //        options.SwaggerEndpoint($"/doc/{item}/swagger.json", item);
            //    }
            //}); //URL: /swagger

            //通过网关设置跨域
            app.UseOcelot((ocelotBuilder, pipelineConfiguration) =>
            {
                // This is registered to catch any global exceptions that are not handled
                // It also sets the Request Id if anything is set globally
                ocelotBuilder.UseExceptionHandlerMiddleware();
                // This is registered first so it can catch any errors and issue an appropriate response
                ocelotBuilder.UseResponderMiddleware();
                ocelotBuilder.UseDownstreamRouteFinderMiddleware();
                ocelotBuilder.UseDownstreamRequestInitialiser();
                ocelotBuilder.UseRequestIdMiddleware();
                ocelotBuilder.UseMiddleware<ClientRateLimitMiddleware>();
                ocelotBuilder.UseMiddleware<ClaimsToHeadersMiddleware>();
                ocelotBuilder.UseLoadBalancingMiddleware();
                ocelotBuilder.UseDownstreamUrlCreatorMiddleware();
                ocelotBuilder.UseOutputCacheMiddleware();
                ocelotBuilder.UseMiddleware<HttpRequesterMiddleware>();
                // cors headers
                ocelotBuilder.Use(async (context, next) =>
                {
                    if (!context.DownstreamResponse.Headers.Exists(h => h.Key == HeaderNames.AccessControlAllowOrigin))
                    {
                        var allowed = Configuration["CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                        context.DownstreamResponse.Headers.Add(new Header(HeaderNames.AccessControlAllowOrigin, allowed.Length == 0 ? new[] { "*" } : allowed));
                    }
                    if (!context.DownstreamResponse.Headers.Exists(h => h.Key == HeaderNames.AccessControlAllowHeaders))
                    {
                        var allowed = Configuration["Headers"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                        context.DownstreamResponse.Headers.Add(new Header(HeaderNames.AccessControlAllowHeaders, allowed.Length == 0 ? new[] { "*" } : allowed));
                    }
                    if (!context.DownstreamResponse.Headers.Exists(h => h.Key == HeaderNames.AccessControlRequestMethod))
                    {
                        var allowed = Configuration["Method"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                        context.DownstreamResponse.Headers.Add(new Header(HeaderNames.AccessControlRequestMethod, allowed.Length == 0 ? new[] { "*" } : allowed));
                    }
                    await next();
                });
            });
            //app.UseOcelot().Wait();
        }
    }
}

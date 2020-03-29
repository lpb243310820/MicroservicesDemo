using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Castle.Facilities.Logging;
using Lpb.Gateway.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Headers.Middleware;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Ocelot.RateLimit.Middleware;
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
using Microsoft.Net.Http.Headers;
using System.Linq;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lpb.RedisKey;
using TokenCheck;

namespace Lpb.Gateway.Web.Startup
{
    public class Startup
    {
        private readonly IConfiguration _appConfiguration;
        private readonly ICacheManager _cacheManager;

        public Startup(IConfiguration appConfiguration, ICacheManager cacheManager)
        {
            _appConfiguration = appConfiguration;
            _cacheManager = cacheManager;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Configure DbContext
            //services.AddAbpDbContext<Service2DbContext>(options =>
            //{
            //    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            //});

            services.AddControllers();

            //添加jwt认证授权
            AuthConfigurer.Configure(services, _appConfiguration);

            //Ocelot
            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("Ocelot.json").Build())
                .AddPolly();

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "网关服务", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                //// Define the BearerAuth scheme that's in use
                //options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<GatewayWebModule>(options =>
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

            //添加jwt认证授权
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
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
                        var allowed = _appConfiguration["CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                        context.DownstreamResponse.Headers.Add(new Header(HeaderNames.AccessControlAllowOrigin, allowed.Length == 0 ? new[] { "*" } : allowed));
                    }
                    if (!context.DownstreamResponse.Headers.Exists(h => h.Key == HeaderNames.AccessControlAllowHeaders))
                    {
                        var allowed = _appConfiguration["Headers"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                        context.DownstreamResponse.Headers.Add(new Header(HeaderNames.AccessControlAllowHeaders, allowed.Length == 0 ? new[] { "*" } : allowed));
                    }
                    if (!context.DownstreamResponse.Headers.Exists(h => h.Key == HeaderNames.AccessControlRequestMethod))
                    {
                        var allowed = _appConfiguration["Method"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                        context.DownstreamResponse.Headers.Add(new Header(HeaderNames.AccessControlRequestMethod, allowed.Length == 0 ? new[] { "*" } : allowed));
                    }
                    await next();
                });

                // TokenCheck
                ocelotBuilder.Use(async (context, next) =>
                {
                    var model = context.HttpContext.JwtDecoder();
                    if (model != null)
                    {
                        //token黑名单
                        List<string> tokenBlacklist = _cacheManager.GetCache(CacheKeyService.BlacklistToken).Get(model.client_id + model.sub, () => new List<string>());
                        if (tokenBlacklist.Count > 0 && !string.IsNullOrWhiteSpace(model.token))
                        {
                            //包含在黑名单中
                            if (tokenBlacklist.Contains(model.token))
                            {
                                context.HttpContext.Response.StatusCode = 456;
                                await HandleExceptionAsync(context.HttpContext, 456, "您已经在其他设备登陆，请重新登陆");
                                return;
                            }
                        }
                    }
                    await next();
                });
            });
            //app.UseOcelot().Wait();
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var data = new { code = statusCode.ToString(), is_success = false, msg = msg };
            var result = JsonConvert.SerializeObject(new { data = data });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}

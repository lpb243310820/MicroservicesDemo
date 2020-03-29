using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lpb.Gateway.Web.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["Authentication:IsEnabled"]))
            {
                string address = configuration["Authentication:Address"];
                int port = configuration.GetValue<int>("Authentication:Port");
                int JwtValidationClockSkew = configuration.GetValue<int>("Authentication:JwtValidationClockSkew");

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
            }
        }
    }
}

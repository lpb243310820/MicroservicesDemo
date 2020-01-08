using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Lpb.UserCenter.Web.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                })
                .AddJwtBearer("JwtBearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = configuration["Authentication:JwtBearer:Audience"];
                    options.Authority = configuration["Authentication:JwtBearer:Authority"];

                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    // The signing key must match!
                    //    ValidateIssuerSigningKey = true,
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                    //    // Validate the JWT Issuer (iss) claim
                    //    ValidateIssuer = true,
                    //    ValidIssuer = configuration["Authentication:JwtBearer:Authority"],

                    //    // Validate the JWT Audience (aud) claim
                    //    ValidateAudience = true,
                    //    ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                    //    // Validate the token expiry
                    //    ValidateLifetime = true,

                    //    // If you want to allow a certain amount of clock drift, set that here
                    //    ClockSkew = TimeSpan.Zero
                    //};

                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = QueryStringTokenResolver,

                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        Console.WriteLine("401");
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            }
        }

        /* This method is needed to authorize SignalR javascript client.
         * SignalR can not send authorization header. So, we are getting it from query string as an encrypted text. */
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }
            // http://192.168.0.226/.well-known/openid-configuration
            var claims = context.Principal.Claims.ToList();
            Console.WriteLine(claims.ToJsonString());

            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }

            // Set auth token from cookie
            //context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken, AppConsts.DefaultPassPhrase);
            return Task.CompletedTask;
        }
    }
}

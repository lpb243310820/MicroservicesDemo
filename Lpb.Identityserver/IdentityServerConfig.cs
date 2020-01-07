using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Lpb.Identityserver
{
    public static class IdentityServerConfig
    {
        /// <summary>
        /// 允许使用认证服务的api列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api_a", "5001"),
                new ApiResource("api_b", "5002")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration appConfiguration)
        {
            //List<string> grantTypes = GrantTypes.ClientCredentials.Union(GrantTypes.ResourceOwnerPassword).ToList();
            List<string> customerGrantTypes = GrantTypes.ClientCredentials.ToList();
            customerGrantTypes.Add("customer_auth_code");

            List<string> doctorGrantTypes = GrantTypes.ClientCredentials.ToList();
            doctorGrantTypes.Add("doctor_auth_code");

            return new List<Client>
            {
                new Client
                {
                    ClientId = appConfiguration["customer:ClientId"],
                    AllowedGrantTypes = customerGrantTypes,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = appConfiguration.GetValue<int>("customer:SlidingRefreshTokenLifetime"),//15 days
                    AbsoluteRefreshTokenLifetime = appConfiguration.GetValue<int>("customer:AbsoluteRefreshTokenLifetime"),//30 days
                    AccessTokenLifetime = appConfiguration.GetValue<int>("customer:AccessTokenLifetime"),
                    AllowOfflineAccess = true,
                    //RequireClientSecret = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowedScopes =
                    {
                        "api_a",
                        "api_b",
                        //IdentityServerConstants.StandardScopes.OfflineAccess,
                        //IdentityServerConstants.StandardScopes.OpenId,//控制是否返回id_token
                        //IdentityServerConstants.StandardScopes.Profile
                    },
                    ClientSecrets = { new Secret(appConfiguration["customer:ClientSecrets"].Sha256())}
                },
                new Client
                {
                    ClientId = appConfiguration["doctor:ClientId"],
                    AllowedGrantTypes = doctorGrantTypes,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = appConfiguration.GetValue<int>("doctor:SlidingRefreshTokenLifetime"),//15 days
                    AbsoluteRefreshTokenLifetime = appConfiguration.GetValue<int>("doctor:AbsoluteRefreshTokenLifetime"),//30 days
                    AccessTokenLifetime = appConfiguration.GetValue<int>("doctor:AccessTokenLifetime"),
                    AllowOfflineAccess = true,
                    //RequireClientSecret = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    AllowedScopes =
                    {
                        "api_a",
                    },
                    ClientSecrets = { new Secret(appConfiguration["doctor:ClientSecrets"].Sha256())}
                }
            };
        }
    }
}
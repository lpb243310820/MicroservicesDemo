using Castle.Core.Logging;
using DnsClient;
using Lpb.Dto.IdentityServer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PollyHttpClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using UseConsul;

namespace ServiceDiscovery.IdentityServer
{
    public class TokenService : BaseService, ITokenService
    {
        public TokenService(ILogger logger, IOptions<ServiceDiscoveryOptions> options, IDnsQuery dnsQuery, IHttpClient httpClient, IHostingEnvironment env)
            : base(logger, httpClient, dnsQuery, env, options.Value.IdentityServiceName, options.Value.LocalDebugAddress)
        {
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="form">字典</param>
        /// <param name="clientId">客户端Id</param>
        /// <returns>TokenModel</returns>
        public async Task<TokenModel> RequestToken(Dictionary<string, string> form, string clientId)
        {
            return await TokenPostServices<TokenModel>(_serviceUrl + "/connect/token", form);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="form">字典</param>
        /// <param name="clientId">客户端Id</param>
        /// <returns>TokenModel</returns>
        public async Task<TokenModel> RefreshToken(Dictionary<string, string> form, string clientId)
        {
            return await TokenPostServices<TokenModel>(_serviceUrl + "/connect/token", form); 
        }
    }
}
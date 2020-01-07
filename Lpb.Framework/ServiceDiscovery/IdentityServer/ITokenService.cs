using Lpb.Dto.IdentityServer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDiscovery.IdentityServer
{
    public interface ITokenService
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="form">字典</param>
        /// <param name="clientId">客户端Id</param>
        /// <returns>TokenModel</returns>
        Task<TokenModel> RequestToken(Dictionary<string, string> form, string clientId);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="form">字典</param>
        /// <param name="clientId">客户端Id</param>
        /// <returns>TokenModel</returns>
        Task<TokenModel> RefreshToken(Dictionary<string, string> form, string clientId);
    }
}
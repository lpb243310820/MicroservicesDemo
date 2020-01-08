using System;

namespace Lpb.RedisKey
{
    /// <summary>
    /// 微服务缓存Key
    /// </summary>
    public class CacheKeyService
    {
        /// <summary>
        /// 网关Token，key:ClientId + userId,value:string
        /// </summary>
        public const string GatewayToken = "GatewayToken";
        /// <summary>
        /// 软件登陆的设备唯一Id，key:ClientId + userId,value:deviceUUID List
        /// </summary>
        public const string DeviceUser = "DeviceUser";
        /// <summary>
        /// 软件在单一固定设备上的Token，key:ClientId + userId + deviceUUID,value:List string
        /// </summary>
        public const string DeviceToken = "DeviceToken";
        /// <summary>
        /// 网关Token黑名单，key:ClientId + userId,value:string
        /// </summary>
        public const string BlacklistToken = "BlacklistToken";
        /// <summary>
        /// 刷新token，key:ClientId + userId,value:string
        /// </summary>
        public const string RefreshToken = "RefreshToken";

    }

}

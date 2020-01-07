namespace Lpb.Dto.UserCenter.Token
{
    /// <summary>
    /// RequestToken函数需要的传参
    /// </summary>
    public class RequestTokenDto
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 联合Id，微信登录用
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 极光推送的Id
        /// </summary>
        public string JiguangId { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 手机号码登录用密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 小程序使用手机号码和密码登录时
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string DeviceUUID { get; set; } = "DeviceUUID";
    }
}
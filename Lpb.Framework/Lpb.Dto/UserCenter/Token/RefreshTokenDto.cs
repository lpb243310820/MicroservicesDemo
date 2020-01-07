namespace Lpb.Dto.UserCenter.Token
{
    /// <summary>
    /// 刷新Token函数需要的传参
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string DeviceUUID { get; set; } = "DeviceUUID";
    }
}

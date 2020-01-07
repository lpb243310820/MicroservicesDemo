namespace Lpb.Dto.UserCenter.Token
{
    /// <summary>
    /// RequestToken函数的返回值
    /// </summary>
    public class ResponseTokenModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int ExpiresIn { get; set; }
        /// <summary>
        /// token类型
        /// </summary>
        public string TokenType { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 是否需要设置密码
        /// </summary>
        public bool SetPassword { get; set; }
        /// <summary>
        /// 成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回的信息
        /// </summary>
        public string Msg { get; set; }
    }
}
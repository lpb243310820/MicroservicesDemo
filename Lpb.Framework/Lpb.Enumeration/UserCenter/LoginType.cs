using Lpb.Extend;

namespace Lpb.Enumeration
{
    /// <summary>
    /// 登录类型枚举
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 微信公众号
        /// </summary>
        [Remark("微信公众号", Description = "微信公众号")]
        WeChat = 0,
        /// <summary>
        /// 小程序（单独一套机制 https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/union-id.html）
        /// </summary>
        [Remark("小程序", Description = "小程序")]
        WxOpen = 1,
        /// <summary>
        /// APP
        /// </summary>
        [Remark("APP", Description = "APP")]
        App = 2,
    }
}

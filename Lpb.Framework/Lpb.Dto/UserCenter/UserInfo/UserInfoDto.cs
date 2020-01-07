

using Abp.Application.Services.Dto;
using System;

namespace Lpb.Dto.UserCenter.UserInfo
{
    public class UserListDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// 同一个微信开放平台帐号下的移动应用、网站应用和公众帐号，用户的unionid是唯一的。换句话说，同一用户，对同一个微信开放平台下的不同应用，unionid是相同的。
        /// </summary>
        public string UnionId { get; set; }

        /// <summary>
        /// 微信公众平台（针对app的用户唯一Id）
        /// </summary>
        public string AppOpenId { get; set; }

        /// <summary>
        /// 微信公众号（针对微信公众号的用户唯一Id）
        /// </summary>
        public string MpOpenId { get; set; }

        /// <summary>
        /// 小程序（针对小程序的用户唯一Id）
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 极光推送的Id
        /// </summary>
        public string JiguangId { get; set; }

        /// <summary>
        /// 环信Id
        /// </summary>
        public string HuanxinId { get; set; }


        /// <summary>
        /// 用户身份证号码
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// 性别，0：未知，1：男，2：女，冗余信息
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// 昵称（默认是Laiba_guid）
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// 访问失败次数
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// 是否可以锁定
        /// </summary>
        public bool IsLockoutEnabled { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户是否被激活，默认是激活的
        /// </summary>
        public bool IsActive { get; set; }
    }

}
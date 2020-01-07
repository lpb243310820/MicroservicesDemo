using Lpb.Extend;

namespace Lpb.Enumeration
{
    public enum UserRole
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Remark("用户", Description = "用户")]
        Customer = 1,
        /// <summary>
        /// 医生
        /// </summary>
        [Remark("医生", Description = "医生")]
        Doctor = 2
    }
}

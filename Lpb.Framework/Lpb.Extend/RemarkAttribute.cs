using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lpb.Extend
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取当前枚举值的Remark
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetRemark(this Enum value)
        {
            string remark = string.Empty;
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            try
            {
                object[] attrs = fieldInfo.GetCustomAttributes(typeof(RemarkAttribute), false);
                RemarkAttribute attr = (RemarkAttribute)attrs.FirstOrDefault(a => a is RemarkAttribute);
                if (attr == null)
                {
                    remark = fieldInfo.Name;
                }
                else
                {
                    remark = attr.Remark;
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("EnumExtension的GetRemark出现异常");
                //logger.Error("EnumExtension的GetRemark出现异常", ex);
                remark = string.Empty;
            }

            return remark;
        }

        /// <summary>
        /// 获取当前枚举的全部Remark
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetAllRemarks(this Enum value)
        {
            Type type = value.GetType();
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            //ShowAttribute.
            foreach (var field in type.GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    object tmp = field.GetValue(value);
                    Enum enumValue = (Enum)tmp;
                    int intValue = (int)tmp;
                    result.Add(new KeyValuePair<string, string>(intValue.ToString(), enumValue.GetRemark()));
                }
            }
            return result;
        }
    }

    /// <summary>
    /// RemarkAttribute
    /// </summary>
    public class RemarkAttribute : Attribute
    {
        private string _remark = string.Empty;
        public RemarkAttribute(string remark)
        {
            _remark = remark;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
            }
        }

        public string Description
        { get; set; }
    }


    public enum ShowAttribute
    {
        /// <summary>
        /// 这里是good的注释
        /// </summary>
        [Remark("这里是good", Description = "这里是good的Description")]
        Good = 0,
        Bad = 1
    }
}

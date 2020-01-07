using System;

namespace Lpb.Extend
{
    public static class DateTimeDiff
    {
        /// <summary>
        /// 计算两个日期之间的月间隔
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static int toResult(this DateTime d1, DateTime d2)
        {
            DateTime max;
            DateTime min;

            if (d1 > d2)
            {
                max = d1;
                min = d2;
            }
            else
            {
                max = d2;
                min = d1;
            }
            int year = max.Year - min.Year;
            int month = max.Month - min.Month;
            int day = max.Day - min.Day;

            return month + year * 12 + (day < 0 ? -1 : 0);
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UsuallyCommon
{
    /// <summary>
    /// DateTime Extension
    /// 时间扩展
    /// </summary>
    public static class DatetimeExtensions
    {
        /// <summary>
        /// 获取时间戳（秒）
        /// </summary>
        /// <returns>long</returns>
        public static long GetTimeSeconds()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); 
        }

        /// <summary>
        /// 获取时间戳（毫秒）
        /// </summary>
        /// <returns>long</returns>
        public static long GetTimeMilliseconds()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }


        /// <summary>
        /// 获取当月的第一天
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime GetCurrentMonthFirstDay()
        {
            return DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
        }

        /// <summary>
        /// 获取当月的最后一天
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime GetCurrentMonthLastDay()
        {
            return DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1);
        }


        /// <summary>
        /// 获取当年的第一天
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime GetCurrentYearFirstDay()
        {
            return new DateTime(DateTime.Now.Year, 1, 1); 
        }

        /// <summary>
        /// 获取当年的最后一天
        /// </summary>
        /// <returns>DateTime</returns>
        public static DateTime GetCurrentYearLastDay()
        {
            return new DateTime(DateTime.Now.Year, 12, 31);
        }

  
    }
}

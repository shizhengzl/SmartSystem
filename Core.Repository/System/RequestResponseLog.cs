using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace Core.Repository
{
    /// <summary>
    /// 请求日志表
    /// </summary>
    [Description("请求日志表")]
    public class RequestResponseLog : SysBaseEntity
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        [Column(StringLength = -1)]
        [Description("请求地址")]
        public string Url { get; set; }

        /// <summary>
        /// 请求Header
        /// </summary>
        [Column(StringLength = -1)]
        [Description("请求Header")]
        public String Headers { get; set; }


        /// <summary>
        /// 请求方法
        /// </summary>
        [Column(StringLength = -1)]
        [Description("请求方法")]
        public string Method { get; set; }

        /// <summary>
        /// 请求Body
        /// </summary>
        [Column(StringLength = -1)]
        [Description("请求Body")]
        public string RequestBody { get; set; }


        /// <summary>
        /// 响应Body
        /// </summary>
        [Column(StringLength = -1)]
        [Description("响应Body")]
        public string ResponseBody { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        [Description("执行开始时间")]
        public DateTime ExcuteStartTime { get; set; }

        /// <summary>
        /// 执行完成时间
        /// </summary> 
        [Description("执行完成时间")]
        public DateTime ExcuteEndTime { get; set; }


        /// <summary>
        /// 请求IP地址
        /// </summary> 
        [Description("请求IP地址")]
        public String IPAddress { get; set; }


        /// <summary>
        /// 请求端口
        /// </summary> 
        [Description("请求端口")]
        public Int32 Port { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        public String UserName { get; set; }


        //public override string ToString()
        //{
        //    string headers = "[" + string.Join(",", this.Headers.Select(i => "{" + $"\"{i.Key}\":\"{i.Value}\"" + "}")) + "]";
        //    return $"Url: {this.Url},\r\nHeaders: {headers},\r\nMethod: {this.Method},\r\nRequestBody: {this.RequestBody},\r\nResponseBody: {this.ResponseBody},\r\nExcuteStartTime: {this.ExcuteStartTime.ToString("yyyy-MM-dd HH:mm:ss.fff")},\r\nExcuteStartTime: {this.ExcuteEndTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
        //}
    }

}

using Core.Services;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices.Middleware
{
    public class GlobalExceptionFilter : IExceptionFilter
    {

        private SystemServices _sysservices { get; set; }

        public GlobalExceptionFilter(SystemServices sysservices)
        {
            _sysservices = sysservices;
        }

        public void OnException(ExceptionContext context)
        {

            //日志入库
            //向负责人发报警邮件，异步
            //向负责人发送报警短信或者报警电话，异步 
            Exception ex = context.Exception; 
            // 这里获取服务器ip时，需要考虑如果是使用nginx做了负载，这里要兼容负载后的ip，
            // 监控了ip方便定位到底是那台服务器出故障了
            string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();

            _sysservices.AddExexptionLogs(ex, context.ActionDescriptor.DisplayName);

            //_logger.LogError($"系统编号：{sysId},主机IP:{ip},堆栈信息：{ex.StackTrace},异常描述：{ex.Message}");
 

            context.Result  = new JsonResult
            (new {
                Success = false,
                Message = ex.Message,
                Code = 20000
            });
            context.ExceptionHandled = true;
             
        }
    }
}

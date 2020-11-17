using Core.Repository;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppServices.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private AppSystemServices _appSystemServices { get; set; }
      

        public RequestResponseLoggingMiddleware(RequestDelegate next, AppSystemServices appSystemServices)
        {
            _next = next;
            _appSystemServices = appSystemServices;
        }

        public async Task Invoke(HttpContext context)
        {
            RequestResponseLog _logInfo = new RequestResponseLog();

            HttpRequest request = context.Request;
            _logInfo.Url = request.Path.ToString();
            IDictionary<string, string> Headers = request.Headers.ToDictionary(k => k.Key, v => string.Join(";", v.Value.ToList()));

            _logInfo.Headers = $"[" + string.Join(",", Headers.Select(i => "{" + $"\"{i.Key}\":\"{i.Value}\"" + "}")) + "]";

            _logInfo.Method = request.Method;
            _logInfo.ExcuteStartTime = DateTime.Now;

            _logInfo.IPAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();
            _logInfo.Port = request.HttpContext.Connection.RemotePort;

            //获取request.Body内容
            if (request.Method.ToLower().Equals("post"))
            {

                request.EnableRewind(); //启用倒带功能，就可以让 Request.Body 可以再次读取

                Stream stream = request.Body;
                byte[] buffer = new byte[request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                _logInfo.RequestBody = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0; 

            }
            else if (request.Method.ToLower().Equals("get"))
            {
                _logInfo.RequestBody = request.QueryString.Value;
            }


            using (var responseBody = new MemoryStream())
            { 
                //获取Response.Body内容
                var originalBodyStream = context.Response.Body;
                context.Response.Body = responseBody;

                await _next(context);

                _logInfo.ResponseBody = await FormatResponse(context.Response); 

                //Log4Net.LogInfo($"VisitLog: {_logInfo.ToString()}");

                await responseBody.CopyToAsync(originalBodyStream);
            }

            context.Response.OnCompleted(async o =>  { 
             
                _logInfo.ExcuteEndTime = DateTime.Now;
                _appSystemServices.Create<RequestResponseLog>(_logInfo);

            },context); 
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin); 
            return text;
        }
    }

    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}

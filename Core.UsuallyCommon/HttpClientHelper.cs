using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.UsuallyCommon
{
    public static class HttpClientHelper
    {
        /// <summary>
        /// 获取Get请求String
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="container">cookie</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, CookieContainer container)
        {
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = container };
            HttpClient client = new HttpClient(handler);
            var result = await client.GetAsync(url);
            return await result.Content.ReadAsStringAsync();
        }
    }
}

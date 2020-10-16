using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UsuallyCommon
{
    public class ResponseDto<T>  
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = string.Empty;


        /// <summary>
        /// 总数
        /// </summary>
        public Int32 Total { get; set; }
    }
}

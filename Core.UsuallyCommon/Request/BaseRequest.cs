using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UsuallyCommon
{
    public class BaseRequest
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public Int32 PageIndex { get; set; }


        /// <summary>
        /// 页大小
        /// </summary>
        public Int32 PageSize { get; set; }
    }
}

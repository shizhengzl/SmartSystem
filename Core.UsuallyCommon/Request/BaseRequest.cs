using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UsuallyCommon
{
    public class BaseRequest<T>
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public Int32 PageIndex { get; set; } = 1;


        /// <summary>
        /// 页大小
        /// </summary>
        public Int32 PageSize { get; set; } = 20;

        public  T Search { get; set; }
    }
}

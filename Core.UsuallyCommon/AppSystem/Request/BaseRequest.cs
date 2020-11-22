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


        /// <summary>
        /// 总数
        /// </summary>
        public Int64 TotalCount { get; set; } = 20;

        /// <summary>
        /// 查询属性
        /// </summary>
        public  T Model { get; set; }

        /// <summary>
        /// 模糊查询
        /// </summary>
        public String Filter { get; set; }


        /// <summary>
        /// 排序字段
        /// </summary>
        public String Sort { get; set; }


        /// <summary>
        /// 升序
        /// </summary>
        public Boolean Asc { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public String ClassType { get; set; }
    }
}

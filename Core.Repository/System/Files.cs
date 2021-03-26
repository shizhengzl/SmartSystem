using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 文件
    /// </summary>
    [Description("文件")]
    public class Files : SysBaseEntity
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [Description("文件名称")]
        [Column(StringLength = 100)]
        public String FileName { get; set; }
 
        /// <summary>
        /// 文件路径
        /// </summary>
        [Description("文件路径")]
        [Column(StringLength = -1)]
        public String FilePath { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        [Description("文件")]
        [Column(StringLength = -1)]
        public String FileData { get; set; }


        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Description("文件扩展名")]
        [Column(StringLength = 100)]
        public String FileExt { get; set; }
         
    }
}

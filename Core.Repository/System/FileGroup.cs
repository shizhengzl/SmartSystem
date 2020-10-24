using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    public class FileGroup : SysBaseEntity
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
        /// 文件扩展名
        /// </summary>
        [Description("文件扩展名")]
        [Column(StringLength = 100)]
        public String FileExt { get; set; }


        /// <summary>
        /// 文件组ID
        /// </summary>
        [Description("文件组ID")]
        public Guid FileGroupId { get; set; }


    }
}

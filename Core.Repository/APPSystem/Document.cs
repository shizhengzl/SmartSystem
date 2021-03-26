using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 文档管理
    /// </summary>
    [Description("文档管理")]
    public class Document : SysBaseEntity
    {
        /// <summary>
        /// 文档名称
        /// </summary>
        [Description("文档名称")]
        [Column(StringLength = 200)]
        public String DocumentName { get; set; }



        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

        ///<summary>
        /// 子节点
        /// </summary> 
        [Description("子节点")]
        [Column(IsIgnore = true)]
        public List<Document> children { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        [Description("父级")]
        public Int64 ParentId { get; set; }


        /// <summary>
        /// 是否是文件
        /// </summary>
        [Description("文件")] 
        public Boolean IsFile { get; set; }


        /// <summary>
        /// 文件类型
        /// </summary>
        [Description("文件类型")]
        [Column(StringLength = 200)]
        public String DocumentType { get; set; }


        /// <summary>
        /// 文档数据
        /// </summary>
        [Description("文档数据")]
        [Column(StringLength = -1)]
        public String DocumentData { get; set; }

    }
}

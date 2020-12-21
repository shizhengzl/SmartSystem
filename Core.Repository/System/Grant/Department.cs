using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Description("部门表")]
    public class Department : SysBaseEntity
    {

        /// <summary>
        /// 单位ID
        /// </summary> 
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Description("部门名称")]
        [Column(StringLength = 200)]
        public String DepartmentName { get; set; }


        /// <summary>
        /// 部门描述
        /// </summary>
        [Description("部门描述")]
        [Column(StringLength = -1)]
        public String Description { get; set; }


        /// <summary>
        /// 部门负责人
        /// </summary>
        [Description("部门负责人")]
        [Column(StringLength = -1)]
        public Int64 ManagerUserId { get; set; }


        /// <summary>
        /// 子节点
        /// </summary> 
        [Description("子节点")]
        [Column(IsIgnore = true)]
        public List<Menus> children { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Description("父级ID")]
        public Int64 ParentId { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        [Column(StringLength = 100)]
        [Description("父级名称")]
        public String ParentName { get; set; }
    }
}

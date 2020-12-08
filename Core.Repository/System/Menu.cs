using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{

    /// <summary>
    /// 菜单表
    /// </summary>
    [Description("菜单表")]
    public class Menus : SysBaseEntity
    { 
        ///  <summary>
        /// 菜单名称
        /// </summary>
        [Description("菜单名称")]
        public String MenuName { get; set; }

        

        ///  <summary>
        /// 菜单层级
        /// </summary>
        [Description("菜单层级")]
        public Int16? MenuLevel { get; set; }

        ///  <summary>
        /// 菜单排序
        /// </summary>
        [Description("菜单排序")]
        public Int32? MenuOrder { get; set; }

        ///  <summary>
        /// 是否启用
        /// </summary>
        [Description("是否启用")]
        public Boolean? IsAvailable { get; set; }

        ///  <summary>
        /// 菜单URL
        /// </summary>
        [Description("菜单URL")]
        public String Url { get; set; }

        ///  <summary>
        /// 菜单图标
        /// </summary>
        [Description("菜单图标")]
        public String MenuIcon { get; set; }

        ///  <summary>
        /// 菜单路径
        /// </summary>
        [Description("菜单路径")]
        public String MenuPath { get; set; } 
        ///  <summary>
        /// 是否总是显示
        /// </summary>
        [Description("是否总是显示")]
        public Boolean? IsAlwaysShow { get; set; }


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

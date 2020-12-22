using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 单位表
    /// </summary>
    [Description("单位表")]
    public class Company : SysBaseEntity
    { 
        /// <summary>
        /// 单位名称
        /// </summary>
        [Column(StringLength = 200)]
        [Description("单位名称")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 信用代码
        /// </summary>
        [Column(StringLength = 200)]
        [Description("信用代码")]
        public string CorpCode { get; set; } 

        /// <summary>
        /// 单位地址
        /// </summary>
        [Column(StringLength = 200)]
        [Description("单位地址")]
        public string CompanyAddress { get; set; }
         
        /// <summary>
        /// 单位区域
        /// </summary>
        [Column(StringLength = 200)]
        [Description("单位区域")]
        public string CompanyRegionCode { get; set; }

        /// <summary>
        /// 单位法人
        /// </summary>
        [Column(StringLength = 200)]
        [Description("单位法人")]
        public string CompanyLegal { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 200)]
        [Description("手机号")]
        public string CompanyPhone { get; set; }

        /// <summary>
        /// 单位电话
        /// </summary>
        [Column(StringLength = 200)]
        [Description("单位电话")]
        public string CompanyTel { get; set; }
         
        /// <summary>
        /// 子单位
        /// </summary> 
        [Description("子单位")]
        [Column(IsIgnore = true)]
        public List<Company> children { get; set; }

        /// <summary>
        /// 父单位
        /// </summary>
        [Description("父单位")]
        public Int64 ParentId { get; set; }

        /// <summary>
        /// 父单位名称
        /// </summary>
        [Column(StringLength = 100)]
        [Description("父单位名称")]
        public String ParentName { get; set; }
          
        /// <summary>
        /// 授权模式
        /// </summary> 
        [Description("授权模式")]
        public GrantMode GrantMode { get; set; }

        /// <summary>
        /// 我的单位
        /// </summary> 
        [Description("我的单位")]
        [Column(IsIgnore = true)]
        public Boolean IsMy { get; set; }

    }
}

using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// SQL配置类
    /// </summary>
    [Description("SQL配置表")]
    public class SQLConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity =true)]
        [Description("主键")]
        public Int32 Id { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        [Description("数据库类型")]
        public DataType Type { get; set; }
        /// <summary>
        /// 查询所有数据库SQL
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取数据库")]
        public string GetDataBaseSQL { get; set; }
        /// <summary>
        /// 获取表
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取表")]
        public string GetTableSQL { get; set; }
        /// <summary>
        /// 获取列
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取列")]
        public string GetColumnSQL { get; set; }
        /// <summary>
        /// 获取存储过程
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取存储过程")]
        public string GetProducuteSQL { get; set; }
        /// <summary>
        /// 获取试图
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取试图")]
        public string GetViewSQL { get; set; }
        /// <summary>
        /// 获取索引
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取索引")]
        public string GetIndexSQL { get; set; }
        /// <summary>
        /// 获取同义词
        /// </summary>
        [Column(StringLength = -1)]
        [Description("获取同义词")]
        public string GetSYNONYMSQL { get; set; }


        /// <summary>
        /// 添加列备注
        /// </summary>
        [Column(StringLength = -1)]
        [Description("添加列备注")]
        public string AddExtendedproperty { get; set; }

        /// <summary>
        /// 修改列备注
        /// </summary>
        [Column(StringLength = -1)]
        [Description("修改列备注")]
        public string ModifyExtendedproperty { get; set; }



        /// <summary>
        /// 添加表备注
        /// </summary>
        [Description("添加表备注")]
        [Column(StringLength = -1)]
        public string AddTableExtendedproperty { get; set; }

        /// <summary>
        /// 修改表备注
        /// </summary>
        [Column(StringLength = -1)]
        [Description("修改表备注")]
        public string ModifyTableExtendedproperty { get; set; }
    }
}

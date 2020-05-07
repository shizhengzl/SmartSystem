using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.Generator
{
    public class DBService
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public Int32 Id { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerAddress { get; set; }


        /// <summary>
        /// 服务账号
        /// </summary>
        public string Account { get; set; }


        /// <summary>
        /// 服务密码
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DataType { get; set; }


        /// <summary>
        /// 默认数据库
        /// </summary>
        public string DefaultDataBase { get; set; }


    }
}

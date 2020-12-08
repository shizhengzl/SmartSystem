using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UsuallyCommon
{
    public class CommonEnum
    {


        /// <summary>
        /// 系统超级单位
        /// </summary>
        public static string SupperCompany { get { return "知科云科技信息有限公司"; } }

        /// <summary>
        /// 系统超级管理员
        /// </summary>
        public static string SupperAdmin { get { return "系统超级管理员"; } }

        /// <summary>
        /// 游客
        /// </summary>
        public static string Tourist { get { return "游客"; } }
        

        /// <summary>
        /// 超级管理员用户
        /// </summary>
        public static string SupperUser { get { return "admin"; } }

        /// <summary>
        /// 游客用户
        /// </summary>
        public static string Youke { get { return "youke"; } }


        public static readonly Int32 ToLoginCode = 50008;
    }
}

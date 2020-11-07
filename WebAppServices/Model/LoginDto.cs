using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices
{
    public class LoginDto
    {
        /// <summary>
        /// 用户名或者Phone
        /// </summary>
        public string Username { get; set; }


        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
    }
}

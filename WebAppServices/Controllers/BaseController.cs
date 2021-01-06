using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.UsuallyCommon;

namespace WebAppServices.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public class BaseController : ControllerBase
    {
        private UserDto GetUsers()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var users = MemoryCacheManager.GetCache<UserDto>(token);

            return users;
        }

        private String GetToken()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        /// <summary>
        /// 获取token
        /// </summary>
        public String Token { get { return GetToken(); }  }


        /// <summary>
        /// 活当前用户
        /// </summary>
        public UserDto CurrentUser { get { return GetUsers(); } }



       
    } 
}

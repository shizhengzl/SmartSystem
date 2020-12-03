using Core.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices.Controllers
{
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

        public String Token { get { return GetToken(); }  }


        public UserDto CurrentUser { get { return GetUsers(); } }
    }
}

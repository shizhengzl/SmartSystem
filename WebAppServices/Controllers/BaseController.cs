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
        public UserDto GetUsers()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var users = MemoryCacheManager.GetCache<UserDto>(token);

            return users;
        }


        public UserDto CurrentUser { get { return GetUsers(); } }
    }
}

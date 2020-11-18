using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Mvc;

namespace WebAppServices
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {

     
        public PermissionAuthorizationHandler()
        {
            
        } 

          

        /// <summary>
        /// 判断是否授权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            Microsoft.AspNetCore.Http.HttpContext httpContext = ((Microsoft.AspNetCore.Http.DefaultHttpContext)((Microsoft.AspNetCore.Mvc.ActionContext)context.Resource).HttpContext);
         

            var authorizationFilterContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            var user = httpContext;
            if (user == null)
                authorizationFilterContext.Result = new JsonResult("Not Logged In") { StatusCode = 403 };


            //if (requirement.Name.ToStringExtension() != Permissions.UserLogin && user != null)
            //{
            //    if (!_permissions.IsAdmin(user.UserId, user.CommerceId.ToInt64()) && !_permissions.CheckPermission(user.UserId, requirement.Name, user.CommerceId.ToInt64()))
            //        authorizationFilterContext.Result = new JsonResult("Not Permissions") { StatusCode = 401 };
            //}

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

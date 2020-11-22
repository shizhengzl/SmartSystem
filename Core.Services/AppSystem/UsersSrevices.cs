using AutoMapper;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.UsuallyCommon;

namespace Core.Services.AppSystem
{
    public class UsersSrevices : BaseServices
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="mapper"></param>
        public UsersSrevices(IMapper mapper) : base(mapper) {

        }


        public List<RoleUsers> GetUserRoles(Int64 UserId)
        {
            return FreeSqlFactory._Freesql.Select<RoleUsers>().Where(x=>x.UserId == UserId).ToList();
        }

        public List<Menus> GetUserMenus(Int64 UserId)
        {
            var roles = this.GetUserRoles(UserId).Select(x=>x.RoleId).ToList(); 
            var adminrole = FreeSqlFactory._Freesql.Select<Roles>().Where(x => x.RoleName == CommonEnum.SupperAdmin).First().Id;

            Boolean isAdmin = roles.Any(x => x == adminrole);
            if (isAdmin)
                return FreeSqlFactory._Freesql.Select<Menus>().ToList();
            else
            {
                var menuIds = FreeSqlFactory._Freesql.Select<RoleMenus>().Where(x => roles.Contains(x.RoleId)).ToList().Select(p=>p.MenuId);
                return FreeSqlFactory._Freesql.Select<Menus>().Where(x=> menuIds.Contains(x.Id)).ToList();
            }
        }



        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<UserDto> GetUserList()
        {
            List<UserDto> response = new List<UserDto>();
            var list = FreeSqlFactory._Freesql.Select<Users>().ToList();
            response = _mapper.Map<List<UserDto>>(list);
            return response;
        }


        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public UserDto GetUser(String phone)
        { 
            var response =  FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Phone == phone || x.UserName == phone).ToList().FirstOrDefault(); 
            return _mapper.Map<UserDto>(response);
        }
         

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Boolean RegisterUser(UserDto dto)
        {
            Boolean result = false;
            var entity = _mapper.Map<Users>(dto);
            FreeSqlFactory._Freesql.Insert<Users>(entity).ExecuteAffrows();
 
            return result;
        }
    }
}

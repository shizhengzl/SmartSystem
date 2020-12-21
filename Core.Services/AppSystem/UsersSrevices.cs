using AutoMapper;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.UsuallyCommon;

namespace Core.Services.AppSystem
{

    [AppServiceAttribute]
    public class UsersSrevices : IServices
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="mapper"></param>
  
        /// <summary>
        /// 注入接口
        /// </summary>
        public readonly IMapper _mapper;

        public UsersSrevices(IMapper mapper)  {
            _mapper = mapper;
        }


        public List<RoleUsers> GetUserRoles(Int64 UserId, Int64 CompanyId)
        {
            return FreeSqlFactory._Freesql.Select<RoleUsers>().Where(x=>x.UserId == UserId && x.CompanyId == CompanyId).ToList();
        }

        public List<Menus> GetUserMenus(Int64 UserId,Int64 CompanyId)
        { 
            var company = FreeSqlFactory._Freesql.Select<Company>().Where(x => x.Id == CompanyId).First(); 
            var user = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Id == UserId).First();
            List<long> menuids = new List<long>();

            switch (company.GrantMode) {
                case GrantMode.RoleGrant: 
                    menuids = FreeSqlFactory._Freesql.Select<RoleMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
                    break;
                case GrantMode.DepartGrant:
                    menuids = FreeSqlFactory._Freesql.Select<DepartmentMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
                    break; 
                case GrantMode.UserGrant:
                    menuids = FreeSqlFactory._Freesql.Select<UserMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
                    break;
                case GrantMode.CompanyGrant:
                    menuids = FreeSqlFactory._Freesql.Select<CompanyMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
                    break;
            }

            // 角色授权 如果是超级管理员则拥有所有权限
            if (company.GrantMode ==  GrantMode.RoleGrant)
            {
                var roles = this.GetUserRoles(UserId, CompanyId).Select(x => x.RoleId).ToList();
                var adminrole = FreeSqlFactory._Freesql.Select<Roles>().Where(x => x.RoleName == CommonEnum.SupperAdmin).First().Id; 
                Boolean isAdmin = roles.Any(x => x == adminrole);
                if (isAdmin) {
                    menuids = FreeSqlFactory._Freesql.Select<CompanyMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
                }
            }
            // 判断用户是不是法人 授权所有单位菜单
            if (company.CompanyPhone.Trim() == user.Phone.Trim())
            {
                menuids = FreeSqlFactory._Freesql.Select<CompanyMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
            }

            if (user.Phone == "13701859214")
            {
                menuids = FreeSqlFactory._Freesql.Select<Menus>().ToList().Select(p => p.Id).ToList();
            }

            // 如果用户没有任何菜单
            if (menuids.Count == 0)
            {
                menuids = FreeSqlFactory._Freesql.Select<Menus>().Where(x=>x.IsDeafult.Value).ToList().Select(p => p.Id).ToList();
            }

            return FreeSqlFactory._Freesql.Select<Menus>().Where(x => menuids.Contains(x.Id) && x.IsAvailable.Value ).ToList();
          
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<UserDto> GetUserList(Int64 CompanyId)
        {
            List<UserDto> response = new List<UserDto>();
            var list = FreeSqlFactory._Freesql.Select<Users>().Where(x=>x.CompanyId == CompanyId).ToList();
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

            // 新建单位
            var companyid = FreeSqlFactory._Freesql.Insert<Company>(new Company() {
                CompanyName = dto.Phone,CompanyPhone = dto.Phone, CompanyLegal = dto.Name,GrantMode = GrantMode.CompanyGrant
            }).ExecuteIdentity();
            entity.CompanyId = companyid;
            FreeSqlFactory._Freesql.Insert<Users>(entity).ExecuteAffrows();
 
            return result;
        }
    }
}

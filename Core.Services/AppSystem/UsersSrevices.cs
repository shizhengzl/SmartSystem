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

        public UsersSrevices(IMapper mapper)
        {
            _mapper = mapper;
        }


        public List<RoleUsers> GetUserRoles(Int64 UserId, Int64 CompanyId)
        {
            return FreeSqlFactory._Freesql.Select<RoleUsers>().Where(x => x.UserId == UserId && x.CompanyId == CompanyId).ToList();
        }

        public List<Menus> GetUserMenus(Int64 UserId, Int64 CompanyId)
        {
            var company = FreeSqlFactory._Freesql.Select<Company>().Where(x => x.Id == CompanyId).First();
            var user = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Id == UserId).First();
            List<long> menuids = new List<long>();
            Boolean isAdmin = false;
            var roleIds = this.GetUserRoles(UserId, CompanyId).Select(x => x.RoleId).ToList();
            var adminrole = FreeSqlFactory._Freesql.Select<Roles>().Where(x => x.RoleName == CommonEnum.SupperAdmin).ToOne();
            if (!adminrole.IsNull()) 
                isAdmin = roleIds.Any(x => x == adminrole.Id);
            // 如果是adin 或者是法人 直接走单位授权 
            if (isAdmin || company.CompanyPhone.Trim() == user.Phone.ToStringExtension().Trim())
            {
                company.GrantMode = GrantMode.CompanyGrant;
            }
            switch (company.GrantMode)
            {
                case GrantMode.RoleGrant:
                    menuids = FreeSqlFactory._Freesql.Select<RoleMenus>().Where(x => x.CompanyId == CompanyId && roleIds.Contains(x.RoleId)).ToList().Select(p => p.MenuId).ToList();
                    break;
                case GrantMode.DepartGrant:
                    // 获取用户所有部门 
                    var userdepartIds = FreeSqlFactory._Freesql.Select<DepartmentUsers>().Where(x => x.UserId == UserId).ToList().Select(p => p.DepartmentId).ToList();
                    menuids = FreeSqlFactory._Freesql.Select<DepartmentMenus>().Where(x => x.CompanyId == CompanyId && userdepartIds.Contains(x.DepartmentId)).ToList().Select(p => p.MenuId).ToList();
                    break;
                case GrantMode.UserGrant:
                    menuids = FreeSqlFactory._Freesql.Select<UserMenus>().Where(x => x.CompanyId == CompanyId && x.UserId == UserId).ToList().Select(p => p.MenuId).ToList();
                    break;
                case GrantMode.CompanyGrant:
                    menuids = FreeSqlFactory._Freesql.Select<CompanyMenus>().Where(x => x.CompanyId == CompanyId).ToList().Select(p => p.MenuId).ToList();
                    break;
            } 
            if (user.Phone == "13701859214")
            {
                menuids = FreeSqlFactory._Freesql.Select<Menus>().ToList().Select(p => p.Id).ToList();
            }

            //// 如果用户没有任何菜单
            //if (menuids.Count == 0)
            //{
            //    menuids = FreeSqlFactory._Freesql.Select<Menus>().Where(x => x.IsDeafult.Value).ToList().Select(p => p.Id).ToList();
            //}

            return FreeSqlFactory._Freesql.Select<Menus>().Where(x => menuids.Contains(x.Id) && x.IsAvailable.Value).ToList();

        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<UserDto> GetUserList(Int64 CompanyId)
        {
            List<UserDto> response = new List<UserDto>();
            var list = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.CompanyId == CompanyId).ToList();
            response = _mapper.Map<List<UserDto>>(list);
            return response;
        }


        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public UserDto GetUser(String phone)
        {
            var user = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Phone == phone || x.UserName == phone).ToList().FirstOrDefault(); 
            var company = FreeSqlFactory._Freesql.Select<Company>().Where(x => x.Id == user.CompanyId).ToOne();
            var response = _mapper.Map<UserDto>(user);
            response.CompanyName = company.CompanyName;
            return response;
        }

        /// <summary>
        /// 获取实体用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Users GetEntityUser(String phone)
        {
            var response = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Phone == phone || x.UserName == phone).ToList().FirstOrDefault();
            return response;
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
            var companyid = FreeSqlFactory._Freesql.Insert<Company>(new Company()
            {
                CompanyName = dto.Phone,
                CompanyPhone = dto.Phone,
                CompanyLegal = dto.Name,
                GrantMode = GrantMode.CompanyGrant
            }).ExecuteIdentity();
            entity.CompanyId = companyid;

            var userid = FreeSqlFactory._Freesql.Insert<Users>(entity).ExecuteIdentity();

            entity = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Id == userid).ToList().First();

            entity.Password = (entity.Password + userid.ToStringExtension()).ToMD5();
            FreeSqlFactory._Freesql.Update<Users>().SetSource(entity).ExecuteAffrows();

            // 加入单位用户
            CompanyUsers companyUsers = new CompanyUsers()
            {
                UserId = userid,
                JobStatus = JobStatus.InJob,
                CompanyId = companyid
            };

            FreeSqlFactory._Freesql.Insert<CompanyUsers>(companyUsers).ExecuteAffrows();

            dto.Id = userid;
            return result;
        }
    }
}

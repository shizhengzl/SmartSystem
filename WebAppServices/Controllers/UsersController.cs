using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Repository;
using Core.Services;
using Core.Services.AppSystem;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppServices.Model;
using static AutoMapper.Internal.ExpressionFactory;
namespace WebAppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public UsersController(IMapper mapper
            , UsersSrevices usersSrevices
            , SystemServices sysservices
            , DataBaseServices dataBaseServices
            , AppSystemServices appSystemServices
            )
        {
            _mapper = mapper;
            _userServices = usersSrevices;
            _sysservices = sysservices;
            _dataBaseServices = dataBaseServices;
            _appSystemServices = appSystemServices;
        }


        [HttpPost("GetHeader")]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            response.Data = _dataBaseServices.GetColumns(typeof(Users).Name);

            return response;
        }



        [HttpPost("GetUserInfo")]
        [Authorize]
        public IActionResult GetUserInfo()
        {

            // 获取用请求携带token
            var users = this.CurrentUser;

            var menus = _userServices.GetUserMenus(this.CurrentUser.Id, CurrentUser.CompanyId);


            List<MenuTree> router = new List<MenuTree>();
            // 组织menus
            var parent = menus.Where(x => x.ParentId.ToInt32() == 0).ToList();
            parent.ForEach(p =>
            {
                router.Add(GetMenuTree(p, menus));
            });

            // 获取用户角色
            var roles = new List<string>() { "admin" };
            // 获取用户菜单
            return Ok(new { Success = true, code = 20000, roles, router = router, name = users.UserName, avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif" });
        }


        private MenuTree GetMenuTree(Menus menu, List<Menus> menus)
        {
            var child = GetChilds(menu.Id, menus);
            return new MenuTree()
            {
                Id = menu.Id,
                path = menu.MenuPath,
                component = menu.Url,
                name = menu.MenuName,
                meta = new MenuMeta() { icon = menu.MenuIcon, title = menu.MenuName, noCache = true },
                children = child,
                redirect = child.Count == 0 ? null : child.FirstOrDefault().path
            };
        }


        private List<MenuTree> GetChilds(Int64 ParentID, List<Menus> menus)
        {
            List<MenuTree> result = new List<MenuTree>();
            menus.Where(x => x.ParentId == ParentID).ToList().ForEach(p => result.Add(GetMenuTree(p, menus)));
            return result;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]

        [HttpPost("GetResult")]
        public ResponseListDto<Users> GetResult([FromBody] BaseRequest<Users> request)
        {
            ResponseListDto<Users> response = new ResponseListDto<Users>();

            var data = _appSystemServices.GetEntitys<Users>().Where(x => x.CompanyId == CurrentUser.CompanyId);

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.UserName.Contains(request.Filter) || x.Phone.Contains(request.Filter));
                }

                if (!string.IsNullOrEmpty(request.Sort.ToStringExtension()))
                {
                    data = data.OrderByPropertyName(request.Sort, request.Asc.ToBoolean());
                }
                else
                {
                    data = data.OrderBy(x => x.Id);
                }
            }
            response.Total = data.Count();
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Users>();

            return response;
        }


        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<Users> Save([FromBody] Users request)
        {
            ResponseDto<Users> response = new ResponseDto<Users>();

            var _entity = _appSystemServices.GetEntitys<Users>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                var userId = _appSystemServices.Create<Users>(request);

                if (!request.DepartmentId.IsNull())
                    _appSystemServices.Create<DepartmentUsers>(new DepartmentUsers() { CompanyId = CurrentUser.CompanyId, DepartmentId = request.DepartmentId, UserId = userId });

                if (!request.RoleId.IsNull() && request.RoleId.Count > 0)
                {
                    request.RoleId.ForEach(p => {
                        _appSystemServices.Create<RoleUsers>(new RoleUsers() { CompanyId = CurrentUser.CompanyId, RoleId = p, UserId = userId });
                    });
                } 
            }
            else
            {
                _appSystemServices.Modify<Users>(request);

                if (!request.DepartmentId.IsNull())
                {
                    _appSystemServices.GetEntitys<DepartmentUsers>().Where(x => x.UserId == request.Id && x.CompanyId == CurrentUser.CompanyId).ToDelete();
                    _appSystemServices.Create<DepartmentUsers>(new DepartmentUsers() { CompanyId = CurrentUser.CompanyId, DepartmentId = request.DepartmentId, UserId = request.Id });

                    if (!request.RoleId.IsNull() && request.RoleId.Count > 0)
                    {
                        _appSystemServices.GetEntitys<RoleUsers>().Where(x => x.UserId == request.Id && x.CompanyId == CurrentUser.CompanyId).ToDelete();
                        request.RoleId.ForEach(p => {
                            _appSystemServices.Create<RoleUsers>(new RoleUsers() { CompanyId = CurrentUser.CompanyId, RoleId = p, UserId = request.Id });
                        });
                    }
                   
                }
            }
            return response;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] Users request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();


            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<Users>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }


        /// <summary>
        /// 获取单位用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsersList")]
        [Authorize]
        public ResponseListDto<UserDto> GetUsersList()
        {
            ResponseListDto<UserDto> response = new ResponseListDto<UserDto>();
            try
            {
                response.Data = _userServices.GetUserList(CurrentUser.CompanyId);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetUsersList");
            }
            return response;
        }




        [HttpPost("Logout")]
        public ResponseDto<String> Logout()
        {
            MemoryCacheManager.Remove(this.Token);
            return new ResponseDto<string>() { Code = CommonEnum.ToLoginCode };
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ResponseDto<String> Login([FromBody] LoginDto user)
        {
            ResponseDto<String> response = new ResponseDto<String>() { Success = false };
            try
            {
                if (string.IsNullOrEmpty(user.Username))
                {
                    response.Message = "请输入用户名或者手机号码";
                    return response;
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    response.Message = "请输入密码";
                    return response;
                }

                user.Password = user.Password.ToMD5();

                var users = _userServices.GetUser(user.Username);
                if (users == null)
                {
                    response.Message = "用户名不正确";
                    return response;
                }
                if (users.Password != user.Password)
                {
                    response.Message = "密码不正确";
                    return response;
                }
                if (!users.IsEnabled)
                {
                    response.Message = "用户已经禁用";
                    return response;
                }


                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, users.Id.ToStringExtension())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PublicConst.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: PublicConst.Domain,
                    audience: PublicConst.Domain,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                response.Success = true;
                response.Data = new JwtSecurityTokenHandler().WriteToken(token);


                MemoryCacheManager.SetRefushCache<UserDto>(response.Data, users, TimeSpan.FromMinutes(30));
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Login");
            }
            return response;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public ResponseDto<bool> Register([FromBody] UserDto user)
        {
            ResponseDto<bool> response = new ResponseDto<bool>();
            try
            {

                if (string.IsNullOrEmpty(user.Phone))
                {
                    response.Message = "请输入手机号";
                    response.Success = false;
                }

                var searchuser = _userServices.GetUser(user.Phone);
                if (searchuser != null)
                {
                    response.Message = "手机号码已经注册";
                    response.Success = false;
                }
                else
                {
                    user.Password = user.Password.ToMD5();
                    response.Data = _userServices.RegisterUser(user);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Register");
            }
            return response;
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetMenus")]
        [Authorize]
        public ResponseListDto<UserMenus> GetMenus([FromBody] Users request)
        {
            ResponseListDto<UserMenus> response = new ResponseListDto<UserMenus>();
            var data = _appSystemServices.GetEntitys<UserMenus>();

            if (!request.IsNull())
            {
                data = data.Where(x => x.UserId == request.Id);
            }
            response.Total = data.Count();
            response.Data = data.ToList();
            return response;
        }



        /// <summary>
        /// 保存授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SaveGrant")]
        [Authorize]
        public ResponseDto<Users> SaveGrant([FromBody] List<UserMenus> request)
        {
            ResponseDto<Users> response = new ResponseDto<Users>();

            var _entity = _appSystemServices.GetEntitys<UserMenus>();
            _entity.Where(x => x.UserId == request.FirstOrDefault().UserId).ToDelete();
            if (request.Count > 0)
            {
                request.ForEach(x => {
                    _appSystemServices.Create<UserMenus>(new UserMenus() { UserId = x.UserId, MenuId = x.MenuId, CompanyId = CurrentUser.CompanyId });
                });
            }
            return response;
        }
    }  
}

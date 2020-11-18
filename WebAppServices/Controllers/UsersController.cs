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
            try
            {
                response.Data = _dataBaseServices.GetColumns(typeof(Users).Name);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetHeader");
            }
            return response;
        }



        [HttpPost("GetUserInfo")]
        public IActionResult GetUserInfo()
        {

            // 获取用请求携带token
            var users = this.CurrentUser;

            // 判断是否是管理员
            var menus = _userServices.GetUserMenus(this.CurrentUser.Id);


            List<MenuTree> router = new List<MenuTree>();
            // 组织menus
            var parent = menus.Where(x => x.ParentMenuID.ToInt32() == 0).ToList();
            parent.ForEach(p => {
                router.Add(GetMenuTree(p,menus)); 
            });

            // 获取用户角色
            var roles = new List<string>() { "admin"};
            // 获取用户菜单
            return Ok(new { code =20000, roles , router = router, name = users.UserName, avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif" });
        }


        private MenuTree GetMenuTree(Menus menu,List<Menus> menus)
        {
            var child = GetChilds(menu.Id, menus);
            return  new MenuTree()
            {
                Id = menu.Id,
                path = menu.MenuPath,
                component = menu.Url,
                alwaysShow = menu.IsAlwaysShow.ToBoolean(), 
                name = menu.MenuName,
                meta = new MenuMeta() { icon = menu.MenuIcon, title = menu.MenuName , noCache  = true},
                children =   child,
                redirect = child.Count == 0 ? null : child.FirstOrDefault().path
            };
        }


        public List<MenuTree> GetChilds(Int32 ParentID, List<Menus> menus)
        {
            List<MenuTree> result = new List<MenuTree>(); 
            menus.Where(x => x.ParentMenuID == ParentID).ToList().ForEach(p=> result.Add(GetMenuTree(p, menus))) ; 
            return   result;
        }


        [HttpPost("GetResult")]
        public ResponseListDto<Users> GetResult([FromBody] BaseRequest<Users> request)
        {
            ResponseListDto<Users> response = new ResponseListDto<Users>();
            try
            {
                var data = _appSystemServices.GetEntitys<Users>();

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

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetResult");
            }
            return response;
        }



        [HttpPost("Save")]
        public ResponseDto<Users> Save([FromBody] Users request)
        {
            ResponseDto<Users> response = new ResponseDto<Users>();
            try
            {
                var _entity = _appSystemServices.GetEntitys<Users>();
                if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
                {
                    _appSystemServices.Create<Users>(request);
                }
                else
                {
                    _appSystemServices.Modify<Users>(request);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Save");
            }
            return response;
        }


        [HttpPost("Remove")]
        public ResponseDto<Boolean> Remove([FromBody] Users request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            try
            {
                if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
                {

                    response.Message = "Key 不能为空";
                    response.Success = false;
                    return response;
                }

                var _entity = _appSystemServices.GetEntitys<Users>();
                response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Remove");
            }
            return response;
        }



        [HttpGet("GetUsersList")]
        public ResponseListDto<UserDto> GetUsersList()
        {
            ResponseListDto<UserDto> response = new ResponseListDto<UserDto>();
            try
            {
                response.Data = _userServices.GetUserList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetUsersList");
            }
            return response;
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


        [HttpPost("Register")]
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
                user.Password = user.Password.ToMD5();
                response.Data = _userServices.RegisterUser(user);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Register");
            }
            return response;
        }
    }
}

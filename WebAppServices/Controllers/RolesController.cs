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
    public class RolesController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public RolesController(IMapper mapper
            , UsersSrevices usersSrevices
            , SystemServices sysservices
            , DataBaseServices dataBaseServices
            , AppSystemServices appSystemServices
            )
        {
            _mapper = mapper;
            _sysservices = sysservices;
            _dataBaseServices = dataBaseServices;
            _appSystemServices = appSystemServices;
        }

        /// <summary>
        /// 获取列头
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHeader")]
        [Authorize]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var user = this.CurrentUser;

            response.Data = _dataBaseServices.GetColumns(typeof(Roles).Name);

            return response;
        }


        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        [Authorize]
        public ResponseListDto<Roles> GetResult([FromBody] BaseRequest<Roles> request)
        {
            ResponseListDto<Roles> response = new ResponseListDto<Roles>();

            var data = _appSystemServices.GetEntitys<Roles>().Where(x => x.CompanyId == CurrentUser.CompanyId);

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.RoleName.Contains(request.Filter));
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
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Roles>();
            return response;
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Save")]
        public ResponseDto<Roles> Save([FromBody] Roles request)
        {
            ResponseDto<Roles> response = new ResponseDto<Roles>();

            var _entity = _appSystemServices.GetEntitys<Roles>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                _appSystemServices.Create<Roles>(request);
            }
            else
            {
                _appSystemServices.Modify<Roles>(request);
            }
            return response;
        }


        [HttpPost("Remove")]
        public ResponseDto<Boolean> Remove([FromBody] Roles request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<Roles>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }


        /// <summary>
        /// 获取角色用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetRoleUser")]
        public ResponseListDto<Users> GetRoleUser([FromBody] BaseRequest<Roles> request)
        {
            ResponseListDto<Users> response = new ResponseListDto<Users>();

            var data = _appSystemServices.GetEntitys<Users>().Where(x => x.CompanyId == CurrentUser.CompanyId);

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.UserName.Contains(request.Filter));
                }

                if (!request.Model.IsNull() && !request.Model.Id.IsNull())
                {
                    var userids = _appSystemServices.GetEntitys<RoleUsers>().Where(x => x.RoleId == request.Model.Id).ToList().Select(p => p.UserId).ToList();
                    if (userids.Count > 0)
                        data = data.Where(x => userids.Contains(x.Id));
                    else
                        data = data.Where("1=2");
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
        /// 获取角色可以选择的用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetRoleChoseUser")]
        public ResponseListDto<Users> GetRoleChoseUser([FromBody] BaseRequest<Roles> request)
        {
            ResponseListDto<Users> response = new ResponseListDto<Users>();

            var data = _appSystemServices.GetEntitys<Users>().Where(x => x.CompanyId == CurrentUser.CompanyId);

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.UserName.Contains(request.Filter));
                }

                if (!request.Model.IsNull() && !request.Model.Id.IsNull())
                {
                    var userids = _appSystemServices.GetEntitys<RoleUsers>().Where(x => x.RoleId == request.Model.Id).ToList().Select(p => p.UserId).ToList();
                    if (userids.Count > 0)
                        data = data.Where(x => !userids.Contains(x.Id));
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
        /// 保存角色用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SaveRoleUser")]
        [Authorize]
        public ResponseListDto<RoleUsers> SaveRoleUser([FromBody] List<RoleUsers> request)
        {
            ResponseListDto<RoleUsers> response = new ResponseListDto<RoleUsers>();
            var _entity = _appSystemServices.GetEntitys<RoleUsers>();
            if (request.Count > 0)
            {
                request.ForEach(p =>
                {
                    if (!_entity.Any(x => x.UserId == p.UserId && x.RoleId == p.RoleId))
                    {
                        p.CompanyId = CurrentUser.CompanyId;
                        _appSystemServices.Create<RoleUsers>(p);
                    }
                });
            }
            return response;
        }


        /// <summary>
        /// 移除角色用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RemoveRoleUser")]
        [Authorize]
        public ResponseListDto<RoleUsers> RemoveRoleUser([FromBody] List<RoleUsers> request)
        {
            ResponseListDto<RoleUsers> response = new ResponseListDto<RoleUsers>();
            var _entity = _appSystemServices.GetEntitys<RoleUsers>();
            if (request.Count > 0)
            {
                request.ForEach(p =>
                {
                    var roleuser = _entity.Where(x => x.UserId == p.UserId && x.RoleId == p.RoleId).First();
                    if (!roleuser.IsNull())
                    {
                        _appSystemServices.Remove<RoleUsers>(roleuser);
                    }
                });

            }
            return response;
        }


        /// <summary>
        /// 获取角色菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetRoleMenus")]
        [Authorize]
        public ResponseListDto<RoleMenus> GetRoleMenus([FromBody] Roles request)
        {
            ResponseListDto<RoleMenus> response = new ResponseListDto<RoleMenus>();
            var data = _appSystemServices.GetEntitys<RoleMenus>();

            if (!request.IsNull())
            {
                data = data.Where(x => x.RoleId == request.Id);
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
        public ResponseDto<Roles> SaveGrant([FromBody] List<RoleMenus> request)
        {
            ResponseDto<Roles> response = new ResponseDto<Roles>();

            var _entity = _appSystemServices.GetEntitys<RoleMenus>();
            _entity.Where(x => x.RoleId == request.FirstOrDefault().RoleId).ToDelete();
            if (request.Count > 0)
            {
                request.ForEach(x => {
                    _appSystemServices.Create<RoleMenus>(new RoleMenus() { RoleId = x.RoleId, MenuId = x.MenuId,CompanyId = CurrentUser.CompanyId });
                });
            }
            return response;
        }
    }
}

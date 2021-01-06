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
    public class DepartmentController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public DepartmentController(IMapper mapper
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

            response.Data = _dataBaseServices.GetColumns(typeof(Department).Name);
            return response;
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        [Authorize]
        public ResponseListDto<Department> GetResult([FromBody] BaseRequest<Department> request)
        {
            ResponseListDto<Department> response = new ResponseListDto<Department>();

            var data = _appSystemServices.GetEntitys<Department>();

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.DepartmentName.Contains(request.Filter));
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
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Department>();

            return response;
        }


        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<Department> Save([FromBody] Department request)
        {
            ResponseDto<Department> response = new ResponseDto<Department>();
            var _entity = _appSystemServices.GetEntitys<Department>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                request.SetCreateDefault(this.CurrentUser);
                _appSystemServices.Create<Department>(request);
            }
            else
            {
                request.SetModifyDefault(this.CurrentUser);
                _appSystemServices.Modify<Department>(request);
            }

            return response;
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] Department request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<Department>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }


        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetTree")]
        public ResponseListDto<Department> GetTree()
        {
            ResponseListDto<Department> response = new ResponseListDto<Department>();

            var data = _appSystemServices.GetEntitys<Department>().Where(x => x.ParentId == 0).ToList();

            data.ForEach(x =>
            {
                GetChildren(x);
            });

            response.Data = data.ToList<Department>();

            return response;
        }

        [HttpPost("GetChildren")]
        private void GetChildren(Department tree)
        {
            tree.children = _appSystemServices.GetEntitys<Department>().Where(o => o.ParentId == tree.Id).ToList<Department>();
            tree.children.ForEach(x =>
            {
                GetChildren(x);
            });
        }


        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetMenus")]
        [Authorize]
        public ResponseListDto<DepartmentMenus> GetMenus([FromBody] Department request)
        {
            ResponseListDto<DepartmentMenus> response = new ResponseListDto<DepartmentMenus>();
            var data = _appSystemServices.GetEntitys<DepartmentMenus>();

            if (!request.IsNull())
            {
                data = data.Where(x => x.DepartmentId == request.Id);
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
        public ResponseDto<Department> SaveGrant([FromBody] List<DepartmentMenus> request)
        {
            ResponseDto<Department> response = new ResponseDto<Department>();

            var _entity = _appSystemServices.GetEntitys<DepartmentMenus>();
            _entity.Where(x => x.DepartmentId == request.FirstOrDefault().DepartmentId).ToDelete().ExecuteAffrows();
            if (request.Count > 0)
            {
                request.ForEach(x => {
                    var menus = new DepartmentMenus() { DepartmentId = x.DepartmentId, MenuId = x.MenuId, CompanyId = CurrentUser.CompanyId };
                    menus.SetCreateDefault(this.CurrentUser);
                    _appSystemServices.Create<DepartmentMenus>(menus);
                });
            }
            return response;
        }



        /// <summary>
        /// 获取部门用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetDepartmentUser")]
        public ResponseListDto<Users> GetDepartmentUser([FromBody] BaseRequest<Department> request)
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
                    var userids = _appSystemServices.GetEntitys<DepartmentUsers>().Where(x => x.DepartmentId == request.Model.Id).ToList().Select(p => p.UserId).ToList();
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
        /// 获取部门可以选择的用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetDepartmentChoseUser")]
        public ResponseListDto<Users> GetDepartmentChoseUser([FromBody] BaseRequest<Department> request)
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
                    var userids = _appSystemServices.GetEntitys<DepartmentUsers>().Where(x => x.DepartmentId == request.Model.Id).ToList().Select(p => p.UserId).ToList();
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
        /// 保存部门用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SaveDepartmentUser")]
        [Authorize]
        public ResponseListDto<DepartmentUsers> SaveDepartmentUser([FromBody] List<DepartmentUsers> request)
        {
            ResponseListDto<DepartmentUsers> response = new ResponseListDto<DepartmentUsers>();
            var _entity = _appSystemServices.GetEntitys<DepartmentUsers>();
            if (request.Count > 0)
            {
                request.ForEach(p =>
                {
                    if (!_entity.Any(x => x.UserId == p.UserId && x.DepartmentId == p.DepartmentId))
                    {
                        p.CompanyId = CurrentUser.CompanyId;
                        p.SetCreateDefault(this.CurrentUser);
                        _appSystemServices.Create<DepartmentUsers>(p);
                    }
                });
            }
            return response;
        }


        /// <summary>
        /// 移除部门用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RemoveDepartmentUser")]
        [Authorize]
        public ResponseListDto<DepartmentUsers> RemoveDepartmentUser([FromBody] List<DepartmentUsers> request)
        {
            ResponseListDto<DepartmentUsers> response = new ResponseListDto<DepartmentUsers>();
            var _entity = _appSystemServices.GetEntitys<DepartmentUsers>();
            if (request.Count > 0)
            {
                request.ForEach(p =>
                {
                    var departmentuser = _entity.Where(x => x.UserId == p.UserId && x.DepartmentId == p.DepartmentId).First();
                    if (!departmentuser.IsNull())
                    {
                        _appSystemServices.Remove<DepartmentUsers>(departmentuser);
                    }
                });

            }
            return response;
        }

    }
}

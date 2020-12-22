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
    public class MenusController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public MenusController(IMapper mapper
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
        /// 获取菜单头部
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetHeader")]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var user = this.CurrentUser;

            response.Data = _dataBaseServices.GetColumns(typeof(Menus).Name);

            return response;
        }


        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetResult")]
        public ResponseListDto<MenuDto> GetResult([FromBody] BaseRequest<MenuDto> request)
        {
            ResponseListDto<MenuDto> response = new ResponseListDto<MenuDto>();

            var data = _appSystemServices.GetEntitys<Menus>();

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.MenuName.Contains(request.Filter));
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

            var alldata = data.ToList<MenuDto>();
            // 组织menus
            var parent = alldata.Where(x => x.ParentId.ToInt64() == 0).ToList();
            parent.ForEach(p =>
            {
                p.children = GetChilds(p, alldata);
            });

            response.Total = data.Count();
            response.Data = parent;// data.Page(request.PageIndex, request.PageSize).ToList<Menus>();

            return response;
        }


        private List<MenuDto> GetChilds(MenuDto menu, List<MenuDto> menus)
        {
            var childs = menus.Where(x => x.ParentId == menu.Id).ToList();
            childs.ForEach(x =>
            {
                x.children = GetChilds(x, menus);
            });

            return childs.ToList();
        }


        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<Menus> Save([FromBody] Menus request)
        {
            ResponseDto<Menus> response = new ResponseDto<Menus>();

            var _entity = _appSystemServices.GetEntitys<Menus>();
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                _appSystemServices.Create<Menus>(request);
            }
            else
            {
                _appSystemServices.Modify<Menus>(request);
            }

            return response;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] Menus request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<Menus>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetTree")]
        public ResponseListDto<Menus> GetTree()
        {
            ResponseListDto<Menus> response = new ResponseListDto<Menus>();

            var data = _appSystemServices.GetEntitys<Menus>().Where(x => x.ParentId == 0).ToList();

            data.ForEach(x =>
            {
                GetChildren(x);
            });

            response.Data = data.ToList<Menus>();

            return response;
        }

        [HttpPost("GetChildren")]
        private void GetChildren(Menus tree)
        {
            tree.children = _appSystemServices.GetEntitys<Menus>().Where(o => o.ParentId == tree.Id).ToList<Menus>();
            tree.children.ForEach(x =>
            {
                GetChildren(x);
            });
        }
    }
}


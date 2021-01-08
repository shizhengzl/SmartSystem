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
    public class TableAreaController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public TableAreaController(IMapper mapper
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
        /// 获取请求头
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHeader")]
        [Authorize]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var user = this.CurrentUser;

            response.Data = _dataBaseServices.GetColumns(typeof(TableArea).Name);

            return response;
        }

        private List<TableArea> GetChilds(TableArea model, List<TableArea> models)
        {
            var childs = models.Where(x => x.ParentId == model.Id).ToList();
            childs.ForEach(x =>
            {
                x.children = GetChilds(x, models);
            });

            return childs.ToList();
        }





        /// <summary>
        /// 获取表分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        [Authorize]
        public ResponseListDto<TableArea> GetResult([FromBody] BaseRequest<TableArea> request)
        {
            ResponseListDto<TableArea> response = new ResponseListDto<TableArea>();

            var data = _appSystemServices.GetEntitys<TableArea>();
            data = data.Where(x => x.CompanyId == CurrentUser.CompanyId);
            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.AreaName.Contains(request.Filter));
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

            var alldata = data.ToList<TableArea>();
            // 组织menus
            var parent = alldata.Where(x => x.ParentId.ToInt64() == 0).ToList();
            parent.ForEach(p =>
            {
                p.children = GetChilds(p, alldata);
            });

            response.Total = data.Count();
            response.Data = parent;// data.Page(request.PageIndex, request.PageSize).ToList<Menus>();

            //response.Total = data.Count();
            //response.Data = data.Page(request.PageIndex, request.PageSize).ToList<TableArea>();
            return response;
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<TableArea> Save([FromBody] TableArea request)
        {
            ResponseDto<TableArea> response = new ResponseDto<TableArea>();
            var _entity = _appSystemServices.GetEntitys<TableArea>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                request.SetCreateDefault(this.CurrentUser);
                _appSystemServices.Create<TableArea>(request);
            }
            else
            {
                request.SetModifyDefault(this.CurrentUser);
                _appSystemServices.Modify<TableArea>(request);
            }
            return response;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] TableArea request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<TableArea>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }
    }
}

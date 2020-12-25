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
    public class CompanyController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public CompanyController(IMapper mapper
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
        /// 获取单位列头
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHeader")]
        [Authorize]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();
            response.Data = _dataBaseServices.GetColumns(typeof(Company).Name);

            return response;
        }


        /// <summary>
        /// 获取单位详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        [Authorize]
        public ResponseListDto<Company> GetResult([FromBody] BaseRequest<Company> request)
        {
            ResponseListDto<Company> response = new ResponseListDto<Company>();
            var data = _appSystemServices.GetEntitys<Company>();

            if (!request.IsNull())
            {

                if (request.Model.IsMy)
                {
                    data = data.Where(x => x.Id == CurrentUser.CompanyId);
                }

                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.CompanyName.Contains(request.Filter));
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
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Company>(); 
            return response;
        }


        /// <summary>
        /// 保存单位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<Company> Save([FromBody] Company request)
        {
            ResponseDto<Company> response = new ResponseDto<Company>();
            var _entity = _appSystemServices.GetEntitys<Company>();
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                _appSystemServices.Create<Company>(request);
            }
            else
            {
                _appSystemServices.Modify<Company>(request);
            }
            return response;
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] Company request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {
                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<Company>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }

        /// <summary>
        /// 保存授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SaveGrant")]
        [Authorize]
        public ResponseDto<Company> SaveGrant([FromBody] List<CompanyMenus> request)
        {
            ResponseDto<Company> response = new ResponseDto<Company>();

            var _entity = _appSystemServices.GetEntitys<CompanyMenus>();
            _entity.Where(x => x.CompanyId == request.FirstOrDefault().CompanyId).ToDelete();
            if (request.Count > 0)
            { 
                request.ForEach(x=> {
                    _appSystemServices.Create<CompanyMenus>(new CompanyMenus() { CompanyId = x.CompanyId, MenuId = x.MenuId });
                });
            } 
            return response;
        }




        /// <summary>
        /// 获取单位菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetCompanyMenus")]
        [Authorize]
        public ResponseListDto<CompanyMenus> GetCompanyMenus([FromBody] Company request)
        {
            ResponseListDto<CompanyMenus> response = new ResponseListDto<CompanyMenus>();
            var data = _appSystemServices.GetEntitys<CompanyMenus>();

            if (!request.IsNull())
            {
                data = data.Where(x => x.CompanyId == request.Id);
            }
            response.Total = data.Count();
            response.Data = data.ToList();
            return response;
        }
    }
}

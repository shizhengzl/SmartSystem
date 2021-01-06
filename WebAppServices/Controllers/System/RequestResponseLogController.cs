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

    public class RequestResponseLogController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public RequestResponseLogController(IMapper mapper
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
        /// 获取请求日志头
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHeader")]
        [Authorize]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var user = this.CurrentUser;

            response.Data = _dataBaseServices.GetColumns(typeof(RequestResponseLog).Name);

            return response;
        }


        /// <summary>
        /// 获取请求日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        [Authorize]
        public ResponseListDto<RequestResponseLog> GetResult([FromBody] BaseRequest<RequestResponseLog> request)
        {
            ResponseListDto<RequestResponseLog> response = new ResponseListDto<RequestResponseLog>();

            var data = _appSystemServices.GetEntitys<RequestResponseLog>();

            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.Url.Contains(request.Filter) 
                    || x.UserName.Contains(request.Filter)
                    //|| x.RequestBody.Contains(request.Filter) || x.ResponseBody.Contains(request.Filter)
                    );
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
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<RequestResponseLog>();

            return response;
        }


        /// <summary>
        /// 保存请求日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<RequestResponseLog> Save([FromBody] RequestResponseLog request)
        {
            ResponseDto<RequestResponseLog> response = new ResponseDto<RequestResponseLog>();

            var _entity = _appSystemServices.GetEntitys<RequestResponseLog>();
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                request.SetCreateDefault(this.CurrentUser);
                _appSystemServices.Create<RequestResponseLog>(request);
            }
            else
            {
                request.SetModifyDefault(this.CurrentUser);
                _appSystemServices.Modify<RequestResponseLog>(request);
            }

            return response;
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] RequestResponseLog request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<RequestResponseLog>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }
    }
}

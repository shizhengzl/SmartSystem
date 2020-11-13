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
    public class IntellisenceController : ControllerBase
    {
        private IMapper _mapper { get; set; } 
        private UsersSrevices _userServices { get; set; } 
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; } 
        private SystemServices _sysservices { get; set; }
        public IntellisenceController(IMapper mapper
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

      
        [HttpPost("GetHeader")]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();
            try
            { 
                response.Data = _dataBaseServices.GetColumns(typeof(Intellisence).Name); 
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetHeader");
            }
            return response;
        }



        [HttpPost("GetResult")]
        public ResponseListDto<Intellisence> GetResult([FromBody] BaseRequest<Intellisence> request)    
        {
            ResponseListDto<Intellisence> response = new ResponseListDto<Intellisence>();
            try
            { 
                var data = _appSystemServices.GetEntitys<Intellisence>();

                if (!request.IsNull())
                { 
                    if (!string.IsNullOrEmpty(request.Filter.ToStringExtension())) {
                        data = data.Where(x => x.StartChar.Contains(request.Filter));
                    }
                } 
                response.Total = data.Count();
                response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Intellisence>();

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
        public ResponseDto<Intellisence> Save([FromBody] Intellisence request)
        {
            ResponseDto<Intellisence> response = new ResponseDto<Intellisence>();
            try
            {
                var _entity = _appSystemServices.GetEntitys<Intellisence>();
                if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
                {
                    _appSystemServices.Create<Intellisence>(request);
                }
                else
                {
                    _appSystemServices.Modify<Intellisence>(request);
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
        public ResponseDto<Boolean> Remove([FromBody] Intellisence request)
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

                var _entity = _appSystemServices.GetEntitys<Intellisence>();
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
    }
}

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
                var data = _appSystemServices.GetData<Intellisence>();
                if (request.Search != null &&  !string.IsNullOrEmpty( request.Search.InsertionText))
                    data.Where(x => x.InsertionText.Contains(request.Search.InsertionText));

                response.Total = data.Count();
                response.Data = data.Page((request.PageIndex - 1) * request.PageSize + 1, request.PageSize).ToList<Intellisence>();

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetResult");
            }
            return response;
        }

    }
}

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
using Core.Repository.APPSystem;
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
    public class TableAreaDataController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public TableAreaDataController(IMapper mapper
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
                var user = this.CurrentUser;

                response.Data = _dataBaseServices.GetColumns(typeof(TableAreaData).Name);
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
        public ResponseListDto<TableAreaData> GetResult([FromBody] BaseRequest<TableAreaData> request)
        {
            ResponseListDto<TableAreaData> response = new ResponseListDto<TableAreaData>();
            try
            {
                var data = _appSystemServices.GetEntitys<TableAreaData>();

                if (!request.IsNull())
                {
                    if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                    {
                        data = data.Where(x => x.TableName.Contains(request.Filter));
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
                response.Data = data.Page(request.PageIndex, request.PageSize).ToList<TableAreaData>();

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
        public ResponseDto<TableAreaData> Save([FromBody] TableAreaData request)
        {
            ResponseDto<TableAreaData> response = new ResponseDto<TableAreaData>();
            try
            {
                var _entity = _appSystemServices.GetEntitys<TableAreaData>();
                if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
                {
                    _appSystemServices.Create<TableAreaData>(request);
                }
                else
                {
                    _appSystemServices.Modify<TableAreaData>(request);
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



        [HttpPost("SaveList")]
        public ResponseListDto<TableAreaData> SaveList([FromBody] List<TableAreaData> request)
        {
            ResponseListDto<TableAreaData> response = new ResponseListDto<TableAreaData>();
            try
            {
                var _entity = _appSystemServices.GetEntitys<TableAreaData>();

                if (request.Count > 0)
                {
                    _entity.Where(x => x.TableAreaId == request.FirstOrDefault().TableAreaId).ToDelete().ExecuteAffrows();

                    request.ForEach(p =>
                    {
                        _appSystemServices.Create<TableAreaData>(p);
                    });
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "SaveList");
            }
            return response;
        }



        [HttpPost("Remove")]
        public ResponseDto<Boolean> Remove([FromBody] TableAreaData request)
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

                var _entity = _appSystemServices.GetEntitys<TableAreaData>();
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

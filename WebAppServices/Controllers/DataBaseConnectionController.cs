using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Repository;
using Core.Services;
using Core.Services.AppSystem;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Mvc;
using WebAppServices.Model;

namespace WebAppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseConnectionController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public DataBaseConnectionController(IMapper mapper
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

                response.Data = _dataBaseServices.GetColumns(typeof(DataBaseConnection).Name);
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
        public ResponseListDto<DataBaseConnection> GetResult([FromBody] BaseRequest<DataBaseConnection> request)
        {
            ResponseListDto<DataBaseConnection> response = new ResponseListDto<DataBaseConnection>();
            try
            {
                var data = _appSystemServices.GetEntitys<DataBaseConnection>();

                if (!request.IsNull())
                {
                    if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                    {
                        data = data.Where(x => x.DataBaseName.Contains(request.Filter));
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
                response.Data = data.Page(request.PageIndex, request.PageSize).ToList<DataBaseConnection>();

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
        public ResponseDto<DataBaseConnection> Save([FromBody] DataBaseConnection request)
        {
            ResponseDto<DataBaseConnection> response = new ResponseDto<DataBaseConnection>();
            try
            {
                var _entity = _appSystemServices.GetEntitys<DataBaseConnection>();
                if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
                {
                    _appSystemServices.Create<DataBaseConnection>(request);
                }
                else
                {
                    _appSystemServices.Modify<DataBaseConnection>(request);
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
        public ResponseDto<Boolean> Remove([FromBody] DataBaseConnection request)
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

                var _entity = _appSystemServices.GetEntitys<DataBaseConnection>();
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
 
         


        [HttpGet("GetDataBaseList")]
        public ResponseListDto<TreeDto> GetDataBaseConnectionList()
        {
            ResponseListDto<TreeDto> response = new ResponseListDto<TreeDto>();
            try
            { 
                var data = _dataBaseServices.GetDataBaseConnections();
                List<TreeDto> responsedto = new List<TreeDto>();
                data.ForEach(x=> {
                    responsedto.Add(new TreeDto() { id = x.Id,label = x.DataBaseName,parentId = 0 });
                });
                response.Data = responsedto.ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                _sysservices.AddExexptionLogs(ex, "GetDataBaseConnectionList");
            } 
            return response;
        }


        [HttpGet("GetDataBaseTableList/{id}")]
        public ResponseListDto<TreeDto> GetDataBaseTableList(Int64 Id)
        {
            ResponseListDto<TreeDto> response = new ResponseListDto<TreeDto>();
            try
            {
                var baseconnection = _dataBaseServices.GetConnectionString(Id);
                var data = _dataBaseServices.GetTables(baseconnection);
                List<TreeDto> responsedto = new List<TreeDto>();
                var i = 1;
                data.ForEach(x => {
                    responsedto.Add(new TreeDto() { id = i, label = $"{ x.TableName}" ,parentId=Id,description = x.TableDescription});
                    i++;
                });
                response.Data = responsedto.ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                _sysservices.AddExexptionLogs(ex, "GetDataBaseTableList");
            }
            return response;
        }


        [HttpGet("GetTableColumnList/{TableName}")]
        public ResponseListDto<Column> GetTableColumnList(string TableName)
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();
            try
            {
                var Id = TableName.Split(',')[0].ToInt64();
                var tablename = TableName.Split(',')[1].ToStringExtension();
                var baseconnection = _dataBaseServices.GetConnectionString(Id);
                var data = _dataBaseServices.GetColumns(baseconnection, tablename);
                data.ForEach(x => { x.TableName = tablename; x.Id = Id; });
                response.Data = data.ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                _sysservices.AddExexptionLogs(ex, "ResponseListDto");
            }
            return response;
        }




        [HttpGet("SetExtendedproperty/{TableName}")]
        public ResponseDto<Boolean> SetExtendedproperty(string TableName)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();
            try
            {
                var Id = TableName.Split(',')[0].ToInt64();
                var table = TableName.Split(',')[1].ToStringExtension();
                var column = TableName.Split(',')[2].ToStringExtension();
                var des = TableName.Split(',')[3].ToStringExtension();

                var baseconnection = _dataBaseServices.GetConnectionString(Id);


                var isnull = _dataBaseServices.GetColumns(baseconnection, table).Where(x => x.ColumnName.ToUpper() == column.ToUpper()).FirstOrDefault().ColumnDescription.IsNull();
                if(isnull)
                    response.Data = _dataBaseServices.AddExtendedproperty(baseconnection, table,column,des);
                else
                    response.Data = _dataBaseServices.ModifyExtendedproperty(baseconnection, table, column, des);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                _sysservices.AddExexptionLogs(ex, "ResponseListDto");
            }
            return response;
        }


        [HttpGet("SetTableExtendedproperty/{TableName}")]
        public ResponseDto<Boolean> SetTableExtendedproperty(string TableName)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();
            try
            {
                var Id = TableName.Split(',')[0].ToInt64();
                var table = TableName.Split(',')[1].ToStringExtension();
                var des = TableName.Split(',')[2].ToStringExtension();

                var baseconnection = _dataBaseServices.GetConnectionString(Id);


                var isnull = string.IsNullOrEmpty(_dataBaseServices.GetTables(baseconnection).Where(x => x.TableName.ToUpper() == table.ToUpper()).FirstOrDefault().TableDescription.ToStringExtension());
                if (isnull)
                    response.Data = _dataBaseServices.AddTableExtendedproperty(baseconnection, table, des);
                else
                    response.Data = _dataBaseServices.ModifyTableExtendedproperty(baseconnection, table, des);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                _sysservices.AddExexptionLogs(ex, "ResponseListDto");
            }
            return response;

        }
        }
    }
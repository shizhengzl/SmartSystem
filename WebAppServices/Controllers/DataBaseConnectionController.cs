using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repository;
using Core.Services;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Mvc;
using WebAppServices.Model;

namespace WebAppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseConnectionController : ControllerBase
    {
        DataBaseServices services = new DataBaseServices();
        SystemServices sysservices = new SystemServices();


        [HttpGet("GetDataBaseList")]
        public ResponseListDto<TreeDto> GetDataBaseConnectionList()
        {
            ResponseListDto<TreeDto> response = new ResponseListDto<TreeDto>();
            try
            { 
                var data = services.GetDataBaseConnections();
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

                sysservices.AddExexptionLogs(ex, "GetDataBaseConnectionList");
            } 
            return response;
        }


        [HttpGet("GetDataBaseTableList/{id}")]
        public ResponseListDto<TreeDto> GetDataBaseTableList(Int64 Id)
        {
            ResponseListDto<TreeDto> response = new ResponseListDto<TreeDto>();
            try
            {
                var baseconnection = services.GetConnectionString(Id);
                var data = services.GetTables(baseconnection);
                List<TreeDto> responsedto = new List<TreeDto>();
                data.ForEach(x => {
                    responsedto.Add(new TreeDto() { id = x.Id, label = $"{ x.TableName}" ,parentId=Id,description = x.TableDescription});
                });
                response.Data = responsedto.ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                sysservices.AddExexptionLogs(ex, "GetDataBaseTableList");
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
                var baseconnection = services.GetConnectionString(Id);
                var data = services.GetColumns(baseconnection, tablename);
                data.ForEach(x => { x.TableName = tablename; x.Id = Id; });
                response.Data = data.ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                sysservices.AddExexptionLogs(ex, "ResponseListDto");
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

                var baseconnection = services.GetConnectionString(Id);
                var data = services.SetExtendedproperty(baseconnection, table,column,des);
               
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                sysservices.AddExexptionLogs(ex, "ResponseListDto");
            }
            return response;
        }

    }
}
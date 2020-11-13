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


                var isnull = services.GetColumns(baseconnection, table).Where(x => x.ColumnName.ToUpper() == column.ToUpper()).FirstOrDefault().ColumnDescription.IsNull();
                if(isnull)
                    response.Data = services.AddExtendedproperty(baseconnection, table,column,des);
                else
                    response.Data = services.ModifyExtendedproperty(baseconnection, table, column, des);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;

                sysservices.AddExexptionLogs(ex, "ResponseListDto");
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

                var baseconnection = services.GetConnectionString(Id);


                var isnull = string.IsNullOrEmpty(services.GetTables(baseconnection).Where(x => x.TableName.ToUpper() == table.ToUpper()).FirstOrDefault().TableDescription.ToStringExtension());
                if (isnull)
                    response.Data = services.AddTableExtendedproperty(baseconnection, table, des);
                else
                    response.Data = services.ModifyTableExtendedproperty(baseconnection, table, des);

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
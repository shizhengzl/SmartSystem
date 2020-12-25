using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Repository;
using Core.Services;
using Core.Services.AppSystem;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
             , IHostingEnvironment hostingEnvironment)

        {
            _mapper = mapper;
            _sysservices = sysservices;
            _dataBaseServices = dataBaseServices;
            _appSystemServices = appSystemServices;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取连接字符串列头
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetHeader")]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var user = this.CurrentUser;

            response.Data = _dataBaseServices.GetColumns(typeof(DataBaseConnection).Name);

            return response;
        }

        /// <summary>
        /// 获取连接字符串结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetResult")]
        public ResponseListDto<DataBaseConnection> GetResult([FromBody] BaseRequest<DataBaseConnection> request)
        {
            ResponseListDto<DataBaseConnection> response = new ResponseListDto<DataBaseConnection>();

            var data = _appSystemServices.GetEntitys<DataBaseConnection>();
            data = data.Where(x => x.CompanyId == CurrentUser.CompanyId);
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


            return response;
        }


        /// <summary>
        /// 保存连接字符串
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<DataBaseConnection> Save([FromBody] DataBaseConnection request)
        {
            ResponseDto<DataBaseConnection> response = new ResponseDto<DataBaseConnection>();

            var _entity = _appSystemServices.GetEntitys<DataBaseConnection>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                _appSystemServices.Create<DataBaseConnection>(request);
            }
            else
            {
                _appSystemServices.Modify<DataBaseConnection>(request);
            }

            return response;
        }

        /// <summary>
        /// 删除连接字符串
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] DataBaseConnection request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            }

            var _entity = _appSystemServices.GetEntitys<DataBaseConnection>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        }



        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDataBaseList")]
        [Authorize]
        public ResponseListDto<TreeDto> GetDataBaseConnectionList()
        {
            ResponseListDto<TreeDto> response = new ResponseListDto<TreeDto>();

            var data = _dataBaseServices.GetDataBaseConnections();
            data = data.Where(x => x.CompanyId == CurrentUser.CompanyId).ToList();
            List<TreeDto> responsedto = new List<TreeDto>();
            data.ForEach(x =>
            {
                responsedto.Add(new TreeDto() { id = x.Id, label = x.DataBaseName, parentId = 0 });
            });
            response.Data = responsedto.ToList();

            return response;
        }

        /// <summary>
        /// 获取数据库表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetDataBaseTableList/{id}")]
        [Authorize]
        public ResponseListDto<TreeDto> GetDataBaseTableList(Int64 Id)
        {
            ResponseListDto<TreeDto> response = new ResponseListDto<TreeDto>();

            var baseconnection = _dataBaseServices.GetConnectionString(Id);
            var data = _dataBaseServices.GetTables(baseconnection);
            List<TreeDto> responsedto = new List<TreeDto>();
            var i = 1;
            data.ForEach(x =>
            {
                responsedto.Add(new TreeDto() { id = i, label = $"{ x.TableName}", parentId = Id, description = x.TableDescription });
                i++;
            });
            response.Data = responsedto.ToList();

            return response;
        }


        /// <summary>
        /// 获取表列
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpGet("GetTableColumnList/{TableName}")]
        [Authorize]
        public ResponseListDto<Column> GetTableColumnList(string TableName)
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var Id = TableName.Split(',')[0].ToInt64();
            var tablename = TableName.Split(',')[1].ToStringExtension();
            var baseconnection = _dataBaseServices.GetConnectionString(Id);
            var data = _dataBaseServices.GetColumns(baseconnection, tablename);
            data.ForEach(x => { x.TableName = tablename; x.Id = Id; });
            response.Data = data.ToList();

            return response;
        }


        /// <summary>
        /// 导出表列
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpGet("ExportTableColumnList/{TableName}")]
        [Authorize]
        public ResponseDto<string> ExportTableColumnList(string TableName)
        {
            ResponseDto<string> response = new ResponseDto<string>();

            var Id = TableName.Split(',')[0].ToInt64();
            var tablename = TableName.Split(',')[1].ToStringExtension();
            var baseconnection = _dataBaseServices.GetConnectionString(Id);
            var data = _dataBaseServices.GetColumns(baseconnection, tablename);
            data.ForEach(x => { x.TableName = tablename; x.Id = Id; });
            var result = data.ToList().Select(x => new
            {
                x.TableName,
                x.ColumnName
                ,
                x.ColumnDescription,
                IsIdentity = (x.IsIdentity ? "是" : "否"),
                IsPrimarykey = (x.IsPrimarykey ? "是" : "否")
                ,
                x.MaxLength,
                IsRequire = (x.IsRequire ? "是" : "否"),
                x.Scale,
                x.DefaultValue,
                x.SQLType
            });


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(tablename);
                worksheet.Cells.LoadFromCollection(result, true);

                for (int i = 0; i < worksheet.Dimension.End.Column; i++)
                {
                    var name = worksheet.Cells[1, i + 1].Value;
                    var field = typeof(Column).GetProperties().Where(x => x.Name.ToUpper() == name.ToStringExtension().ToUpper()).FirstOrDefault();
                    if (!field.IsNull())
                    {
                        var attribute = Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;

                        worksheet.Cells[1, i + 1].Value = attribute.Description.ToStringExtension();
                        worksheet.Column(i+1).Width = 18;//设置列宽
                        worksheet.Column(i + 1).Style.WrapText = true;
                    }

                }
                using (var cell = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    //设置样式:首行居中加粗背景色
                    cell.Style.Font.Bold = true; //加粗
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //水平居中
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;     //垂直居中
                    cell.Style.Font.Size = 14;
                    
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;  //背景颜色
                    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));//设置单元格背景色

                     
                }

              
           



                var virpath = "tempfile";
                var virdir = $"{ _hostingEnvironment.WebRootPath}\\{virpath}";

                var returnpath = $"\\{virpath}\\{tablename}.xlsx";
                if (!Directory.Exists(virdir))
                {
                    Directory.CreateDirectory(virdir);
                }

                var filepath = $"{ _hostingEnvironment.WebRootPath}{returnpath}";
                var stream = new FileStream(filepath, FileMode.Create);
                package.SaveAs(stream);
                stream.Flush();
                stream.Close();

                response.Data = returnpath;
            }
            return response;
        }

        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 设置列属性
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpGet("SetExtendedproperty/{TableName}")]
        [Authorize]
        public ResponseDto<Boolean> SetExtendedproperty(string TableName)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            var Id = TableName.Split(',')[0].ToInt64();
            var table = TableName.Split(',')[1].ToStringExtension();
            var column = TableName.Split(',')[2].ToStringExtension();
            var des = TableName.Split(',')[3].ToStringExtension();

            var baseconnection = _dataBaseServices.GetConnectionString(Id);


            var isnull = _dataBaseServices.GetColumns(baseconnection, table).Where(x => x.ColumnName.ToUpper() == column.ToUpper()).FirstOrDefault().ColumnDescription.IsNull();
            if (isnull)
                response.Data = _dataBaseServices.AddExtendedproperty(baseconnection, table, column, des);
            else
                response.Data = _dataBaseServices.ModifyExtendedproperty(baseconnection, table, column, des);


            return response;
        }

        /// <summary>
        /// 设置表属性
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpGet("SetTableExtendedproperty/{TableName}")]
        [Authorize]
        public ResponseDto<Boolean> SetTableExtendedproperty(string TableName)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>();

            var Id = TableName.Split(',')[0].ToInt64();
            var table = TableName.Split(',')[1].ToStringExtension();
            var des = TableName.Split(',')[2].ToStringExtension();

            var baseconnection = _dataBaseServices.GetConnectionString(Id);

            var isnull = string.IsNullOrEmpty(_dataBaseServices.GetTables(baseconnection).Where(x => x.TableName.ToUpper() == table.ToUpper()).FirstOrDefault().TableDescription.ToStringExtension());
            if (isnull)
                response.Data = _dataBaseServices.AddTableExtendedproperty(baseconnection, table, des);
            else
                response.Data = _dataBaseServices.ModifyTableExtendedproperty(baseconnection, table, des);
            return response;

        }
    }
}
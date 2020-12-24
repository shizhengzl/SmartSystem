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
    public class TestModuleController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public TestModuleController(IMapper mapper
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
        /// 获取列头
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHeader")]
        [Authorize]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();

            var user = this.CurrentUser;

            response.Data = _dataBaseServices.GetColumns(typeof(TestModule).Name);

            return response;
        }


        /// <summary>
        /// 获取yml文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetYml")]
        [Authorize]
        public ResponseDto<String> GetYml([FromBody] BaseRequest<TestModule> request)
        {
            ResponseDto<String> response = new ResponseDto<string>();

            var data = _appSystemServices.GetEntitys<TestModule>().Where(x => x.ParentId == request.Model.Id || request.Model.Id.ToInt64() == 0).ToList();

            data.ForEach(x =>
            {
                GetChildren(x);
            });
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("pages:");
            sb.AppendLine();
            data.ForEach(x =>
            {

                if (x.Url.ToStringExtension().Length > 0)
                {
                    sb.AppendFormat("   - page:");
                    sb.AppendLine();
                    sb.AppendFormat("      pageName: {0}", x.ModuleName);
                    sb.AppendLine();
                    sb.AppendFormat("      value: \"{0}\"", x.Url);
                    sb.AppendLine();
                    sb.AppendFormat("      desc: \"{0}\"", x.Note);
                    sb.AppendLine();
                    sb.AppendFormat("      locators:");
                    sb.AppendLine();

                    var elements = _appSystemServices.GetEntitys<Element>().Where(p => p.ParentId == x.Id);
                    elements.ToList().ForEach(o =>
                    {
                        sb.AppendFormat("         - {{type: \"{0}\",timeout: \"{1}\",value: \"{2}\",desc: \"{3}\",name: \"{4}\"}}", o.Type, o.Timeout, o.Value, o.Desc, o.Name);
                        sb.AppendLine();
                    });
                }

            });

            response.Data = sb.ToString();

            return response;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("GetResult")]
        public ResponseListDto<TestModule> GetResult([FromBody] BaseRequest<TestModule> request)
        {
            ResponseListDto<TestModule> response = new ResponseListDto<TestModule>();

            var data = _appSystemServices.GetEntitys<TestModule>();
            data = data.Where(x => x.CompanyId == CurrentUser.CompanyId);
            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.ModuleName.Contains(request.Filter));
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
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<TestModule>();

            return response;
        }


        /// <summary>
        /// 保存测试模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<TestModule> Save([FromBody] TestModule request)
        {
            ResponseDto<TestModule> response = new ResponseDto<TestModule>();

            var _entity = _appSystemServices.GetEntitys<TestModule>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                _appSystemServices.Create<TestModule>(request);
            }
            else
            {
                _appSystemServices.Modify<TestModule>(request);
            }

            return response;
        }

        /// <summary>
        /// 删除测试模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] TestModule request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>(); 
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            } 
            var _entity = _appSystemServices.GetEntitys<TestModule>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0;

            return response;
        } 

        /// <summary>
        /// 获取树形测试模块
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTree")]
        [Authorize]
        public ResponseListDto<TestModule> GetTree()
        {
            ResponseListDto<TestModule> response = new ResponseListDto<TestModule>();
            try
            {
                var data = _appSystemServices.GetEntitys<TestModule>().Where(x => x.ParentId == 0 && x.CompanyId == CurrentUser.CompanyId).ToList();

                data.ForEach(x =>
                {
                    GetChildren(x);
                });

                response.Data = data.ToList<TestModule>();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "GetHeader");
            }
            return response;
        }

        [HttpPost("GetChildren")]
        private void GetChildren(TestModule tree)
        {
            tree.children = _appSystemServices.GetEntitys<TestModule>().Where(o => o.ParentId == tree.Id).ToList<TestModule>();
            tree.children.ForEach(x =>
            {
                GetChildren(x);
            });
        }
    }
}

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
    public class ElementController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public ElementController(IMapper mapper
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
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>();
            try
            {
                var user = this.CurrentUser;

                response.Data = _dataBaseServices.GetColumns(typeof(Element).Name);
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
        private void GetChildren(TestModule tree, List<Int64> list)
        {
            tree.children = _appSystemServices.GetEntitys<TestModule>().Where(o => o.ParentId == tree.Id).ToList<TestModule>();
            list.AddRange(tree.children.Select(o => o.Id));
            tree.children.ForEach(x =>
            {
                GetChildren(x, list);
            });
        }


        [HttpPost("GetResult")]
        public ResponseListDto<Element> GetResult([FromBody] BaseRequest<Element> request)
        {
            ResponseListDto<Element> response = new ResponseListDto<Element>();
            try
            {
                var data = _appSystemServices.GetEntitys<Element>();

                if (!request.IsNull())
                {
                    if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                    {
                        data = data.Where(x => x.Name.Contains(request.Filter));
                    }

                    if (request.Model.ParentId.ToInt32() > 0)
                    {
                        List<Int64> rlist = new List<long>();
                        var testmodule = _appSystemServices.GetEntitys<TestModule>().Where(p => p.Id == request.Model.ParentId).ToList().FirstOrDefault();

                        rlist.Add(testmodule.Id);
                        GetChildren(testmodule, rlist);
                        data = data.Where(x => rlist.Contains(x.ParentId));

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
                response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Element>();

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
        public ResponseDto<Element> Save([FromBody] Element request)
        {
            ResponseDto<Element> response = new ResponseDto<Element>();
            try
            {
                var _entity = _appSystemServices.GetEntitys<Element>();
                if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
                {
                    _appSystemServices.Create<Element>(request);
                }
                else
                {
                    _appSystemServices.Modify<Element>(request);
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
        public ResponseDto<Boolean> Remove([FromBody] Element request)
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

                var _entity = _appSystemServices.GetEntitys<Element>();
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

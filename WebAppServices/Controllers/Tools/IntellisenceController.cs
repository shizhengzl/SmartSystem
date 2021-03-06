﻿using System;
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
    public class IntellisenceController : BaseController
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

        /// <summary>
        /// 获取代码片段列头
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHeader")]
        [Authorize]
        public ResponseListDto<Column> GetHeader()
        {
            ResponseListDto<Column> response = new ResponseListDto<Column>(); 
            response.Data = _dataBaseServices.GetColumns(typeof(Intellisence).Name); 
            return response;
        }


        /// <summary>
        /// 获取代码片段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetResult")]
        [Authorize]
        public ResponseListDto<Intellisence> GetResult([FromBody] BaseRequest<Intellisence> request)
        {
            ResponseListDto<Intellisence> response = new ResponseListDto<Intellisence>(); 
            var data = _appSystemServices.GetEntitys<Intellisence>();
            data = data.Where(x => x.CompanyId == CurrentUser.CompanyId);
            if (!request.IsNull())
            {
                if (!string.IsNullOrEmpty(request.Filter.ToStringExtension()))
                {
                    data = data.Where(x => x.StartChar.Contains(request.Filter));
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
            response.Data = data.Page(request.PageIndex, request.PageSize).ToList<Intellisence>(); 
            return response;
        }


        /// <summary>
        /// 保存代码片段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize]
        public ResponseDto<Intellisence> Save([FromBody] Intellisence request)
        {
            ResponseDto<Intellisence> response = new ResponseDto<Intellisence>();
            var _entity = _appSystemServices.GetEntitys<Intellisence>();
            request.CompanyId = CurrentUser.CompanyId;
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()) || request.Id.ToInt32() == 0)
            {
                request.SetCreateDefault(this.CurrentUser);
                _appSystemServices.Create<Intellisence>(request);
            }
            else
            {
                request.SetModifyDefault(this.CurrentUser);
                _appSystemServices.Modify<Intellisence>(request);
            }
            return response;
        }

        /// <summary>
        /// 删除代码片段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Remove")]
        [Authorize]
        public ResponseDto<Boolean> Remove([FromBody] Intellisence request)
        {
            ResponseDto<Boolean> response = new ResponseDto<Boolean>(); 
            if (string.IsNullOrEmpty(request.Id.ToStringExtension()))
            {

                response.Message = "Key 不能为空";
                response.Success = false;
                return response;
            } 
            var _entity = _appSystemServices.GetEntitys<Intellisence>();
            response.Data = _entity.Where(x => x.Id == request.Id).ToDelete().ExecuteAffrows() > 0; 
            return response;
        }
    }
}

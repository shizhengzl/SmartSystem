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
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppServices.Model;
using static AutoMapper.Internal.ExpressionFactory;

namespace WebAppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : BaseController
    {
        private IMapper _mapper { get; set; }
        private UsersSrevices _userServices { get; set; }
        private DataBaseServices _dataBaseServices { get; set; }

        private AppSystemServices _appSystemServices { get; set; }
        private SystemServices _sysservices { get; set; }
        public EnumController(IMapper mapper
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
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetDataType")]
        [Authorize]
        public ResponseListDto<EnumClass> GetDataType()
        {
            ResponseListDto<EnumClass> response = new ResponseListDto<EnumClass>();

            var data = EnumExtensions.GetListEnumClass<DataType>();
            response.Total = data.Count();
            response.Data = data;

            return response;
        }
        /// <summary>
        /// 获取授权模式
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetGrantMode")]
        [Authorize]
        public ResponseListDto<EnumClass> GetGrantMode()
        {
            ResponseListDto<EnumClass> response = new ResponseListDto<EnumClass>();

            var data = EnumExtensions.GetListEnumClass<GrantMode>();
            response.Total = data.Count();
            response.Data = data;

            return response;
        }
    }
}

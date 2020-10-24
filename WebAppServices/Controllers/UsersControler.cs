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
    public class UsersController : ControllerBase
    {
        private IMapper _mapper { get; set; }

        private UsersSrevices _userServices { get; set; }


        private SystemServices _sysservices { get; set; }
        public UsersController(IMapper mapper
            , UsersSrevices usersSrevices
            , SystemServices sysservices
            )
        {
            _mapper = mapper;
            _userServices = usersSrevices;
            _sysservices = sysservices;
        }

        [HttpGet("GetUsersList")]
        public ResponseListDto<UserDto> GetUsersList()
        {
            ResponseListDto<UserDto> response = new ResponseListDto<UserDto>();
            try
            { 
                response.Data = _userServices.GetUserList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false; 
                _sysservices.AddExexptionLogs(ex, "GetUsersList");
            }
            return response;
        }


        [HttpPost("Register")]
        public ResponseDto<bool> Register([FromBody] UserDto user)
        {
            ResponseDto<bool> response = new ResponseDto<bool>();
            try
            {
                if (string.IsNullOrEmpty(user.Phone))
                {
                    response.Message = "请输入手机号";
                    response.Success = false;
                }

                var searchuser = _userServices.GetUser(user.Phone);
                if (searchuser != null)
                {
                    response.Message = "手机号码已经注册";
                    response.Success = false;
                }


                response.Data = _userServices.RegisterUser(user);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Register");
            }
            return response;
        }

    }
}

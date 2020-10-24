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
         
    }
}

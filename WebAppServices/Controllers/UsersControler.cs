using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        [HttpPost("Login")]
        [AllowAnonymous]
        public ResponseDto<String> Login([FromBody] LoginDto user)
        {
            ResponseDto<String> response = new ResponseDto<String>() { Success = false};
            try
            {
                if (string.IsNullOrEmpty(user.Username))
                {
                    response.Message = "请输入用户名或者手机号码";
                    return response;
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    response.Message = "请输入密码";
                    return response;
                }

                user.Password = user.Password.ToMD5();

                var users = _userServices.GetUser(user.Username);
                if (users == null)
                {
                    response.Message = "用户名不正确";
                    return response;
                }
                if (users.Password != user.Password)
                {
                    response.Message = "密码不正确";
                    return response;
                }
                if (!users.IsEnabled)
                {
                    response.Message = "用户已经禁用";
                    return response;
                }
 
                 
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, users.Id.ToStringExtension())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PublicConst.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: PublicConst.Domain,
                    audience: PublicConst.Domain,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                response.Success = true;
                response.Data = new JwtSecurityTokenHandler().WriteToken(token);
                
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                _sysservices.AddExexptionLogs(ex, "Login");
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
                user.Password = user.Password.ToMD5();
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

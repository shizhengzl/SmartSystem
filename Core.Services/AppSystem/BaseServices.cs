using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class BaseServices : IServices
    { 
        /// <summary>
        /// 注入接口
        /// </summary>
        public readonly IMapper _mapper;
        public BaseServices(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}

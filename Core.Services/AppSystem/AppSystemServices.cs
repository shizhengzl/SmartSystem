using AutoMapper;
using Core.Repository;
using Core.UsuallyCommon;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static AutoMapper.Internal.ExpressionFactory;

namespace Core.Services
{
    public class AppSystemServices : IServices
    {
        /// <summary>
        /// 注入接口
        /// </summary>
        public readonly IMapper _mapper;

         

        public AppSystemServices(IMapper mapper)
        {
            _mapper = mapper;
        }
         

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseRequest"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public FreeSql.ISelect<T> GetData<T>() where T : class
        {
            ResponseListDto<T> response = new ResponseListDto<T>(); 
            return FreeSqlFactory._Freesql.Select<T>(); 
        }

    }
}

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
    [AppServiceAttribute]
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
        public FreeSql.ISelect<T> GetEntitys<T>() where T : class
        {
            ResponseListDto<T> response = new ResponseListDto<T>(); 
            return FreeSqlFactory._Freesql.Select<T>(); 
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseRequest"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public Boolean Remove<T>(T t) where T : class
        {
            ResponseListDto<T> response = new ResponseListDto<T>();
            return FreeSqlFactory._Freesql.Delete<T>(t).ExecuteAffrows() > 1;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseRequest"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public Boolean Create<T>(T t) where T : class
        {
            ResponseListDto<T> response = new ResponseListDto<T>();
            return FreeSqlFactory._Freesql.Insert<T>(t).ExecuteAffrows() > 1;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseRequest"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public Boolean Modify<T>(T t) where T : class
        {
            ResponseListDto<T> response = new ResponseListDto<T>(); 
            return FreeSqlFactory._Freesql.Update<T>().SetSource(t).ExecuteAffrows() > 1;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repository; 
using Core.UsuallyCommon;

namespace Core.Intellisence
{
    /// <summary>
    /// 数据服务类
    /// </summary>
    public static class DataServices
    {
        /// <summary>
        /// 获取需要提示的Intellisence
        /// </summary>
        /// <param name="startchar">激活智能感知字符串</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        public static List<Core.Repository.Intellisence> GetIntellisence(String startchar, String keyword)
        {
            var list = FreeSqlFactory._Freesql.Select<Core.Repository.Intellisence>().Where(x => x.StartChar == startchar ).ToList();

            var SqlList = list.Where(x => x.IsSql).ToList();

            var containsList = list.Where(x => !x.IsSql
               &&

               (
                   (x.DisplayText.ToStringExtension().Contains(keyword, StringComparison.OrdinalIgnoreCase) || keyword.SearchWordExists(new String[] { x.DisplayText.ToStringExtension() }))
                   ||
                   (x.SearchText.ToStringExtension().Contains(keyword, StringComparison.OrdinalIgnoreCase) || keyword.SearchWordExists(new String[] { x.SearchText.ToStringExtension() }))
               )

               ).ToList();
            return SqlList.Concat(containsList).ToList();
        }

        /// <summary>
        /// 判断是否需要命中
        /// </summary>
        /// <param name="startchar"></param>
        /// <returns></returns>
        public static Boolean IsHit(String startchar)
        {
            return FreeSqlFactory._Freesql.Select<Core.Repository.Intellisence>().Any(x => x.StartChar == startchar);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="startchar"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<SCompletionList> GetCompletionList(String startchar, String keyword)
        {
            List<SCompletionList> list = new List<SCompletionList>();
            List<Core.Repository.Intellisence> intellisences = GetIntellisence(startchar, keyword);

            intellisences.Where(x => !x.IsSql).ToList().ForEach(o =>
            {
                list.Add(new SCompletionList()
                {
                    Description = o.Description,
                    DisplayText = o.DisplayText,
                    InsertionText = o.InsertionText
                });
            });

            intellisences.Where(x => x.IsSql && !String.IsNullOrEmpty(x.ConnectionString)).ToList().ForEach(o=> {

                IFreeSql freeSql = FreeSqlFactory.GetFreeSql(o.DataType, o.ConnectionString);

                var sql = o.DefinedSql.Replace(FreeSqlFactory.ReplaceString, keyword);
                var scpmpletiontable = freeSql.Ado.ExecuteDataTable(sql);
                if (scpmpletiontable.Columns.Count == 1)
                {
                    foreach (DataRow item in scpmpletiontable.Rows)
                    {
                        var text = item[0].ToStringExtension();
                        list.Add(new SCompletionList() {
                            Description = text,
                            DisplayText = text,
                            InsertionText = text
                        });

                    }
                }
                else
                    list.AddRange(scpmpletiontable.ToList<SCompletionList>());
                
                
            });

            return list;
        }



        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="description"></param>
        public static void AddExexptionLogs(Exception ex, string description)
        {
            SystemLogs logs = new SystemLogs()
            {
                CreateTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Description = description

            };
            FreeSqlFactory._Freesql.Insert<SystemLogs>(logs).ExecuteAffrows();
        }
    }
}

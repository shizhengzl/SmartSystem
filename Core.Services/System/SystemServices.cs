using Core.Repository;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class SystemServices : IServices
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="description"></param>
        public void AddExexptionLogs(Exception ex, string description)
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

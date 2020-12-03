using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{ 
    public interface IServices
    {

    }


    /// <summary>
    /// 标记服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AppServiceAttribute : Attribute
    {
    }
}

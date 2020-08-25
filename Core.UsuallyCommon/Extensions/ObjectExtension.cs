using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Core.UsuallyCommon
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 获取对象属性名称
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static List<String> GetPropertyList(this object objects)
        {
            PropertyInfo[] propertys = objects.GetType().GetProperties();

            return propertys.Select(x => x.Name).ToList<string>();
        }
    }
}

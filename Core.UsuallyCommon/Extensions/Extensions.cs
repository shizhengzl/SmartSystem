using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Core.UsuallyCommon
{
    /// <summary>
    /// type conversion
    /// 类型转换
    /// </summary>
    public static class Extensions
    {
        public static Type GetClassType(this string typeName)
        {
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))
                    {
                        return typeArray[j];
                    }
                }
            }
            return type;
        }

            /// <summary>
            /// 扩展判断是否为空
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static Boolean IsNull(this object obj)
        {
            return obj == null;
        } 

        /// <summary>
        /// Any object converts a string. If the conversion fails, string.empty will be returned
        /// 任意对象转字符串，Guid.Empty
        /// </summary>
        /// <param name="obj">Any object(任意对象)</param>
        /// <returns>Guid</returns>
        public static Guid ToGuid(this object obj)
        {
            Guid result = Guid.Empty;
            if (obj != null)
                Guid.TryParse(obj.ToStringExtension(), out result);
            return result;
        }

        /// <summary>
        /// Any object converts a string. If the conversion fails, string.empty will be returned
        /// 任意对象转字符串，如转换失败则返回String.Empty
        /// </summary>
        /// <param name="obj">Any object(任意对象)</param>
        /// <returns>String</returns>
        public static string ToStringExtension(this object obj)
        {
            string result = string.Empty;
            if (obj != null)
                result = obj.ToString();
            return result;
        }


        /// <summary>
        /// Any object is transferred to int32, if the conversion fails, 0 will be returned
        /// 任意对象转Int32，如转换失败则返回0
        /// </summary>
        /// <param name="obj">Any object(任意对象)</param>
        /// <returns>Int32</returns>
        public static Int32 ToInt32(this object obj)
        {
            Int32 result = 0;
            if (obj == null)
                return result;
            bool isparse = Int32.TryParse(obj.ToStringExtension(), out result);
            return result;
        }


        /// <summary>
        /// Any object is transferred to int64, if the conversion fails, 0 will be returned
        /// 任意对象转Int64，如转换失败则返回0
        /// </summary>
        /// <param name="obj">Any object(任意对象)</param>
        /// <returns>Int64</returns>
        public static Int64 ToInt64(this object obj)
        {
            Int64 result = 0;
            if (obj == null)
                return result;
            bool isparse = Int64.TryParse(obj.ToStringExtension(), out result);
            return result;
        }


        /// <summary>
        /// Any object is transferred to DateTime, if the conversion fails, DateTime.MinValue will be returned
        /// 任意对象转DateTime，如转换失败则返回DateTime.MinValue
        /// </summary>
        /// <param name="obj">Any object(任意对象)</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this object obj)
        {
            DateTime result = DateTime.MinValue;
            if (obj != null)
                DateTime.TryParse(obj.ToString(), out result);
            return result;
        }
 

        /// <summary>
        /// Any object is transferred to Decimal, if the conversion fails, Decimal.MaxValue will be returned
        /// 任意对象转Decimal，如转换失败则返回Decimal.MaxValue
        /// </summary>
        /// <param name="obj">Any object(任意对象)</param>
        /// <returns>ToDecimal</returns>
        public static Decimal ToDecimal(this object obj)
        {
            Decimal result = Decimal.MaxValue;
            if (obj != null)
                Decimal.TryParse(obj.ToStringExtension(), out result);
            return result;
        }


        /// <summary>
        /// Any object is transferred to Boolean, if the conversion fails, false will be returned
        /// 任意对象转Boolean，如转换失败则返回false
        /// </summary>
        /// <param name="objects">Any object(任意对象)</param>
        /// <returns>Boolean</returns>
        public static Boolean ToBoolean(this object objects)
        {
            Boolean result = false;
            if (objects != null)
                Boolean.TryParse(objects.ToString(), out result);
            return result;
        }


        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>List<T></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();
                //找到对应的数据  并赋值
                prlist.ForEach(p =>
                {
                    if (row[p.Name] != DBNull.Value)
                    {
                        if (p.PropertyType == typeof(string))
                            p.SetValue(ob, row[p.Name].ToStringExtension(), null);
                        else if (p.PropertyType == typeof(Guid))
                            p.SetValue(ob, row[p.Name].ToGuid(), null);
                        else if (p.PropertyType == typeof(bool))
                            p.SetValue(ob, row[p.Name].ToBoolean(), null);
                        else if (p.PropertyType == typeof(decimal))
                            p.SetValue(ob, row[p.Name].ToDecimal(), null);
                        else if (p.PropertyType == typeof(Int16))
                            p.SetValue(ob, row[p.Name].ToInt32(), null);
                        else if (p.PropertyType == typeof(Int32))
                            p.SetValue(ob, row[p.Name].ToInt32(), null);
                        else if (p.PropertyType == typeof(Int64))
                            p.SetValue(ob, row[p.Name].ToInt64(), null);

                        else if (p.PropertyType == typeof(decimal?))
                            p.SetValue(ob, row[p.Name].ToDecimal(), null);
                        else if (p.PropertyType == typeof(Int16?))
                            p.SetValue(ob, row[p.Name].ToInt32(), null);
                        else if (p.PropertyType == typeof(Int32?))
                            p.SetValue(ob, row[p.Name].ToInt32(), null);
                        else if (p.PropertyType == typeof(Int64?))
                            p.SetValue(ob, row[p.Name].ToInt64(), null);

                        else
                            p.SetValue(ob, row[p.Name], null);
                    }
                });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetPropertyValue(this object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj, null).ToStringExtension();
        }
    }
}
 
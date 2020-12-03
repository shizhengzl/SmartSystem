using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebAppServices
{
    public static class AutomaticInjection
    {
        /// <summary>
        /// 注册应用程序域中所有有AppService特性的服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAppServices(this IServiceCollection services)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var serviceAttribute = type.GetCustomAttribute<AppServiceAttribute>();

                    if (serviceAttribute != null)
                    {
                        services.AddScoped(type);
                    }
                }
            }
        }

        public static void ResolveAllTypes(this IServiceCollection services, params string[] projectSuffixes)
        {
            //projectSuffixes 需要扫描的项目名称集合
            //注意: 如果使用此方法，必须提供需要扫描的项目名称
            var allAssemblies = new List<Assembly>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(dll));
            }
            var implementTypes = new List<Type>();
            var assemblyList = allAssemblies.Where(t => projectSuffixes.Any(m => t.FullName.Contains(m))).ToList();
            foreach (var assembly in assemblyList)
            {
                //找到程序集所有接口
                implementTypes.AddRange(assembly.DefinedTypes.Where(t => t.IsClass).ToList());
            }
            foreach (var implementType in implementTypes)
            {
                if (implementType.GetInterfaces().Any(x => x.Name == "IServices" ))
                {
                    services.AddScoped (implementType);
                }

                //接口和实现的命名规则为："AService"类实现了"IAService"接口,你也可以自定义规则
                //var interfaceType = implementType.GetInterface("IServices" );
                //if (interfaceType != null && !interfaceType.IsGenericType)
                //{
                //    //services.AddSingleton(interfaceType, implementType);
                //    services.AddScoped(implementType);
                //}
            } 
            //注意这里:上面两行代码是.net core 正常配置代码（为什么这里使用了反射自动配置还要加入此代码,我在这里解释一下,因为上面两个是泛型类和泛型接口,我也不知道为什么这里用反射配置泛型的时候会报错,暂时没找到解决办法,所以这里采用一个傻的办法 就是手写一遍这两个的依赖注入关系）
        }
    }
}

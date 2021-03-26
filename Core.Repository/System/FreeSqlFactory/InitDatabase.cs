using Core.Repository.Generator;
using Core.UsuallyCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository
{
    public class InitDatabase
    {

        public void AddData()
        {

            if (!FreeSqlFactory._Freesql.Select<Menus>().Where(x => x.MenuName == "资料管理").Any())
            { 
                Menus s1 = new Menus() { MenuName = "资料管理", MenuIcon = "404", MenuPath = "/document", Url = "Layout", IsDeafult = true, IsAvailable = true };
                var s1id = FreeSqlFactory._Freesql.Insert<Menus>(s1).ExecuteIdentity();

                Menus m2 = new Menus()
                {
                    MenuName = "学习资料管理",
                    MenuIcon = "404",
                    MenuPath = "/document",
                    Url = "/document/study",
                    IsAvailable = true,
                    ParentId = s1id
                };

                FreeSqlFactory._Freesql.Insert<Menus>(m2).ExecuteAffrows();
            }
        }

        public void InitData(Boolean isClean = false)
        {
            if (isClean)
            {
                FreeSqlFactory._Freesql.Delete<SQLConfig>().Where("1=1").ExecuteAffrows();


                FreeSqlFactory._Freesql.Delete<Roles>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<RoleMenus>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<RoleUsers>().Where("1=1").ExecuteAffrows();

                FreeSqlFactory._Freesql.Delete<Department>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<DepartmentMenus>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<DepartmentUsers>().Where("1=1").ExecuteAffrows();

                FreeSqlFactory._Freesql.Delete<Company>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<CompanyMenus>().Where("1=1").ExecuteAffrows();

                FreeSqlFactory._Freesql.Delete<Users>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<Menus>().Where("1=1").ExecuteAffrows();


                FreeSqlFactory._Freesql.Delete<DataBaseConnection>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<TableArea>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<TableAreaData>().Where("1=1").ExecuteAffrows();

                FreeSqlFactory._Freesql.Delete<TestModule>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<Element>().Where("1=1").ExecuteAffrows();
            }

            Int64 companyid = 0;
            Int64 jzbid = 0;
            // 初始话单位
            if (!FreeSqlFactory._Freesql.Select<Company>().Where(x => x.CompanyName == CommonEnum.SupperCompany).Any())
            {
                Company company = new Company()
                {
                    CompanyName = CommonEnum.SupperCompany,
                    CompanyPhone = "13701859214"
                };


                Company jzb = new Company()
                {
                    CompanyName = "长沙计支宝",
                    CompanyPhone = "13700000000"
                };
                companyid = FreeSqlFactory._Freesql.Insert<Company>(company).ExecuteIdentity();
                jzbid = FreeSqlFactory._Freesql.Insert<Company>(jzb).ExecuteIdentity();
            }


            Int64 shizhengid = 0;
            Int64 tanxudongid = 0;

            // 初始化用户 
            if (!FreeSqlFactory._Freesql.Select<Users>().Where(x => x.UserName == CommonEnum.SupperUser).Any())
            {
                Users shizheng = new Users() { UserName = "shizheng", Phone = "13701859214", NikeName = "shizheng", Name = "施政", IsEnabled = true, Email = "shizheng89@qq.com", Password = "123456".ToMD5(), CompanyId = companyid };

                shizhengid = FreeSqlFactory._Freesql.Insert<Users>(shizheng).ExecuteIdentity();

                var user = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Id == shizhengid).First();
                user.Password = (user.Password + shizhengid.ToStringExtension()).ToMD5();
                FreeSqlFactory._Freesql.Update<Users>().SetSource(user).ExecuteAffrows();

                CompanyUsers companyUsers = new CompanyUsers() { UserId = shizhengid, CompanyId = companyid, JobStatus = JobStatus.InJob };
                FreeSqlFactory._Freesql.Insert<CompanyUsers>(companyUsers).ExecuteAffrows();



                Users tanxudong = new Users() { UserName = "zhaowenyi", Phone = "15221828554", NikeName = "zhaowenyi", Name = "赵文毅", IsEnabled = true, Password = "123456".ToMD5(), CompanyId = jzbid };

                tanxudongid = FreeSqlFactory._Freesql.Insert<Users>(tanxudong).ExecuteIdentity();

                var usertx = FreeSqlFactory._Freesql.Select<Users>().Where(x => x.Id == tanxudongid).First();
                usertx.Password = (usertx.Password + tanxudongid.ToStringExtension()).ToMD5();
                FreeSqlFactory._Freesql.Update<Users>().SetSource(usertx).ExecuteAffrows();

                CompanyUsers companyUserstx = new CompanyUsers() { UserId = tanxudongid, CompanyId = jzbid, JobStatus = JobStatus.InJob };
                FreeSqlFactory._Freesql.Insert<CompanyUsers>(companyUserstx).ExecuteAffrows();

            }



            // 初始化菜单
            if (!FreeSqlFactory._Freesql.Select<Menus>().Where("1=1").Any())
            {
                Menus m1 = new Menus() { MenuName = "超级系统管理", MenuIcon = "system", MenuPath = "/sppersystem", Url = "Layout", IsAvailable = true };
                var m1id = FreeSqlFactory._Freesql.Insert<Menus>(m1).ExecuteIdentity();


                Menus s1 = new Menus() { MenuName = "系统管理", MenuIcon = "system", MenuPath = "/system", Url = "Layout", IsDeafult = true, IsAvailable = true };
                var s1id = FreeSqlFactory._Freesql.Insert<Menus>(s1).ExecuteIdentity();

                Menus m2 = new Menus()
                {
                    MenuName = "菜单管理",
                    MenuIcon = "menu",
                    MenuPath = "/menus",
                    Url = "/system/menus",
                    IsAvailable = true,
                    ParentId = m1id
                };
                Menus m3 = new Menus()
                {
                    MenuName = "请求日志",
                    MenuIcon = "systemlog",
                    MenuPath = "/requestresponselog",
                    Url = "/system/requestresponselog",
                    IsAvailable = true,
                    ParentId = m1id
                };

                Menus m6 = new Menus()
                {
                    MenuName = "系统日志",
                    MenuIcon = "systemlog",
                    MenuPath = "/systemlog",
                    Url = "/system/systemlogs",
                    IsAvailable = true,
                    ParentId = m1id
                };


                Menus m4 = new Menus()
                {
                    MenuName = "角色管理",
                    MenuIcon = "role",
                    MenuPath = "/roles",
                    Url = "/system/roles",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = s1id
                };
                Menus m5 = new Menus()
                {
                    MenuName = "用户管理",
                    MenuIcon = "user",
                    MenuPath = "/users",
                    Url = "/system/users",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = s1id
                };

                Menus m7 = new Menus()
                {
                    MenuName = "单位管理",
                    MenuIcon = "company",
                    MenuPath = "/company",
                    Url = "/system/company",
                    IsAvailable = true,
                    ParentId = m1id
                };



                Menus m8 = new Menus()
                {
                    MenuName = "我的单位管理",
                    MenuIcon = "company",
                    MenuPath = "/mycompany",
                    Url = "/system/mycompany",
                    IsAvailable = true,
                    IsDeafult = true,
                    ParentId = s1id
                };

                Menus m9 = new Menus()
                {
                    MenuName = "部门管理",
                    MenuIcon = "department",
                    MenuPath = "/department",
                    Url = "/system/department",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = s1id
                };


                Menus m10 = new Menus()
                {
                    MenuName = "SQL配置",
                    MenuIcon = "tool",
                    MenuPath = "/sqlconfig",
                    Url = "/system/sqlconfig",
                    IsAvailable = true,
                    ParentId = m1id
                };


                FreeSqlFactory._Freesql.Insert<Menus>(m2).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m3).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m4).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m5).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m6).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m7).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m8).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m9).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m10).ExecuteAffrows();
                Menus t1 = new Menus() { MenuName = "工具管理", MenuIcon = "tool", MenuPath = "/tools", Url = "Layout", IsDeafult = true, IsAvailable = true };
                var t1id = FreeSqlFactory._Freesql.Insert<Menus>(t1).ExecuteIdentity();

                Menus t2 = new Menus()
                {
                    MenuName = "数据字典",
                    MenuIcon = "404",
                    MenuPath = "/dictionarie",
                    Url = "/tools/dictionarie",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t3 = new Menus()
                {
                    MenuName = "代码片段",
                    MenuIcon = "404",
                    MenuPath = "/intellisence",
                    Url = "/tools/intellisence",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t4 = new Menus()
                {
                    MenuName = "业务分类",
                    MenuIcon = "404",
                    MenuPath = "/tablearea",
                    Url = "/tools/tablearea",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t5 = new Menus()
                {
                    MenuName = "表业务数据",
                    MenuIcon = "404",
                    MenuPath = "/tableareadata",
                    Url = "/tools/tableareadata",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t6 = new Menus()
                {
                    MenuName = "连接字符串管理",
                    MenuIcon = "404",
                    MenuPath = "/databaseconnection",
                    Url = "/tools/databaseconnection",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                FreeSqlFactory._Freesql.Insert<Menus>(t2).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t3).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t4).ExecuteAffrows();
                //FreeSqlFactory._Freesql.Insert<Menus>(t5).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t6).ExecuteAffrows();

                Menus c1 = new Menus() { MenuName = "自动化测试", MenuIcon = "404", MenuPath = "/autotest", Url = "Layout", IsDeafult = true, IsAvailable = true };
                var c1id = FreeSqlFactory._Freesql.Insert<Menus>(c1).ExecuteIdentity();

                Menus c2 = new Menus()
                {
                    MenuName = "模块管理",
                    MenuIcon = "404",
                    MenuPath = "/testmodule",
                    Url = "/autotest/testmodule",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = c1id
                };
                FreeSqlFactory._Freesql.Insert<Menus>(c2).ExecuteAffrows();

                Menus c3 = new Menus()
                {
                    MenuName = "页面元素管理",
                    MenuIcon = "404",
                    MenuPath = "/element",
                    Url = "/autotest/element",
                    IsDeafult = true,
                    IsAvailable = true,
                    ParentId = c1id
                };
                FreeSqlFactory._Freesql.Insert<Menus>(c3).ExecuteAffrows();
            }


            // 初始化游客菜单


            // 初始化连接字符串
            if (!FreeSqlFactory._Freesql.Select<DataBaseConnection>().Where("1=1").Any())
            {
                DataBaseConnection dataBaseConnection = new DataBaseConnection()
                {
                    ConnectinString = "server=.;uid=sa;pwd=sasa;database=SmartDb;"
                     ,
                    DataBaseName = "SmartDb"
                     ,
                    DataBaseType = FreeSql.DataType.SqlServer,
                    CompanyId = companyid
                };

                DataBaseConnection JZB_CRM = new DataBaseConnection()
                {
                    ConnectinString = "Data Source=192.168.0.100;Initial Catalog=JZB_CRM;Persist Security Info=True;User ID=u_admin;Password=t%lZIJcZvqmiL2F!8JgU;pooling=false;"
                     ,
                    DataBaseName = "JZB_CRM"
                     ,
                    DataBaseType = FreeSql.DataType.SqlServer
                    ,
                    CompanyId = jzbid
                };

                FreeSqlFactory._Freesql.Insert<DataBaseConnection>(dataBaseConnection).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<DataBaseConnection>(JZB_CRM).ExecuteAffrows();
            }


            // 初始化数据库设置
            if (!FreeSqlFactory._Freesql.Select<SQLConfig>().Any())
            {
                SQLConfig configSqlserver = new SQLConfig()
                {
                    Type = FreeSql.DataType.SqlServer,
                    GetDataBaseSQL = "SELECT name AS DataBaseName FROM sys.sysdatabases ORDER BY name",
                    GetTableSQL = @"select a.name AS TableName,ISNULL(b.value,'') AS TableDescription from sys.tables a LEFT JOIN  sys.extended_properties  b 
                                on a.object_id = b.major_id AND  b.minor_id = 0 ORDER BY a.name",
                    GetColumnSQL = @" 
                                select
                                col.name as ColumnName,
                                col.is_identity IsIdentity,
                                convert(bit,(case when col.is_nullable = 0 then 1 else 0 end)) as IsRequire,
                                convert(int,col.max_length) as MaxLength,
                                tp.name as SQLType,
                                ep.value as ColumnDescription,
                                convert(bit,(
                                    select count(*) from sys.sysobjects
                                    where parent_obj=obj.id
                                    and name=(
                                        select top 1 name from sys.sysindexes ind
                                        inner join sys.sysindexkeys indkey
                                        on ind.indid=indkey.indid
                                        and indkey.colid=col.column_id
                                        and indkey.id=obj.id
                                        where ind.id=obj.id
                                        and ind.name like 'PK_%'
                                    )
                                )) as IsPrimarykey
                                from sys.sysobjects obj
                                inner join sys.columns col
                                on obj.id = col.object_id
                                left join sys.systypes tp
                                on col.system_type_id=tp.xusertype
                                left join sys.extended_properties ep
                                on ep.major_id=obj.id
                                and ep.minor_id=col.column_id
                                and ep.name='MS_Description'
                                where obj.name= '{0}' order by col.is_ansi_padded desc",
                    AddExtendedproperty = @"EXECUTE sp_addextendedproperty N'MS_Description', '{2}', N'user', N'dbo', N'table', N'{0}', N'column', N'{1}'"
                   ,
                    ModifyExtendedproperty = @"EXECUTE sp_updateextendedproperty 'MS_Description', '{2}', 'user', dbo, 'table', '{0}', 'column', {1}"
                   ,
                    AddTableExtendedproperty = @"EXECUTE sp_addextendedproperty   N'MS_Description','{1}',N'user',N'dbo',N'table',N'{0}',NULL,NULL"
                   ,
                    ModifyTableExtendedproperty = @"EXECUTE sp_updateextendedproperty   N'MS_Description','{1}',N'user',N'dbo',N'table',N'{0}',NULL,NULL"
                };
                SQLConfig configMySql = new SQLConfig()
                {
                    Type = FreeSql.DataType.MySql,
                    GetDataBaseSQL = "SELECT `SCHEMA_NAME`   as DataBaseName FROM `information_schema`.`SCHEMATA`",
                    GetTableSQL = @"use information_schema;  
                                select table_name as TableName,
                                table_comment As TableDescription from tables where table_schema = '@DataBaseName'",
                    GetColumnSQL = @"
                                use {1};
                                SELECT TABLE_NAME 'TableName'
                                ,ORDINAL_POSITION 'ORDINAL_POSITION'
                                ,COLUMN_NAME 'ColumnName'
                                ,COLUMN_DEFAULT 'COLUMN_DEFAULT',
                                CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END AS 'IsRequire'
																,DATA_TYPE 'SQLType'
                                ,CASE WHEN  ISNULL(CHARACTER_MAXIMUM_LENGTH) = 1 THEN 0 ELSE CHARACTER_MAXIMUM_LENGTH END AS 'MaxLength'
                                ,COLUMN_COMMENT 'ColumnDescription'
                                ,CASE WHEN COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END AS IsPrimarykey
								,CASE WHEN EXTRA ='auto_increment'  THEN 1 ELSE 0 END AS IsIdentity
                                FROM INFORMATION_SCHEMA.COLUMNS  
                                WHERE    TABLE_NAME='{2}' and table_schema = '{1}';
                                 "
                };
                FreeSqlFactory._Freesql.Insert<SQLConfig>(configSqlserver).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<SQLConfig>(configMySql).ExecuteAffrows();
            }
        }


        public InitDatabase()
        {

        }
    }
}

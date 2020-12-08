using Core.Repository.Generator;
using Core.UsuallyCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository
{
    public class InitDatabase
    {
        public InitDatabase(Boolean isClean = false)
        {


            if (isClean)
            {
                FreeSqlFactory._Freesql.Delete<SQLConfig>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<Roles>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<Menus>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<RoleMenus>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<RoleUsers>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<Users>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<DataBaseConnection>().Where("1=1").ExecuteAffrows();
                FreeSqlFactory._Freesql.Delete<Company>().Where("1=1").ExecuteAffrows();

                FreeSqlFactory._Freesql.Delete<TableArea>().Where("1=1").ExecuteAffrows();

                FreeSqlFactory._Freesql.Delete<TableAreaData>().Where("1=1").ExecuteAffrows();
            }

            Int64 companyid = 0;
            // 初始话单位
            if (!FreeSqlFactory._Freesql.Select<Company>().Where(x => x.CompanyName == CommonEnum.SupperCompany).Any())
            {
                Company company = new Company() {
                     CompanyName = CommonEnum.SupperCompany 
                };
                companyid = FreeSqlFactory._Freesql.Insert<Company>(company).ExecuteIdentity(); 
            }


            Int64 adminrole = 0;
            Int64 touristrole = 0; 

            // 初始化角色
            if (!FreeSqlFactory._Freesql.Select<Roles>().Where(x => x.RoleName == CommonEnum.SupperAdmin).Any())
            {
                Roles supperadmin = new Roles() { RoleName = CommonEnum.SupperAdmin, RoleDescription = CommonEnum.SupperAdmin };
                Roles tourist = new Roles() { RoleName = CommonEnum.Tourist, RoleDescription = CommonEnum.Tourist };
                adminrole = FreeSqlFactory._Freesql.Insert<Roles>(supperadmin).ExecuteIdentity();
                touristrole = FreeSqlFactory._Freesql.Insert<Roles>(tourist).ExecuteIdentity();
            }

            Int64 adminid = 0;
            Int64 shizhengid = 0;
            Int64 youkeid = 0;

            // 初始化用户 
            if (!FreeSqlFactory._Freesql.Select<Users>().Where(x => x.UserName == CommonEnum.SupperUser).Any())
            {
                Users users = new Users() { UserName = CommonEnum.SupperUser, Password = "123456".ToMD5() };
                Users shizheng = new Users() { UserName = "shizheng", Password = "123456".ToMD5() };
                Users youke = new Users() { UserName = CommonEnum.Youke, Password = "123456".ToMD5() };
                adminid = FreeSqlFactory._Freesql.Insert<Users>(users).ExecuteIdentity();
                shizhengid = FreeSqlFactory._Freesql.Insert<Users>(shizheng).ExecuteIdentity();
                youkeid = FreeSqlFactory._Freesql.Insert<Users>(youke).ExecuteIdentity();
            }

            // 初始化角色用户
            if (!FreeSqlFactory._Freesql.Select<RoleUsers>().Where("1=1").Any())
            {
                RoleUsers r1 = new RoleUsers() { RoleId = adminrole, UserId = adminid };
                RoleUsers r2 = new RoleUsers() { RoleId = adminrole, UserId = shizhengid };
                RoleUsers r3 = new RoleUsers() { RoleId = touristrole, UserId = youkeid };
                FreeSqlFactory._Freesql.Insert<RoleUsers>(r1).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<RoleUsers>(r2).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<RoleUsers>(r3).ExecuteAffrows();
            }

            // 初始化菜单
            if (!FreeSqlFactory._Freesql.Select<Menus>().Where("1=1").Any())
            {
                Menus m1 = new Menus() { MenuName = "超级系统管理", MenuIcon = "404", MenuPath = "/sppersystem", Url = "Layout", IsAlwaysShow = true, IsAvailable = true };
                var m1id = FreeSqlFactory._Freesql.Insert<Menus>(m1).ExecuteIdentity();


                Menus s1 = new Menus() { MenuName = "系统管理", MenuIcon = "404", MenuPath = "/system", Url = "Layout", IsAlwaysShow = true, IsAvailable = true };
                var s1id = FreeSqlFactory._Freesql.Insert<Menus>(s1).ExecuteIdentity();

                Menus m2 = new Menus()
                {
                    MenuName = "菜单管理",
                    MenuIcon = "404",
                    MenuPath = "/menus",
                    Url = "/system/menus",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = m1id
                };
                Menus m3 = new Menus()
                {
                    MenuName = "请求日志",
                    MenuIcon = "404",
                    MenuPath = "/requestresponselog",
                    Url = "/system/requestresponselog",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = m1id
                };

                Menus m6 = new Menus()
                {
                    MenuName = "系统日志",
                    MenuIcon = "404",
                    MenuPath = "/systemlog",
                    Url = "/system/systemlogs",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = m1id
                };
                 

                Menus m4 = new Menus()
                {
                    MenuName = "角色管理",
                    MenuIcon = "404",
                    MenuPath = "/roles",
                    Url = "/system/roles",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = s1id
                };
                Menus m5 = new Menus()
                {
                    MenuName = "用户管理",
                    MenuIcon = "404",
                    MenuPath = "/users",
                    Url = "/system/users",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = s1id
                };

               
                FreeSqlFactory._Freesql.Insert<Menus>(m2).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m3).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m4).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m5).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(m6).ExecuteAffrows();


                Menus t1 = new Menus() { MenuName = "工具管理", MenuIcon = "404", MenuPath = "/tools", Url = "Layout", IsAlwaysShow = true, IsAvailable = true };
                var t1id = FreeSqlFactory._Freesql.Insert<Menus>(t1).ExecuteIdentity();

                Menus t2 = new Menus()
                {
                    MenuName = "数据字典",
                    MenuIcon = "404",
                    MenuPath = "/dictionarie",
                    Url = "/tools/dictionarie",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = t1id
                }; 
                Menus t3 = new Menus()
                {
                    MenuName = "代码片段",
                    MenuIcon = "404",
                    MenuPath = "/intellisence",
                    Url = "/tools/intellisence",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t4 = new Menus()
                {
                    MenuName = "表归类",
                    MenuIcon = "404",
                    MenuPath = "/tablearea",
                    Url = "/tools/tablearea",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t5 = new Menus()
                {
                    MenuName = "表归类数据",
                    MenuIcon = "404",
                    MenuPath = "/tableareadata",
                    Url = "/tools/tableareadata",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                Menus t6 = new Menus()
                {
                    MenuName = "连接字符串管理",
                    MenuIcon = "404",
                    MenuPath = "/databaseconnection",
                    Url = "/tools/databaseconnection",
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = t1id
                };
                FreeSqlFactory._Freesql.Insert<Menus>(t2).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t3).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t4).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t5).ExecuteAffrows();
                FreeSqlFactory._Freesql.Insert<Menus>(t6).ExecuteAffrows();

                Menus c1 = new Menus() { MenuName = "自动化测试", MenuIcon = "404", MenuPath = "/autotest", Url = "Layout", IsAlwaysShow = true, IsAvailable = true };
                var c1id = FreeSqlFactory._Freesql.Insert<Menus>(c1).ExecuteIdentity();

                Menus c2 = new Menus()
                {
                    MenuName = "模块管理",
                    MenuIcon = "404",
                    MenuPath = "/testmodule",
                    Url = "/autotest/testmodule",
                    IsAlwaysShow = true,
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
                    IsAlwaysShow = true,
                    IsAvailable = true,
                    ParentId = c1id
                };
                FreeSqlFactory._Freesql.Insert<Menus>(c3).ExecuteAffrows();
            }


            // 初始化游客菜单


            // 初始化连接字符串
            if (!FreeSqlFactory._Freesql.Select<DataBaseConnection>().Where("1=1").Any())
            {
                DataBaseConnection dataBaseConnection = new DataBaseConnection() {
                     ConnectinString= "server=.;uid=sa;pwd=sasa;database=SmartDb;"
                     , DataBaseName= "SmartDb"
                     , DataBaseType= FreeSql.DataType.SqlServer
                };
                FreeSqlFactory._Freesql.Insert<DataBaseConnection>(dataBaseConnection).ExecuteAffrows();
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
    }
}

using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Core.UsuallyCommon;
using Core.Repository;

namespace Core.Services
{
    public class DataBaseServices
    {
        SQLConfigServices services = new SQLConfigServices();

         

        /// <summary>
        /// 获取所有连接
        /// </summary>
        /// <returns></returns>
        public List<DataBaseConnection> GetDataBaseConnections()
        {
            return FreeSqlFactory._Freesql.Select<DataBaseConnection>().ToList() ;
        }


        public DataBaseConnection GetConnectionString(Int64 Id)
        {
            return FreeSqlFactory._Freesql.Select<DataBaseConnection>().Where(x=>x.Id == Id).First();
        }

        public List<Table> GetTables(DataBaseConnection baseConnection)
        {
            var tablesql = services.GetTables(baseConnection.DataBaseType); 
            return FreeSqlFactory.GetFreeSql(baseConnection.DataBaseType,baseConnection.ConnectinString).Ado.ExecuteDataTable(tablesql).ToList<Table>();
        }

        public List<Column> GetColumns(DataBaseConnection baseConnection,string tableName)
        {
            var columnsql = services.GetColumns(baseConnection.DataBaseType);
            columnsql = string.Format(columnsql, tableName);
            return FreeSqlFactory.GetFreeSql(baseConnection.DataBaseType, baseConnection.ConnectinString).Ado.ExecuteDataTable(columnsql).ToList<Column>();
        }


        public bool AddExtendedproperty(DataBaseConnection baseConnection,string table,string column,string description)
        { 
            var propertysql = services.AddExtendedproperty(baseConnection.DataBaseType);
            propertysql = string.Format(propertysql, table,column,description);
            return FreeSqlFactory.GetFreeSql(baseConnection.DataBaseType, baseConnection.ConnectinString).Ado.ExecuteNonQuery(propertysql) > 0;
        }


        public bool ModifyExtendedproperty(DataBaseConnection baseConnection, string table, string column, string description)
        {
            var propertysql = services.ModifyExtendedproperty(baseConnection.DataBaseType);
            propertysql = string.Format(propertysql, table, column, description);
            return FreeSqlFactory.GetFreeSql(baseConnection.DataBaseType, baseConnection.ConnectinString).Ado.ExecuteNonQuery(propertysql) > 0;
        }


        public bool AddTableExtendedproperty(DataBaseConnection baseConnection, string table,  string description)
        {
            var propertysql = services.AddTableExtendedproperty(baseConnection.DataBaseType);
            propertysql = string.Format(propertysql, table, description);
            return FreeSqlFactory.GetFreeSql(baseConnection.DataBaseType, baseConnection.ConnectinString).Ado.ExecuteNonQuery(propertysql) > 0;
        }


        public bool ModifyTableExtendedproperty(DataBaseConnection baseConnection, string table,  string description)
        {
            var propertysql = services.ModifyTableExtendedproperty(baseConnection.DataBaseType);
            propertysql = string.Format(propertysql, table, description);
            return FreeSqlFactory.GetFreeSql(baseConnection.DataBaseType, baseConnection.ConnectinString).Ado.ExecuteNonQuery(propertysql) > 0;
        }


        public List<Table> GetTables()
        {
            var tablesql = services.GetTables(FreeSqlFactory.GetDataType);
            return FreeSqlFactory.GetFreeSql().Ado.ExecuteDataTable(tablesql).ToList<Table>();
        }

        public List<Column> GetColumns( string tableName)
        {
            var columnsql = services.GetColumns(FreeSqlFactory.GetDataType);
            columnsql = string.Format(columnsql, tableName);
            return FreeSqlFactory.GetFreeSql().Ado.ExecuteDataTable(columnsql).ToList<Column>();
        }


        public bool AddExtendedproperty(string table, string column, string description)
        {
            var propertysql = services.AddExtendedproperty(FreeSqlFactory.GetDataType);
            propertysql = string.Format(propertysql, table, column, description);
            return FreeSqlFactory.GetFreeSql().Ado.ExecuteNonQuery(propertysql) > 0;
        }

        public bool ModifyExtendedproperty(string table, string column, string description)
        {
            var propertysql = services.ModifyExtendedproperty(FreeSqlFactory.GetDataType);
            propertysql = string.Format(propertysql, table, column, description);
            return FreeSqlFactory.GetFreeSql().Ado.ExecuteNonQuery(propertysql) > 0;
        } 


        public bool AddTableExtendedproperty(string table, string description)
        {
            var propertysql = services.AddTableExtendedproperty(FreeSqlFactory.GetDataType);
            propertysql = string.Format(propertysql, table, description);
            return FreeSqlFactory.GetFreeSql().Ado.ExecuteNonQuery(propertysql) > 0;
        } 

        public bool ModifyTableExtendedproperty(string table, string description)
        {
            var propertysql = services.ModifyTableExtendedproperty(FreeSqlFactory.GetDataType);
            propertysql = string.Format(propertysql, table, description);
            return FreeSqlFactory.GetFreeSql().Ado.ExecuteNonQuery(propertysql) > 0;
        }
    }
}

using Core.UsuallyCommon;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Core.Repository.DataBase
{
    /// <summary>
    /// ADO帮助类
    /// </summary>
    public static class ADOHelper
    {

        /// <summary>
        /// Connection to sql
        /// </summary>
        private static SqlConnection _sqlConnection { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string connectionString = string.Empty;

        // 单例模式
        public static SqlConnection sqlConnection
        {
            get
            {

                _sqlConnection = new SqlConnection(connectionString);

                return _sqlConnection;
            }
        }

        /// <summary>
        /// 一行一个字段
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static string ExecuteScalar(string Sql)
        {
            string result = string.Empty;
            try
            {
                OpenConnection();
                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.Text;
                comd.CommandText = Sql;
                result = comd.ExecuteScalar().ToStringExtension();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 一行一个字段
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        public static string ExecuteScalar(string Sql, SqlParameter[] sps)
        {
            string result = string.Empty;
            try
            {
                OpenConnection();
                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.Text;
                comd.CommandText = Sql;
                foreach (SqlParameter sp in sps)
                    comd.Parameters.Add(sp);
                result = comd.ExecuteScalar().ToStringExtension();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 执行文本
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(string Sql)
        {
            DataSet Ds = new DataSet();
            try
            {
                OpenConnection();
                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.Text;
                comd.CommandText = Sql;
                SqlDataAdapter sda = new SqlDataAdapter(comd);


                sda.Fill(Ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return Ds;
        }

        /// <summary>
        /// 执行文本
        /// </summary>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(string Sql, SqlParameter[] sps)
        {
            DataSet Ds = new DataSet();
            try
            {
                OpenConnection();
                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.Text;
                comd.CommandText = Sql;
                foreach (SqlParameter sp in sps)
                    comd.Parameters.Add(sp);
                SqlDataAdapter sda = new SqlDataAdapter(comd);
                sda.Fill(Ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return Ds;
        }

        public static int ExecuteNoQuery(string Sql)
        {
            int result = 0;
            try
            {
                OpenConnection();

                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.Text;
                comd.CommandText = Sql;
                result = comd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 执行SQL命令 新增、修改、删除
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string Sql, SqlParameter[] sps)
        {
            int result = 0;
            try
            {
                OpenConnection();

                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.Text;
                comd.CommandText = Sql;

                foreach (SqlParameter sp in sps)
                    comd.Parameters.Add(sp);
                result = comd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 执行过程
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        public static DataSet ExecuteProcess(string ProcessName, SqlParameter[] sps)
        {
            DataSet Ds = new DataSet();
            try
            {
                OpenConnection();
                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.StoredProcedure;
                comd.CommandText = ProcessName;
                foreach (SqlParameter sp in sps)
                {
                    comd.Parameters.Add(sp);
                }
                SqlDataAdapter sda = new SqlDataAdapter(comd);
                sda.Fill(Ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return Ds;
        }

        /// <summary>
        /// 执行无返回的
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        public static int ExecuteProcessNoQuery(string ProcessName, SqlParameter[] sps)
        {
            int result = 0;
            try
            {
                OpenConnection();

                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.StoredProcedure;
                comd.CommandText = ProcessName;
                foreach (SqlParameter sp in sps)
                    comd.Parameters.Add(sp);
                result = comd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        public static int ExecuteProcessNoQuery(string ProcessName, ref SqlParameter[] sps)
        {
            int result = 0;
            try
            {
                OpenConnection();

                SqlCommand comd = new SqlCommand();
                comd.Connection = sqlConnection;
                comd.CommandType = CommandType.StoredProcedure;
                comd.CommandText = ProcessName;
                foreach (SqlParameter sp in sps)
                    comd.Parameters.Add(sp);
                result = comd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        public static void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }

        private static void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
        }
    }
}

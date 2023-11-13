using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LabSystem.DAL
{
    public class SqlHelper
    {
        //private static readonly string connStr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        private static readonly string connStr = new DataControl().strCon;

        #region 断开式操作
        public static DataTable GetDataTable(string sql, CommandType type,params SqlParameter[] pars)
        {
            using(SqlConnection conn =new SqlConnection(connStr))
            {
                using(SqlDataAdapter apter =new SqlDataAdapter(sql, conn))
                {
                    if (pars != null)
                    {
                        apter.SelectCommand.Parameters.AddRange(pars);
                      
                    }
                    apter.SelectCommand.CommandType = type;
                    DataTable da = new DataTable();
                    apter.Fill(da);
                    return da;
                }
            }
        }

        public static DataTable GetList(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    DataTable da = new DataTable();
                    adapter.Fill(da);
                    return da;
                }
                  
            }

        }
        public static DataTable GetList(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.SelectCommand.Parameters.AddRange(ps);
                    DataTable da = new DataTable();
                    adapter.Fill(da);
                    return da;
                }
                   
            }
        }
        #endregion


        #region Command
        /// <summary>
        /// 数据库增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int ExecuteNonquery(string sql,CommandType type, params SqlParameter [] pars)
        {
            using (SqlConnection conn =new SqlConnection(connStr))
            {
                using(SqlCommand cmd =new SqlCommand(sql, conn))
                {
                    if (pars != null)
                    {
                        cmd.Parameters.AddRange(pars);
                    }
                    cmd.CommandType = type;
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static object ExcuteScalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(ps);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 获取结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] ps)
        {
            return ExecuteReader(sql, CommandType.Text, ps);
        }
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(ps);
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
           }
               
        }

        #endregion
    }
}

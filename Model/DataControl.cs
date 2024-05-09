using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using TaskManager;

public class DataControl
{
    //private const int MaxPool = 32766;//最大连接数
    //private const int MinPool = 10;//最小连接数
    //private const bool Asyn_Process = true;//设置异步访问数据库       
    public static int ConnectTimeout = 2;//设置连接等待时间
    //private const int Conn_Lifetime = 15;//设置连接生命周期
    public string strCon; //连接字符串  

    public string ServerIP;
    public string IniPath;
    public string BlobServer;

    #region API函数声明

    [DllImport("kernel32")]//返回0表示失败，非0为成功
    public static extern int WritePrivateProfileString(string section, string key,
        string val, string filePath);

    [DllImport("kernel32")]//返回取得字符串缓冲区的长度
    public static extern int GetPrivateProfileString(string section, string key,
        string def, StringBuilder retVal, int size, string filePath);
    #endregion

    public DataControl()
    {
        IniPath = AppDomain.CurrentDomain.BaseDirectory + "constring.ini";
        var server = new StringBuilder(1024);
        var pwd = new StringBuilder(1024);
        var blobServer = new StringBuilder(1024);
        GetPrivateProfileString("a", "server ", "192.168.1.35", server, 1024, IniPath);
        GetPrivateProfileString("a", "pwd ", "7100196", pwd, 1024, IniPath);
        GetPrivateProfileString("a", "blobServer ", "10.12.48.30", blobServer, 1024, IniPath);
        if (server.ToString().Contains(","))
        {
            ServerIP = server.ToString().Split(',')[0];
        }
        else
        {
            ServerIP = server.ToString();
        }
        BlobServer = blobServer.ToString();

        
        strCon = $"server={server};database=NewTaskManager;uid=sa;pwd={pwd};Pooling=true; max pool size=32765;min pool size=0;Asynchronous Processing=true;";
      


        if (!strCon.EndsWith(";"))
            strCon += ";";
        if (!strCon.Contains("Connect Timeout"))
            strCon += "Connect Timeout=" + ConnectTimeout;
    }

    public bool TestConnection(int timeout, out string errorInfo, string server = null, string pwd = null)
    {
        var result = true;
        
        try
        {
            if (server == null || pwd == null)
            {
                using (var SqlConn = new SqlConnection())
                {
                    var temp = strCon.Replace("Connect Timeout=" + ConnectTimeout, "Connect Timeout=" + timeout);
                    SqlConn.ConnectionString = temp;
                    SqlConn.Open();
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }
            else
            {
                using (var SqlConn = new SqlConnection())
                {
                    var temp = $"server={server};database=NewTaskManager;uid=sa;pwd={pwd};" +
                               "Pooling=true; max pool size=1000;min pool size=0;Asynchronous Processing=true;";
                    SqlConn.ConnectionString = temp;
                    SqlConn.Open();
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }

            errorInfo = "";
        }
        catch (Exception ex)
        {
            errorInfo = ex.ToString();
            result = false;
        }
        return result;
    }
    
    /// <summary>
    /// DataTable导入数据库目标表
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="targetTableName"></param>
    /// <param name="sourceCols">dt中列名</param>
    /// <param name="targetCols">目标表中的列名</param>
    public void DataTable2SqlServer(DataTable dt, string targetTableName, String[] sourceCols, String[] targetCols)
    {
        if (sourceCols.Length != targetCols.Length) return;
        if (sourceCols.Length == 0) return;

        using (var sqlRevdBulkCopy = new SqlBulkCopy(strCon))
        {
            sqlRevdBulkCopy.DestinationTableName = targetTableName;
            sqlRevdBulkCopy.NotifyAfter = dt.Rows.Count;//有几行数据 

            for (int i = 0; i < sourceCols.Length; i++)
                sqlRevdBulkCopy.ColumnMappings.Add(sourceCols[i], targetCols[i]);

            sqlRevdBulkCopy.WriteToServer(dt);//数据导入数据库 
            sqlRevdBulkCopy.Close();//关闭连接  
        }
    }

    /// <summary>
    /// DataTable导入数据库目标表，包括DataTable的所有列，除了ID
    /// </summary>
    /// <param name="dt">数据源</param>
    /// <param name="targetTableName">目标表名称</param>
    public void DataTable2SqlServer(DataTable dt, string targetTableName)
    {
        using (var sqlRevdBulkCopy = new SqlBulkCopy(strCon))
        {
            sqlRevdBulkCopy.DestinationTableName = targetTableName;
            sqlRevdBulkCopy.NotifyAfter = dt.Rows.Count;//有几行数据 
            for (int i = 0; i < dt.Columns.Count; i++)//定义列
            {
                string col_name = dt.Columns[i].ColumnName;
                if (col_name.Equals("ID")) continue;
                sqlRevdBulkCopy.ColumnMappings.Add(col_name, col_name);
            }

            sqlRevdBulkCopy.WriteToServer(dt);//数据导入数据库 
            sqlRevdBulkCopy.Close();//关闭连接  
        }
    }
    
    public DataSet ExecuteHighCostQuery(string strSql)
    {
        DataSet newDataSet;
        using (var SqlConn = new SqlConnection())
        {
            SqlConn.ConnectionString = strCon;
            SqlConn.Open();

            var dataAdapter = new SqlDataAdapter(strSql, SqlConn);
            dataAdapter.SelectCommand.CommandTimeout = 60*15;
            newDataSet = new DataSet();
            dataAdapter.Fill(newDataSet);

            SqlConn.Close();
            SqlConn.Dispose();
        }
        return newDataSet;
    }

    public DataSet ExecuteQuery(string strSql)
    {
        DataSet newDataSet;
        using (var SqlConn = new SqlConnection())
        {
            SqlConn.ConnectionString = strCon;
            SqlConn.Open();

            var dataAdapter = new SqlDataAdapter(strSql, SqlConn);
            newDataSet = new DataSet();
            dataAdapter.Fill(newDataSet);

            SqlConn.Close();
            SqlConn.Dispose();
        }
        return newDataSet;
    }

    public DataSet ExecuteQuery(string strsql, SqlParameter[] parameterValues)
    {
        DataSet ds;
        using (var conn = new SqlConnection(strCon))
        {
            conn.Open();
            // Invoke RegionUpdate Procedure
            SqlCommand cmd = new SqlCommand(strsql, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            foreach (SqlParameter p in parameterValues)
            {
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                cmd.Parameters.Add(p);
            }

            var dataAdapter = new SqlDataAdapter {SelectCommand = cmd};
            ds = new DataSet();
            dataAdapter.Fill(ds);

            conn.Close();
            conn.Dispose();
        }
        return ds;
    }

    public DataSet ExecuteProcedureQuery(string strsql, SqlParameter[] parameterValues)
    {
        SqlDataAdapter dataAdapter;
        DataSet ds;
        using (SqlConnection conn = new SqlConnection(strCon))
        {
            conn.Open();
            // Invoke RegionUpdate Procedure
            SqlCommand cmd = new SqlCommand(strsql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            foreach (SqlParameter p in parameterValues)
            {
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                cmd.Parameters.Add(p);
            }
            dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            ds = new DataSet();
            dataAdapter.Fill(ds);

            conn.Close();
            conn.Dispose();
        }
        return ds;
    }

    #region 执行没有返回的数据库操作

    /// <summary>
    /// 执行没有返回的数据库操作
    /// </summary>
    /// <param name="strsql"></param>
    /// <param name="parameterValues"></param>
    public void ExecuteNonQuery(string strsql, SqlParameter[] parameterValues)
    {
        using (var conn = new SqlConnection(strCon))
        {
            conn.Open();
            // Invoke RegionUpdate Procedure
            var cmd = new SqlCommand(strsql, conn)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 0
            };
            foreach (var p in parameterValues)
            {
                if (p.Direction == ParameterDirection.InputOutput && p.Value == null)
                {
                    p.Value = DBNull.Value;
                }

                cmd.Parameters.Add(p);
            }
            cmd.ExecuteNonQuery();

            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// 执行没有返回的数据库操作
    /// </summary>
    /// <param name="strsql"></param>
    /// <param name="parameterValues"></param>
    public int ExecuteAddQuery(string strsql, SqlParameter[] parameterValues)
    {
        strsql = strsql + ";select SCOPE_IDENTITY()";
        int id = 0;
        using (var conn = new SqlConnection(strCon))
        {
            conn.Open();
            // Invoke RegionUpdate Procedure
            var cmd = new SqlCommand(strsql, conn)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 0
            };
            foreach (var p in parameterValues)
            {
                if (p.Direction == ParameterDirection.InputOutput && p.Value == null)
                {
                    p.Value = DBNull.Value;
                }

                cmd.Parameters.Add(p);
            }
            id = Convert.ToInt32(cmd.ExecuteScalar());

            conn.Close();
            conn.Dispose();
        }

        return id;
    }

    /// <summary>
    /// 执行没有返回的数据库操作
    /// </summary>
    /// <param name="strSql">语句</param>
    public void ExecuteNonQuery(string strSql)
    {
        using (var SqlConn = new SqlConnection())
        {
            SqlConn.ConnectionString = strCon;
            SqlConn.Open();
            var Comm = new SqlCommand(strSql, SqlConn);


            if (SqlConn.State == 0)
                SqlConn.Open();
            Comm.ExecuteNonQuery();
            Comm.Dispose();

            SqlConn.Close();
            SqlConn.Dispose();
        }
    }

    /// <summary>
    /// 获取最新插入数据的id
    /// </summary>
    public int SelectLastId()
    {
        int id;
        string strSql = @"select last_insert_rowid()";
        using (var SqlConn = new SqlConnection())
        {
            SqlConn.ConnectionString = strCon;
            SqlConn.Open();
            var Comm = new SqlCommand(strSql, SqlConn);


            if (SqlConn.State == 0)
                SqlConn.Open();
            id=(int)Comm.ExecuteScalar();
            Comm.Dispose();

            SqlConn.Close();
            SqlConn.Dispose();
        }

        return id;
    }

    #endregion

    #region 修改表的结构

    public void AddColumn(String table, String column, String type)
    {
        String strsql = "if (select name from sys.syscolumns where name='" + column + "' and id=OBJECT_ID('" + table + "')) is null begin";
        strsql += " \n alter table " + table + " ADD  " + column + " " + type;
        strsql += " \n end";
        ExecuteNonQuery(strsql);
    }

    #endregion

    #region 获得数据

    public List<string> GetStringList(string strsql)
    {
        var dt = ExecuteQuery(strsql).Tables[0];

        if (dt.Rows.Count == 0)
            return new List<string>();

        var list = new List<string>();
        foreach (DataRow row in dt.Rows)
        {
            list.Add(row[0].ToString());
        }

        return list;

    }

    public int GetExpr1(string strsql,int defaultValue)
    {
        DataTable dt = ExecuteQuery(strsql).Tables[0];

        if (dt.Rows.Count == 0) return defaultValue;

        return int.TryParse(dt.Rows[0][0].ToString(), out var result) ? result : defaultValue;
    }

    public double GetExpr1(string strsql, double defaultValue)
    {
        var dt = ExecuteQuery(strsql).Tables[0];

        if (dt.Rows.Count == 0) return defaultValue;

        return double.TryParse(dt.Rows[0][0].ToString(), out var result) ? result : defaultValue;
    }

    public string GetExpr1(string strsql, string defaultValue)
    {
        DataTable dt = ExecuteQuery(strsql).Tables[0];

        if (dt.Rows.Count == 0) return defaultValue;
        else return dt.Rows[0][0].ToString();
        
    }

    public byte[] GetExpr1(string strsql)
    {
        DataTable dt = ExecuteQuery(strsql).Tables[0];

        if (dt.Rows.Count == 0) return null;
        if (dt.Columns.Contains("Expre1"))
            return (byte[])dt.Rows[0]["Expr1"];
        else
            return (byte[])dt.Rows[0][0];
    }

    #endregion

    #region 其他

    /// <summary>
    /// 权限检查
    /// </summary>
    /// <param name="module"></param>
    /// <param name="operate"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    internal bool AuthorityCheck2(string module, string operate, string userName = null)
    {
        if (string.IsNullOrWhiteSpace(userName))
            userName = FormSignIn.CurrentUser.Name;

        var strsql = "select COUNT(*) from AuthorityTable2 where module = '系统维护' and operate='超级管理' " +
                     $"and userNames like '%{userName}%'";

        if (GetExpr1(strsql, 0) > 0)
            return true;

        strsql = "select COUNT(*) from AuthorityTable2 ";

        if (!string.IsNullOrWhiteSpace(operate))
        {
            if (string.IsNullOrWhiteSpace(module))
                strsql += $" where  operate='{operate}' and userNames like '%{userName}%'";
            else
                strsql += $"where module = '{module}' and operate='{operate}' and userNames like '%{userName}%'";

            return GetExpr1(strsql, 0) > 0;
        }

        return true;
    }

    /// <summary>
    /// 以默认公共的排序来初始化Field定义
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public List<DataField> InitDataFields(FormTable table)
    {
        var fields = new List<DataField>();
        var strsql = $"select * from FieldDefinitionTable where category='{table.Category}' order by tableIndex";
        var dt = ExecuteQuery(strsql).Tables[0];
        foreach (DataRow row in dt.Rows)
        {
            fields.Add(new DataField(row, table));
        }

        return fields;
    }


    #endregion
}

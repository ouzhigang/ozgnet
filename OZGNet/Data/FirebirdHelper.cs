﻿using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Data
{
    /// <summary>
    /// Firebird访问类,实现IDataHelper接口
    /// </summary>
    public class FirebirdHelper : IDataHelper
    {
        private string _connstring;
        private FbConnection conn;
        /// <summary>
        /// 获取当前执行的Command(ExecuteNonQueryTransaction()除外)
        /// </summary>
        public FbCommand Cmd;
        /// <summary>
        /// 获取当前执行的ExecuteNonQueryTransaction()所有Command对象
        /// </summary>
        public IList<FbCommand> Cmds;
        private FbDataReader reader;
        private FbDataAdapter da;
        private DataSet dataset;

        #region 构造函数
        /// <summary>
        /// 实例化FirebirdHelper
        /// </summary>
        public FirebirdHelper()
        { }
        /// <summary>
        /// 实例化FirebirdHelper
        /// </summary>
        /// <param name="connstring">连接字符</param>
        public FirebirdHelper(string connstring)
        {
            this._connstring = connstring;
        }
        /// <summary>
        /// 实例化FirebirdHelper
        /// </summary>
        /// <param name="fbStringBuilder">FbConnectionStringBuilder实例</param>
        public FirebirdHelper(FbConnectionStringBuilder fbStringBuilder)
        {
            this._connstring = fbStringBuilder.ConnectionString;
        }

        /// <summary>
        /// 连接数据库可以是本地连接，也可以远程连接
        /// </summary>
        /// <param name="server">数据库服务器，本地则为localhost</param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="db">数据库所在主机的本地路径</param>
        public FirebirdHelper(string server, string userName, string passWord, string db)
        {
            FbConnectionStringBuilder fbStringBuilder = new FbConnectionStringBuilder();
            fbStringBuilder.Database = db;
            fbStringBuilder.UserID = userName;
            fbStringBuilder.Password = passWord;
            fbStringBuilder.DataSource = server;
            this._connstring = fbStringBuilder.ConnectionString;
        }

        /// <summary>
        /// 连接数据库可以是本地连接，也可以远程连接，且使用连接池
        /// </summary>
        /// <param name="server">数据库服务器，本地则为localhost</param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="db">数据库所在主机的本地路径</param>
        /// <param name="minPool"></param>
        /// <param name="maxPool"></param>
        public FirebirdHelper(string server, string userName, string passWord, string db, int minPool, int maxPool)
        {
            if (maxPool <= minPool)
            {
                throw new Exception("maxPool不能等于或小于minPool");
            }

            FbConnectionStringBuilder fbStringBuilder = new FbConnectionStringBuilder();
            fbStringBuilder.Database = db;
            fbStringBuilder.UserID = userName;
            fbStringBuilder.Password = passWord;
            fbStringBuilder.DataSource = server;
            fbStringBuilder.Pooling = true;
            fbStringBuilder.MinPoolSize = minPool;
            fbStringBuilder.MaxPoolSize = maxPool;
            this._connstring = fbStringBuilder.ConnectionString;
        } 
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置连接字符
        /// </summary>
        public string ConnString
        {
            get
            {
                return this._connstring;
            }
            set
            {
                this._connstring = value;
            }
        } 
        #endregion

        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public object ExecuteScalar(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            using (conn = new FbConnection())
            {
                PrepareCommand(cmd_type, sql, parameters);
                object obj = Cmd.ExecuteScalar();
                Cmd.Dispose();
                return obj;
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 执行SQL语句或存储过程
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            using (conn = new FbConnection())
            {
                int i;
                PrepareCommand(cmd_type, sql, parameters);
                try
                {
                    i = Cmd.ExecuteNonQuery();
                }
                catch
                {
                    i = 0;
                }
                Cmd.Dispose();
                return i;
            }
        }

        /// <summary>
        /// 执行SQL语句(事务)
        /// </summary>
        /// <param name="sqls">SQL语句(数组)</param>
        public bool ExecuteNonQueryTransaction(IList<string> sqls)
        {
            using (conn = new FbConnection())
            {
                conn.ConnectionString = this._connstring;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                FbTransaction st = conn.BeginTransaction();

                int sql_count = sqls.Count;

                //先用泛型动态获取FbCommand
                Cmds = new List<FbCommand>();
                for (int i = 0; i < sql_count; i++)
                {
                    Cmd = new FbCommand(sqls[i], conn);
                    Cmd.Transaction = st;
                    Cmds.Add(Cmd);
                }

                //执行返型里面的所有SQL语句,如果出现错误,就马上回滚事务,回滚事务isOK则返回false
                bool isOK;
                try
                {
                    for (int i = 0; i < sql_count; i++)
                    {
                        Cmds[i].ExecuteNonQuery();
                        Cmds[i].Dispose();
                    }
                    st.Commit();
                    isOK = true;
                }
                catch
                {
                    st.Rollback();
                    isOK = false;
                }
                st.Dispose();
                return isOK;
            }
        }
        /// <summary>
        /// 返回单行数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataRow SingleRow(string sql)
        {
            return SingleRow(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 返回单行数据
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataRow SingleRow(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            DataTable dt = DataSet(cmd_type, sql, "SingleRowTable", parameters).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }
        /// <summary>
        /// 返回DataRead
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 返回DataRead，（需要Close()Dispose()来释放资源）
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            conn = new FbConnection();
            PrepareCommand(cmd_type, sql, parameters);
            reader = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataTable DataTable(string sql)
        {
            return DataTable(CommandType.Text, sql, null);
        }
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataTable DataTable(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            return DataSet(cmd_type, sql, parameters).Tables[0];
        }
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataSet DataSet(string sql)
        {
            return DataSet(CommandType.Text, sql, "SingleTable", null);
        }
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataSet DataSet(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            return DataSet(cmd_type, sql, "SingleTable", parameters);
        }
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <returns></returns>
        public DataSet DataSet(string sql, string tableName)
        {
            return DataSet(CommandType.Text, sql, tableName, null);
        }
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataSet DataSet(CommandType cmd_type, string sql, string tableName, IList<IDataParameter> parameters)
        {
            using (conn = new FbConnection())
            {
                PrepareCommand(cmd_type, sql, parameters);
                da = new FbDataAdapter();
                da.SelectCommand = Cmd;
                dataset = new DataSet();
                da.Fill(dataset, tableName);
                da.Dispose();
                Cmd.Dispose();
                return dataset;
            }
        }
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public DataSet DataSet(string sql, int pageIndex, int pageSize)
        {
            return DataSet(CommandType.Text, sql, "SingleTable", null, pageIndex, pageSize);
        }
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public DataSet DataSet(CommandType cmd_type, string sql, IList<IDataParameter> parameters, int pageIndex, int pageSize)
        {
            return DataSet(cmd_type, sql, "SingleTable", parameters, pageIndex, pageSize);
        }
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public DataSet DataSet(string sql, string tableName, int pageIndex, int pageSize)
        {
            return DataSet(CommandType.Text, sql, tableName, null, pageIndex, pageSize);
        }
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public DataSet DataSet(CommandType cmd_type, string sql, string tableName, IList<IDataParameter> parameters, int pageIndex, int pageSize)
        {
            using (conn = new FbConnection())
            {
                PrepareCommand(cmd_type, sql, parameters);
                da = new FbDataAdapter();
                da.SelectCommand = Cmd;
                dataset = new DataSet();
                if (cmd_type == CommandType.StoredProcedure)
                {
                    da.Fill(dataset, tableName);
                }
                else
                {
                    da.Fill(dataset, (pageIndex - 1) * pageSize, pageSize, tableName);
                }
                da.Dispose();
                Cmd.Dispose();
                return dataset;
            }
        }

        /// <summary>
        /// 辅助方法
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">命令参数</param>
        protected void PrepareCommand(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            //conn = new FbConnection();
            conn.ConnectionString = this._connstring;
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (FbException ex)
                {
                    throw ex;
                }
                catch (InvalidOperationException ex)
                {
                    throw ex;
                }
            }
            Cmd = new FbCommand(sql, conn);
            Cmd.CommandType = cmd_type;
            if (parameters != null)
            {
                foreach (IDataParameter sp in parameters)
                {
                    if (sp is FbParameter)
                    {
                        Cmd.Parameters.Add(sp);
                    }
                    else
                    {
                        throw new Exception("参数IDataParameter中含有非FbParameter的对象");
                    }
                }
            }
        }

        /// <summary>
        /// 获取一个数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return new FbConnection(this._connstring);
        }

    }
}

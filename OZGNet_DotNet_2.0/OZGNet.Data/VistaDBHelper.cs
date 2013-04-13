using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using VistaDB.Provider;

namespace OZGNet.Data
{
    /// <summary>
    /// VistaDB专用(依赖AspNetPager.dll),实现IDataHelper接口
    /// </summary>
    public class VistaDBHelper : IDataHelper
    {private string _connstring;
        private VistaDBConnection conn;
        /// <summary>
        /// 获取当前执行的Command(ExecuteNonQueryTransaction()除外)
        /// </summary>
        public VistaDBCommand Cmd;
        /// <summary>
        /// 获取当前执行的ExecuteNonQueryTransaction()所有Command对象
        /// </summary>
        public IList<VistaDBCommand> Cmds;
        private VistaDBDataReader reader;
        private VistaDBDataAdapter da;
        private DataSet dataset;

        /// <summary>
        /// 实例化VistaDBHelper
        /// </summary>
        public VistaDBHelper()
        { }

        /// <summary>
        /// 实例化VistaDBHelper
        /// </summary>
        /// <param name="connstring">连接字符</param>
        public VistaDBHelper(string connstring)
        {            
            this._connstring = connstring;
        }

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
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public object ExecuteScalar(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            using (conn = new VistaDBConnection())
            {
                PrepareCommand(cmd_type, sql, parameters);
                object obj = Cmd.ExecuteScalar();
                Cmd.Dispose();
                return obj;
            }
        }
        /// <summary>
        /// 执行SQL语句或存储过程
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
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            using (conn = new VistaDBConnection())
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
            using (conn = new VistaDBConnection())
            {
                conn.ConnectionString = this._connstring;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                VistaDBTransaction st = conn.BeginTransaction();

                int sql_count = sqls.Count;

                //先用泛型动态获取VistaDBCommand
                Cmds = new List<VistaDBCommand>();
                for (int i = 0; i < sql_count; i++)
                {
                    Cmd = new VistaDBCommand(sqls[i], conn);
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
        /// <param name="sql">SQL语句或存储过程</param>
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
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            conn = new VistaDBConnection();
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
        /// <param name="sql">SQL语句或存储过程</param>
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
        /// <param name="sql">SQL语句或存储过程</param>
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
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataSet DataSet(CommandType cmd_type, string sql, string tableName, IList<IDataParameter> parameters)
        {
            using (conn = new VistaDBConnection())
            {
                PrepareCommand(cmd_type, sql, parameters);
                da = new VistaDBDataAdapter();
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
        /// <param name="sql">SQL语句或存储过程</param>
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
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public DataSet DataSet(CommandType cmd_type, string sql, string tableName, IList<IDataParameter> parameters, int pageIndex, int pageSize)
        {
            using (conn = new VistaDBConnection())
            {
                PrepareCommand(cmd_type, sql, parameters);
                da = new VistaDBDataAdapter();
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
        /// AspNetPager分页
        /// </summary>
        /// <param name="pager">AspNetPager分页控件</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataSet AspNetPager(Wuqi.Webdiyer.AspNetPager pager, string sql)
        {
            return AspNetPager(CommandType.Text, pager, sql, "SingleTable", null);
        }
        /// <summary>
        /// AspNetPager分页
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="pager">AspNetPager分页控件</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataSet AspNetPager(CommandType cmd_type, Wuqi.Webdiyer.AspNetPager pager, string sql, IList<IDataParameter> parameters)
        {
            return DataSet(cmd_type, sql, parameters, pager.CurrentPageIndex, pager.PageSize);
        }
        /// <summary>
        /// AspNetPager分页
        /// </summary>
        /// <param name="pager">AspNetPager分页控件</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <returns></returns>
        public DataSet AspNetPager(Wuqi.Webdiyer.AspNetPager pager, string sql, string tableName)
        {
            return AspNetPager(CommandType.Text, pager, sql, tableName, null);
        }
        /// <summary>
        /// AspNetPager分页
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="pager">AspNetPager分页控件</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        public DataSet AspNetPager(CommandType cmd_type, Wuqi.Webdiyer.AspNetPager pager, string sql, string tableName, IList<IDataParameter> parameters)
        {
            return DataSet(cmd_type, sql, tableName, parameters, pager.CurrentPageIndex, pager.PageSize);
        }

        /// <summary>
        /// AspNetPager分页(存储过程DataReaer方式)
        /// </summary>
        /// <param name="pager">AspNetPager分页控件</param>
        /// <param name="sql">存储过程</param>
        /// <returns></returns>
        public IDataReader AspNetPagerProcedure(Wuqi.Webdiyer.AspNetPager pager, string sql)
        {
            List<IDataParameter> parameters = new List<IDataParameter>();
            parameters.Add(new VistaDBParameter("@pageindex", pager.CurrentPageIndex));
            parameters.Add(new VistaDBParameter("@pagesize", pager.PageSize));
            parameters.Add(new VistaDBParameter("@docount", "0"));
            return ExecuteReader(CommandType.StoredProcedure, sql, parameters);
        }

        /// <summary>
        /// 辅助方法
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        protected void PrepareCommand(CommandType cmd_type, string sql, IList<IDataParameter> parameters)
        {
            //conn = new VistaDBConnection();
            conn.ConnectionString = this._connstring;
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (VistaDB.Diagnostic.VistaDBException ex)
                {
                    throw ex;
                }
                catch (InvalidOperationException ex)
                {
                    throw ex;
                }
            }
            Cmd = new VistaDBCommand(sql, conn);
            Cmd.CommandType = cmd_type;
            if (parameters != null)
            {
                foreach (IDataParameter sp in parameters)
                {
                    if (sp is VistaDBParameter)
                    {
                        Cmd.Parameters.Add(sp);
                    }
                    else
                    {
                        throw new Exception("参数IDataParameter中含有非VistaDBParameter的对象");
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
            return new VistaDBConnection(this._connstring);
        }

        ///// <summary>
        ///// 关闭数据库
        ///// </summary>
        //public void Close()
        //{
        //    if (conn.State != ConnectionState.Closed)
        //    {
        //        try
        //        {
        //            conn.Close();
        //        }
        //        catch { }
        //    }
        //    conn.Dispose();
        //}
    }
}

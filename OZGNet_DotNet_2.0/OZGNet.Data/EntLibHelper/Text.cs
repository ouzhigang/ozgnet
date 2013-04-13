using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace OZGNet.Data.EntLibHelper
{
    /// <summary>
    /// EntLib访问类（SQL语句的方式）
    /// </summary>
    public class Text
    {
        private string _name;
        private Options.SelectDB selectDB = Options.SelectDB.SqlClient;
        Database db;

        #region 构造函数
        /// <summary>
        /// 实例化Text
        /// </summary>
        public Text()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 实例化Text
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="selectDB">选择数据库种类</param>
        public Text(string name, Options.SelectDB selectDB)
        {
            this.selectDB = selectDB;
            db = DatabaseFactory.CreateDatabase(name);
        }
        #endregion

        #region 相关属性
        /// <summary>
        /// 获取或设置数据库类型
        /// </summary>
        public Options.SelectDB SelectDB
        {
            get
            {
                return this.selectDB;
            }
            set
            {
                this.selectDB = value;
            }
        }
        /// <summary>
        /// 获取或设置数据库名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                db = DatabaseFactory.CreateDatabase(value);
                this._name = value;
            }
        } 
        #endregion



        #region 返回第一行第一列
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteScalar(string sql)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            return (int)db.ExecuteScalar(dbCommand);
        }
        #endregion

        #region 执行SQL语句
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            return db.ExecuteNonQuery(dbCommand);
        }
        #endregion

        #region 执行SQL语句(事务)
        /// <summary>
        /// 执行SQL语句(事务)
        /// </summary>
        /// <param name="SQLStringList">SQL语句(数组)</param>
        public bool ExecuteNonQueryTransaction(ArrayList SQLStringList)
        {
            bool isOK = false;
            using (DbConnection dbconn = db.CreateConnection())
            {

                dbconn.Open();
                DbTransaction dbtran = dbconn.BeginTransaction();
                try
                {
                    //执行语句

                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            DbCommand dbCommand = db.GetSqlStringCommand(strsql);
                            db.ExecuteNonQuery(dbCommand);
                        }
                    }
                    //执行存储过程

                    //db.ExecuteNonQuery(CommandType.StoredProcedure, "InserOrders");

                    //db.ExecuteDataSet(CommandType.StoredProcedure, "UpdateProducts");
                    isOK = true;
                    dbtran.Commit();
                }
                catch
                {
                    isOK = false;
                    dbtran.Rollback();
                }

                dbconn.Close();
                dbconn.Dispose();
                return isOK;
            }
        }
        #endregion

        #region 返回DataReader ( 注意：使用后一定要对DataReader进行Close)
        /// <summary>
        /// 返回DataReader ( 注意：使用后一定要对DataReader进行Close)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            return db.ExecuteReader(dbCommand);
        }
        #endregion

        #region 返回DataSet
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet</returns>
        public DataSet DataSet(string sql)
        {
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(dbCommand);
        }
        #endregion

        #region 返回DataSet(分页)
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
            DataSet ds = new DataSet();
            DbConnection dbconn = db.CreateConnection();

            if ((int)SelectDB == 1)
            {
                SqlConnection conn = new SqlConnection(dbconn.ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, tableName);
            }
            else if ((int)SelectDB == 2)
            {
                OleDbConnection conn = new OleDbConnection(dbconn.ConnectionString);
                OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, tableName);
            }
            else if ((int)SelectDB == 3)
            {
                OdbcConnection conn = new OdbcConnection(dbconn.ConnectionString);
                OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, tableName);
            }
            else if ((int)SelectDB == 4)
            {
                OracleConnection conn = new OracleConnection(dbconn.ConnectionString);
                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, tableName);
            }

            return ds;
        }
        #endregion

        #region AspNetPager分页
        /// <summary>
        /// AspNetPager分页
        /// </summary>
        /// <param name="pager">AspNetPager分页控件</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <returns></returns>
        public DataSet AspNetPager(Wuqi.Webdiyer.AspNetPager pager, string sql, string tableName)
        {
            return DataSet(sql, tableName, pager.CurrentPageIndex, pager.PageSize);
        } 
        #endregion

        #region 返回单行数据
        /// <summary>
        /// 返回单行数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataRow SingleRow(string sql)
        {
            DataTable dt = DataSet(sql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        } 
        #endregion

    }
}

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
    /// EntLib访问类（存储过程的方式），Hashtable ParametersList，Key为IDataParameter,Value为DBType
    /// </summary>
    public class StoredProcedure
    {       
        private string _name;
        private Options.SelectDB selectDB = Options.SelectDB.SqlClient;
        Database db;
        /// <summary>
        /// 获取当前执行的Command
        /// </summary>
        public DbCommand DbCommand;

        #region 构造函数
        /// <summary>
        /// 实例化StoredProcedure
        /// </summary>
        public StoredProcedure()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 实例化StoredProcedure
        /// </summary>
        /// <param name="name">数据库名称</param>
        /// <param name="selectDB">选择数据库种类</param>
        public StoredProcedure(string name, Options.SelectDB selectDB)
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
        /// <param name="storedProcedureName">存储过程</param>
        /// <param name="ParametersList">Value为DBType,Value为IDataParameter</param>
        /// <returns></returns>
        public int ExecuteScalar(string storedProcedureName, Hashtable ParametersList)
        {
            DbCommand = db.GetStoredProcCommand(storedProcedureName);
            AddParameters(DbCommand, ParametersList);   
            return (int)db.ExecuteScalar(DbCommand);
        }
        #endregion

        #region 执行存储过程
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcedureName">存储过程</param>
        /// <param name="ParametersList">Value为DBType,Value为IDataParameter</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string storedProcedureName, Hashtable ParametersList)
        {
            DbCommand = db.GetStoredProcCommand(storedProcedureName);
            AddParameters(DbCommand, ParametersList);            
            return db.ExecuteNonQuery(DbCommand);
        }
        #endregion

        #region 返回DataReader ( 注意：使用后一定要对DataReader进行Close)
        /// <summary>
        /// 返回DataReader ( 注意：使用后一定要对DataReader进行Close)
        /// </summary>
        /// <param name="storedProcedureName">存储过程</param>
        /// <param name="ParametersList">Value为DBType,Value为IDataParameter</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string storedProcedureName, Hashtable ParametersList)
        {
            DbCommand = db.GetStoredProcCommand(storedProcedureName);
            AddParameters(DbCommand, ParametersList);            
            return db.ExecuteReader(DbCommand);
        }
        #endregion

        #region 返回DataSet
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="storedProcedureName">存储过程</param>
        /// <param name="ParametersList">Value为DBType,Value为IDataParameter</param>
        /// <returns>DataSet</returns>
        public DataSet DataSet(string storedProcedureName, Hashtable ParametersList)
        {
            DbCommand = db.GetStoredProcCommand(storedProcedureName);
            AddParameters(DbCommand, ParametersList);            
            return db.ExecuteDataSet(DbCommand);
        }
        #endregion        

        #region 返回单行数据
        /// <summary>
        /// 返回单行数据
        /// </summary>
        /// <param name="storedProcedureName">存储过程</param>
        /// <param name="ParametersList">Value为DBType,Value为IDataParameter</param>
        /// <returns></returns>
        public DataRow SingleRow(string storedProcedureName, Hashtable ParametersList)
        {
            DataTable dt = DataSet(storedProcedureName, ParametersList).Tables[0];
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

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dbCommand"></param>
        /// <param name="ParametersList"></param>
        protected void AddParameters(DbCommand _dbCommand, Hashtable ParametersList)
        {
            if (ParametersList != null)
            {
                foreach (DictionaryEntry de in ParametersList)
                {
                    IDataParameter Parameter = (IDataParameter)de.Key;
                    DbType type = (DbType)de.Value;
                    db.AddInParameter(_dbCommand, Parameter.ParameterName, type, Parameter.Value);
                }
            }
        } 
        #endregion

    }
}

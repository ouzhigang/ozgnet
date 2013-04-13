using System;
using System.Data;
using System.Collections;
using Spring.Data.NHibernate.Support;
using Spring.Data.Common;

namespace OZGNet.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class SpringSQLProvider : IDbProvider
    {
        #region IDbProvider 成员
        private string _connectionString = "";
        /// <summary>
        /// 获取或设置连接字符
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object CreateCommandBuilder()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter CreateDataAdapter()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbDataParameter CreateParameter()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string CreateParameterName(string name)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string CreateParameterNameForCollection(string name)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public IDbMetadata DbMetadata
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string ExtractError(Exception e)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool IsDataAccessException(Exception e)
        {
            return false;
        }
        #endregion
    }

}
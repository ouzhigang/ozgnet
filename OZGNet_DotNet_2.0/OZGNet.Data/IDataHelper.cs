using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace OZGNet.Data
{
    /// <summary>
    /// 数据访问类接口
    /// </summary>
    public interface IDataHelper
    {
        /// <summary>
        /// 获取或设置连接字符
        /// </summary>
        string ConnString
        {
            get;
            set;
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        object ExecuteScalar(string sql);
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        object ExecuteScalar(CommandType cmd_type, string sql, IList<IDataParameter> parameters);
        /// <summary>
        /// 执行SQL语句或存储过程
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql);
        /// <summary>
        /// 执行SQL语句或存储过程
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        int ExecuteNonQuery(CommandType cmd_type, string sql, IList<IDataParameter> parameters);
        /// <summary>
        /// 执行SQL语句(事务)
        /// </summary>
        /// <param name="sqls">SQL语句(数组)</param>
        /// <returns></returns>
        bool ExecuteNonQueryTransaction(IList<string> sqls);
        /// <summary>
        /// 返回单行数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        DataRow SingleRow(string sql);
        /// <summary>
        /// 返回单行数据
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        DataRow SingleRow(CommandType cmd_type, string sql, IList<IDataParameter> parameters);
        /// <summary>
        /// 返回DataRead
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sql);
        /// <summary>
        /// 返回DataRead
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        IDataReader ExecuteReader(CommandType cmd_type, string sql, IList<IDataParameter> parameters);
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        DataTable DataTable(string sql);
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        DataTable DataTable(CommandType cmd_type, string sql, IList<IDataParameter> parameters);
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        DataSet DataSet(string sql);
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        DataSet DataSet(CommandType cmd_type, string sql, IList<IDataParameter> parameters);
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <returns></returns>
        DataSet DataSet(string sql, string tableName);
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="parameters">命令参数</param>
        /// <returns></returns>
        DataSet DataSet(CommandType cmd_type, string sql, string tableName, IList<IDataParameter> parameters);
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        DataSet DataSet(string sql, int pageIndex, int pageSize);
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        DataSet DataSet(CommandType cmd_type, string sql, IList<IDataParameter> parameters, int pageIndex, int pageSize);
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表的名称</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        DataSet DataSet(string sql, string tableName, int pageIndex, int pageSize);
        /// <summary>
        /// 返回DataSet(分页)
        /// </summary>
        /// <param name="cmd_type">命令类型</param>
        /// <param name="sql">SQL语句或存储过程</param>
        /// <param name="tableName">命令参数</param>
        /// <param name="parameters">表的名称</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        DataSet DataSet(CommandType cmd_type, string sql, string tableName, IList<IDataParameter> parameters, int pageIndex, int pageSize);
        
        /// <summary>
        /// 获取一个数据库连接
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();
    }
}

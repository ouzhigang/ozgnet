using System;
using System.Data;

namespace OZGNet.Data.EntLibHelper.Options
{
    /// <summary>
    /// 数据库种类选择，针对EntLib
    /// </summary>
    public enum SelectDB : int
    {
        /// <summary>
        /// SqlClient
        /// </summary>
        SqlClient = 1,
        /// <summary>
        /// OleDb
        /// </summary>
        OleDb = 2,
        /// <summary>
        /// Odbc
        /// </summary>
        Odbc = 3,
        /// <summary>
        /// OracleClient
        /// </summary>
        OracleClient = 4
    }
}

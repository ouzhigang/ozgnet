using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;

namespace OZGNet.Data
{
    /// <summary>
    /// 数据库辅助方法类
    /// </summary>
    public class HelperMethod
    {
        #region 数据源转成Dictionary<string, object>
        /// <summary>
        /// DataRow转成Dictionary(string为列名,object为数据值)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DataRowToMap(DataRow dr)
        {
            Dictionary<string, object> dataMap = new Dictionary<string, object>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                dataMap.Add(column.ColumnName, dr[column.ColumnName]);
            }
            return dataMap;
        }

        /// <summary>
        /// DataSet转成Dictionary的IList(string为列名,object为数据值)
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static IList<Dictionary<string, object>> DataSetToMaps(DataSet dataSet)
        {
            return DataTableToMaps(dataSet.Tables[0]);
        }

        /// <summary>
        /// DataTable转成Dictionary的IList(string为列名,object为数据值)
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IList<Dictionary<string, object>> DataTableToMaps(DataTable dataTable)
        {
            return DataRowsToMaps(dataTable.Rows);
        }

        /// <summary>
        /// DataRows转成Dictionary的IList(string为列名,object为数据值)
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static IList<Dictionary<string, object>> DataRowsToMaps(DataRowCollection rows)
        {
            IList<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow item in rows)
            {
                list.Add(DataRowToMap(item));
            }
            return list;
        }

        /// <summary>
        /// IDataReader转成Dictionary的IList(string为列名,object为数据值),IDataReader记得要关闭！
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IList<Dictionary<string, object>> DataReaderToMaps(IDataReader dataReader)
        {
            IList<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            while (dataReader.Read())
            {
                Dictionary<string, object> map = new Dictionary<string, object>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    string name = dataReader.GetName(i);
                    map.Add(name, dataReader[name]);
                }
                list.Add(map);
            }

            return list;
        } 
        #endregion

        #region 数据源转换实体类通用模块
        /// <summary>
        /// DataRow转实体类(要约束每个列的类型，否则出错)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DataRowToModel(DataRow dr, Type type)
        {
            Object obj = Utility.ObjectInstance(type);
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (dr[property.Name] == DBNull.Value)
                {
                    property.SetValue(obj, null, null);
                }
                else
                {
                    try
                    {
                        property.SetValue(obj, dr[property.Name], null);
                    }
                    catch
                    {
                        //使用实体类来强制约束类型
                        property.SetValue(obj, Convert.ChangeType(dr[property.Name], property.PropertyType), null);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// DataTable转实体类列表(要约束每个列的类型，否则出错)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ArrayList DataTableToModels(DataTable dt, Type type)
        {
            ArrayList arr = new ArrayList();
            foreach (DataRow dr in dt.Rows)
            {
                arr.Add(DataRowToModel(dr, type));
            }
            return arr;
        }

        /// <summary>
        /// DataReader转实体类列表
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ArrayList DataReaderToModels(IDataReader dataReader, Type type)
        {
            ArrayList arr = new ArrayList();
            while (dataReader.Read())
            {
                Object obj = Utility.ObjectInstance(type);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (dataReader[property.Name] == DBNull.Value)
                    {
                        property.SetValue(obj, null, null);
                    }
                    else
                    {
                        try
                        {
                            property.SetValue(obj, dataReader[property.Name], null);
                        }
                        catch
                        {
                            //使用实体类来强制约束类型
                            property.SetValue(obj, Convert.ChangeType(dataReader[property.Name], property.PropertyType), null);
                        }
                    }
                }
                arr.Add(obj);
            }
            return arr;
        }
        #endregion

        #region 数据实体类压入Hashtable(用在SQL语句)
        /// <summary>
        /// 数据实体类压入Hashtable(用在SQL语句)
        /// </summary>
        /// <param name="model">数据实体类</param>
        /// <param name="PrimaryKey">实体类的主键,主要用于避免压入主键</param>
        /// <returns></returns>
        public static Hashtable PushModel(object model, string PrimaryKey)
        {
            List<string> list = new List<string>();
            list.Add(PrimaryKey);
            return PushModel(model, list);
        }
        /// <summary>
        /// 数据实体类压入Hashtable(用在SQL语句)
        /// </summary>
        /// <param name="model">数据实体类</param>
        /// <param name="PrimaryKeys">实体类的主键,主要用于避免压入主键或其他想忽略的字段</param>
        /// <returns></returns>
        public static Hashtable PushModel(object model, IList<string> PrimaryKeys)
        {
            //全部统一使用小写
            for (int i = 0; i < PrimaryKeys.Count; i++)
            {
                string s = PrimaryKeys[i].ToLower();
                PrimaryKeys[i] = s;
            }

            Hashtable ht = new Hashtable();
            if (model != null)
            {
                Type type = model.GetType();
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (!PrimaryKeys.Contains(property.Name.ToLower()))
                    {
                        object tmpObj = property.GetValue(model, null);
                        if (tmpObj != null)
                        {
                            //if (tmpObj.GetType().FullName.Contains("System.Int") || tmpObj.GetType().Name == "Double")
                            //{
                            //    if (Convert.ToInt16(tmpObj) != 0 || Convert.ToInt32(tmpObj) != 0 || Convert.ToInt64(tmpObj) != 0 || Convert.ToDouble(tmpObj) != 0)
                            //    {
                            //        ht.Add(property.Name, tmpObj);
                            //    }
                            //}
                            //else
                            //{
                            //    ht.Add(property.Name, tmpObj);
                            //}

                            if (tmpObj is bool)
                            {
                                //解决bool类型的问题
                                bool tmpVal = Convert.ToBoolean(tmpObj);
                                if (tmpVal)
                                {
                                    ht.Add("[" + property.Name + "]", 1);
                                }
                                else
                                {
                                    ht.Add("[" + property.Name + "]", 0);
                                }
                            }
                            else
                            {
                                if (tmpObj is string)
                                {
                                    //解决字符串的问题
                                    tmpObj = ReplaceSqlString(tmpObj.ToString());
                                }
                                else if (tmpObj is DateTime)
                                {
                                    //解决DateTime的问题
                                    tmpObj = tmpObj.ToString();
                                }
                                ht.Add("[" + property.Name + "]", tmpObj);
                            }
                        }
                    }
                }
            }
            return ht;
        } 
        #endregion

        #region  Sqlite专用数据实体类压入Hashtable(用在SQL语句)，主要是避免时间字段出错
        /// <summary>
        ///  Sqlite专用数据实体类压入Hashtable(用在SQL语句)，主要是避免时间字段出错
        /// </summary>
        /// <param name="model">数据实体类</param>
        /// <param name="PrimaryKey">实体类的主键,主要用于避免压入主键</param>
        /// <returns></returns>
        public static Hashtable PushModelForSqlite(object model, string PrimaryKey)
        {
            List<string> list = new List<string>();
            list.Add(PrimaryKey);
            return PushModelForSqlite(model, list);
        }

        /// <summary>
        /// Sqlite专用数据实体类压入Hashtable(用在SQL语句)，主要是避免时间字段出错
        /// </summary>
        /// <param name="model">数据实体类</param>
        /// <param name="PrimaryKeys">实体类的主键,主要用于避免压入主键或其他想忽略的字段</param>
        /// <returns></returns>
        public static Hashtable PushModelForSqlite(object model, IList<string> PrimaryKeys)
        {
            //全部统一使用小写
            for (int i = 0; i < PrimaryKeys.Count; i++)
            {
                string s = PrimaryKeys[i].ToLower();
                PrimaryKeys[i] = s;
            }

            Hashtable ht = new Hashtable();
            if (model != null)
            {
                Type type = model.GetType();
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (!PrimaryKeys.Contains(property.Name.ToLower()))
                    {
                        object tmpObj = property.GetValue(model, null);
                        if (tmpObj != null)
                        {
                            //if (tmpObj.GetType().FullName.Contains("System.Int") || tmpObj.GetType().Name == "Double")
                            //{
                            //    if (Convert.ToInt16(tmpObj) != 0 || Convert.ToInt32(tmpObj) != 0 || Convert.ToInt64(tmpObj) != 0 || Convert.ToDouble(tmpObj) != 0)
                            //    {
                            //        ht.Add(property.Name, tmpObj);
                            //    }
                            //}
                            //else
                            //{
                            //    ht.Add(property.Name, tmpObj);
                            //}

                            if (tmpObj is bool)
                            {
                                //解决bool类型的问题
                                bool tmpVal = Convert.ToBoolean(tmpObj);
                                if (tmpVal)
                                {
                                    ht.Add("[" + property.Name + "]", 1);
                                }
                                else
                                {
                                    ht.Add("[" + property.Name + "]", 0);
                                }
                            }
                            else
                            {
                                if (tmpObj is string)
                                {
                                    //解决字符串的问题
                                    tmpObj = ReplaceSqlString(tmpObj.ToString());
                                }
                                else if (tmpObj is DateTime)
                                {
                                    //解决DateTime的问题
                                    tmpObj = Convert.ToDateTime(tmpObj).ToString("s");
                                }
                                ht.Add("[" + property.Name + "]", tmpObj);
                            }
                        }
                    }
                }
            }
            return ht;
        } 
        #endregion

        #region 数据实体类压入List(用在SQL存储过程)
        /// <summary>
        /// 数据实体类压入List(用在SQL存储过程)
        /// </summary>
        /// <param name="model">数据实体类</param>
        /// <param name="PrimaryKey">实体类的主键,主要用于避免压入主键</param>
        /// <returns></returns>
        public static List<SqlParameter> PushSqlData(object model, string PrimaryKey)
        {
            List<string> list = new List<string>();
            list.Add(PrimaryKey);
            return PushSqlData(model, list);
        }
        /// <summary>
        /// 数据实体类压入List(用在SQL存储过程)
        /// </summary>
        /// <param name="model">数据实体类</param>
        /// <param name="PrimaryKeys">实体类的主键,主要用于避免压入主键或其他想忽略的字段</param>
        /// <returns></returns>
        public static List<SqlParameter> PushSqlData(object model, IList<string> PrimaryKeys)
        {
            //全部统一使用小写
            for (int i = 0; i < PrimaryKeys.Count; i++)
            {
                string s = PrimaryKeys[i].ToLower();
                PrimaryKeys[i] = s;
            }

            List<SqlParameter> list = new List<SqlParameter>();
            if (model != null)
            {
                Type type = model.GetType();
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (!PrimaryKeys.Contains(property.Name.ToLower()))
                    {
                        object tmpObj = property.GetValue(model, null);
                        if (tmpObj != null)
                        {
                            //if (tmpObj.GetType().FullName.Contains("System.Int") || tmpObj.GetType().Name == "Double")
                            //{
                            //    if (Convert.ToInt16(tmpObj) != 0 || Convert.ToInt32(tmpObj) != 0 || Convert.ToInt64(tmpObj) != 0 || Convert.ToDouble(tmpObj) != 0)
                            //    {
                            //        if (tmpObj != null && tmpObj.ToString() != "0")
                            //        {
                            //            list.Add(new SqlParameter("@" + property.Name, tmpObj));
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    if (tmpObj != null)
                            //    {
                            //        list.Add(new SqlParameter("@" + property.Name, tmpObj));
                            //    }
                            //}

                            list.Add(new SqlParameter("@" + property.Name, tmpObj));

                        }
                    }
                }
            }
            return list;
        } 
        #endregion
        
        #region 获取一个Odbc连接的所有表名称
        /// <summary>
        /// 获取一个Odbc连接的所有表名称
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string[] GetOdbcTableNames(string connString)
        {
            using (OdbcConnection conn = new OdbcConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();
                DataTable dt = conn.GetSchema("Tables");
                string[] TableNames = new string[dt.Rows.Count];
                for (int i = 0; i < TableNames.Length; i++)
                {
                    TableNames[i] = dt.Rows[i]["TABLE_NAME"].ToString();
                }
                return TableNames;
            }
        } 
        #endregion

        #region 获取一个OleDb连接的所有表名称
        /// <summary>
        /// 获取一个OleDb连接的所有表名称
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string[] GetOleDbTableNames(string connString)
        {
            using (OleDbConnection conn = new OleDbConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();
                DataTable dt = conn.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
                string[] TableNames = new string[dt.Rows.Count];
                for (int i = 0; i < TableNames.Length; i++)
                {
                    TableNames[i] = dt.Rows[i]["TABLE_NAME"].ToString();
                }
                return TableNames;
            }
        } 
        #endregion

        #region 获取一个Sql连接的所有表名称
        /// <summary>
        /// 获取一个Sql连接的所有表名称
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string[] GetSqlTableNames(string connString)
        {
            return GetSqlObjectNames(connString, "select * from sysobjects where xtype='u' and category!=2");
        }
        #endregion

        #region 获取一个Sql连接的所有存储过程名称
        /// <summary>
        /// 获取一个Sql连接的所有存储过程名称
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string[] GetSqlProcedureNames(string connString)
        {
            return GetSqlObjectNames(connString, "select * from sysobjects where xtype='p' and category!=2");
        } 
        #endregion

        #region 获取一个Sql连接的所有视图名称
        /// <summary>
        /// 获取一个Sql连接的所有视图名称
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string[] GetSqlViewNames(string connString)
        {
            return GetSqlObjectNames(connString, "select * from sysobjects where xtype='v' and category!=2");
        }
        #endregion

        #region 获取一个Sql连接的所有触发器名称
        /// <summary>
        /// 获取一个Sql连接的所有触发器名称
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string[] GetSqlTriggerNames(string connString)
        {
            return GetSqlObjectNames(connString, "select * from sysobjects where xtype='tr' and category!=2");
        }
        #endregion        

        #region 获取一个Sql连接的所有对象名称(私有方法)
        /// <summary>
        /// 获取一个Sql连接的所有对象名称(私有方法)
        /// </summary>
        /// <param name="connString">连接字符</param>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        protected static string[] GetSqlObjectNames(string connString, string sql)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connString;
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                conn.Open();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                string[] TableNames = new string[dt.Rows.Count];
                for (int i = 0; i < TableNames.Length; i++)
                {
                    TableNames[i] = dt.Rows[i]["NAME"].ToString();
                }
                return TableNames;
            }
        }
        #endregion
        
        #region 以SQL语句方式，直接获取Model
        /// <summary>
        /// 以SQL语句方式，直接获取单个Model
        /// </summary>
        /// <param name="dbHelper">IDataHelper实例</param>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="type">实体类的Type</param>
        /// <returns></returns>
        public static object GetModel(IDataHelper dbHelper, string sql, Type type)
        {
            DataRow dr = dbHelper.SingleRow(sql);
            object obj = DataRowToModel(dr, type);
            return obj;
        }
        /// <summary>
        /// 以SQL语句方式，直接获取Model的列表
        /// </summary>
        /// <param name="dbHelper">IDataHelper实例</param>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="type">实体类的Type</param>
        /// <returns></returns>
        public static ArrayList GetModels(IDataHelper dbHelper, string sql, Type type)
        {
            DataTable dt = dbHelper.DataTable(sql);
            ArrayList list = DataTableToModels(dt, type);
            return list;
        }
        /// <summary>
        /// 以SQL语句方式，直接获取Model的列表
        /// </summary>
        /// <param name="dbHelper">IDataHelper实例</param>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="type">实体类的Type</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public static ArrayList GetModels(IDataHelper dbHelper, string sql, Type type, int pageIndex, int pageSize)
        {
            DataTable dt = dbHelper.DataSet(sql, pageIndex, pageSize).Tables[0];
            ArrayList list = DataTableToModels(dt, type);
            return list;
        } 
        #endregion

        #region Access日文编码
        /// <summary>
        /// Access日文编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string JPEncode(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return null;
            }
            content = content.Replace("ガ", "#JPString01#");
            content = content.Replace("ギ", "#JPString02#");
            content = content.Replace("ア", "#JPString03#");
            content = content.Replace("ゲ", "#JPString04#");
            content = content.Replace("ゴ", "#JPString05#");
            content = content.Replace("ザ", "#JPString06#");
            content = content.Replace("ジ", "#JPString07#");
            content = content.Replace("ズ", "#JPString08#");
            content = content.Replace("ゼ", "#JPString09#");
            content = content.Replace("ゾ", "#JPString10#");
            content = content.Replace("ダ", "#JPString11#");
            content = content.Replace("ヂ", "#JPString12#");
            content = content.Replace("ヅ", "#JPString13#");
            content = content.Replace("デ", "#JPString14#");
            content = content.Replace("ド", "#JPString15#");
            content = content.Replace("バ", "#JPString16#");
            content = content.Replace("パ", "#JPString17#");
            content = content.Replace("ビ", "#JPString18#");
            content = content.Replace("ピ", "#JPString19#");
            content = content.Replace("ブ", "#JPString20#");
            content = content.Replace("プ", "#JPString21#");
            content = content.Replace("ベ", "#JPString22#");
            content = content.Replace("ペ", "#JPString23#");
            content = content.Replace("ボ", "#JPString24#");
            content = content.Replace("ポ", "#JPString25#");
            content = content.Replace("ヴ", "#JPString26#");
            content = content.Replace("ン", "#JPString27#");
            content = content.Replace("ム", "#JPString28#");
            return content;
        }
        #endregion

        #region Access日文解码
        /// <summary>
        /// Access日文解码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string JPUncode(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return null;
            }
            content = content.Replace("#JPString01#", "ガ");
            content = content.Replace("#JPString02#", "ギ");
            content = content.Replace("#JPString03#", "ア");
            content = content.Replace("#JPString04#", "ゲ");
            content = content.Replace("#JPString05#", "ゴ");
            content = content.Replace("#JPString06#", "ザ");
            content = content.Replace("#JPString07#", "ジ");
            content = content.Replace("#JPString08#", "ズ");
            content = content.Replace("#JPString09#", "ゼ");
            content = content.Replace("#JPString10#", "ゾ");
            content = content.Replace("#JPString11#", "ダ");
            content = content.Replace("#JPString12#", "ヂ");
            content = content.Replace("#JPString13#", "ヅ");
            content = content.Replace("#JPString14#", "デ");
            content = content.Replace("#JPString15#", "ド");
            content = content.Replace("#JPString16#", "バ");
            content = content.Replace("#JPString17#", "パ");
            content = content.Replace("#JPString18#", "ビ");
            content = content.Replace("#JPString19#", "ピ");
            content = content.Replace("#JPString20#", "ブ");
            content = content.Replace("#JPString21#", "プ");
            content = content.Replace("#JPString22#", "ベ");
            content = content.Replace("#JPString23#", "ペ");
            content = content.Replace("#JPString24#", "ボ");
            content = content.Replace("#JPString25#", "ポ");
            content = content.Replace("#JPString26#", "ヴ");
            content = content.Replace("#JPString27#", "ン");
            content = content.Replace("#JPString28#", "ム");
            return content;
        }
        #endregion

        #region 写入二进制的帮助方法ForSQL
        /// <summary>
        /// 写入二进制的帮助方法ForSQL
        /// </summary>
        /// <param name="parameterName">Parameter名称</param>
        /// <param name="data">要写入的数据</param>
        /// <returns></returns>
        public static SqlParameter ToBinaryForSql(string parameterName, byte[] data)
        {
            return ToBinaryForSql(parameterName, data, SqlDbType.Image);
        }
        /// <summary>
        /// 写入二进制的帮助方法ForSQL
        /// </summary>
        /// <param name="parameterName">Parameter名称</param>
        /// <param name="data">要写入的数据</param>
        /// <param name="dbType">数据类型(只认SqlDbType.Binary,SqlDbType.Image,SqlDbType.VarBinary)</param>
        /// <returns></returns>
        public static SqlParameter ToBinaryForSql(string parameterName, byte[] data, SqlDbType dbType)
        {
            if (dbType != SqlDbType.Binary && dbType != SqlDbType.Image && dbType != SqlDbType.VarBinary)
            {
                throw new Exception("dbType参数只接受SqlDbType.Binary,SqlDbType.Image,SqlDbType.VarBinary");
            }

            SqlParameter p = new SqlParameter(parameterName, dbType, int.MaxValue - 1);
            p.Value = data;
            return p;
        } 
        #endregion

        #region 过滤SQL特殊字符(私有方法)
        /// <summary>
        /// 过滤SQL特殊字符(私有方法)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected static string ReplaceSqlString(string content)
        {
            if (content == null)
            {
                return null;
            }
            else
            {
                content = content.Replace("'", "’");

                return content;
            }
        } 
        #endregion

    }
}

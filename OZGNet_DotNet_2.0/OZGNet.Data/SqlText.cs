using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace OZGNet.Data
{
    /// <summary>
    /// 用于获取SQL语句
    /// </summary>
    public class SqlText
    {
        /// <summary>
        /// 获取添加数据的sql语句
        /// </summary>
        /// <param name="ht">Hash表</param>
        /// <param name="table">目标表</param>
        /// <returns>sql语句</returns>
        public static string Insert(Hashtable ht, string table)
        {
            string keys = "";
            string values = "";
            int i = 0;
            string value;
            foreach (DictionaryEntry de in ht)
            {
                //这里是判断添加的值是否是数字
                value = StrOrNum(de.Value);


                //如果不是第一个字段或值,则在字段之前添加,
                if (i == 0)
                {
                    keys = de.Key.ToString();
                    values = value;
                }
                else
                {
                    keys += "," + de.Key.ToString();
                    values += "," + value;
                }
                i += 1;
            }

            return "insert into " + table + " (" + keys + ") values(" + values + ")";
        }


        /// <summary>
        /// 获取删除数据的sql语句
        /// </summary>
        /// <param name="table">目标表</param>
        /// <param name="_where">查询条件</param>
        /// <returns>sql语句</returns>
        public static string Delete(string table, string _where)
        {
            return "delete from " + table + " where " + _where;
        }

        /// <summary>
        /// 获取更新数据的sql语句
        /// </summary>
        /// <param name="ht">Hash表</param>
        /// <param name="table">目标表</param>
        /// <param name="_where">查询条件</param>
        /// <returns>sql语句</returns>
        public static string Update(Hashtable ht, string table, string _where)
        {
            string keys = "";
            string value;
            int i = 0;

            foreach (DictionaryEntry de in ht)
            {
                //这里是判断添加的值是否是数字
                value = StrOrNum(de.Value);


                if (i == 0)
                {
                    keys = de.Key.ToString() + "=" + value;
                }
                else
                {
                    keys += "," + de.Key.ToString() + "=" + value;
                }
                i += 1;
            }
            return "update " + table + " set " + keys + " where " + _where;
        }

        /// <summary>
        /// 递增指定字段的值
        /// </summary>
        /// <param name="key">字段</param>
        /// <param name="value">递增值</param>
        /// <param name="table">目标表</param>
        /// <param name="_where">查询条件</param>
        /// <returns>sql语句</returns>
        public static string UpdateIncremental(string key, int value, string table, string _where)
        {
            return "update " + table + " set " + key + "=" + key + "+" + value + " where " + _where;
        }

        /// <summary>
        /// 获取查询数据的sql语句
        /// </summary>
        /// <param name="top">查询记录数</param>
        /// <param name="key">字段(用,隔开)</param>
        /// <param name="table">目标表</param>
        /// <param name="_where">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns>sql语句</returns>
        public static string Select(string top, string key, string table, string _where, string order)
        {
            string key_ = "";
            StringBuilder sb = new StringBuilder("select");

            //查询指定数据
            if (!String.IsNullOrEmpty(top))
            {
                sb.Append(" top " + top);
            }

            //指定字段
            if (String.IsNullOrEmpty(key))
            {
                key_ = "*";
            }
            else
            {
                key_ = key;
            }

            sb.Append(" " + key_ + " from " + table);

            //查询条件
            if (!String.IsNullOrEmpty(_where))
            {
                sb.Append(" where " + _where);
            }

            //数据排序
            if (!String.IsNullOrEmpty(order))
            {
                sb.Append(" order by " + order);
            }

            return sb.ToString();
        }

        /// <summary>
        /// MSSQL分页语句
        /// </summary>
        /// <param name="key">查询列</param>
        /// <param name="PrimaryKey">主键</param>
        /// <param name="table">表</param>
        /// <param name="_where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示</param>
        /// <returns></returns>
        public static string SelectPage(string key, string PrimaryKey, string table, string _where, string order, int pageIndex, int pageSize)
        {
            string sql = "select top " + pageSize.ToString();

            if (!String.IsNullOrEmpty(key))
            {
                sql += " " + key;
            }
            else
            {
                sql += " *";
            }

            string where1 = "";
            string where2 = "";
            if (!String.IsNullOrEmpty(_where))
            {
                where1 = " where " + _where;
                where2 = " and " + _where;
            }
            string order_ = "";
            if (!String.IsNullOrEmpty(order))
            {
                order_ = " order by " + order;
            }

            int startIndex = (pageIndex - 1) * pageSize;
            if (startIndex != 0)
            {
                sql += " from " + table + " where " + PrimaryKey + " not in (select top " + startIndex.ToString() + " " + PrimaryKey + "  from " + table + " " + where1 + " " + order_ + ") " + where2 + " " + order_;
            }
            else
            {
                sql += " from " + table + " " + where1 + " " + order_;
            }
            return sql;		
        }

        /// <summary>
        /// MYSQL分页语句
        /// </summary>
        /// <param name="key">查询列</param>
        /// <param name="table">表</param>
        /// <param name="_where">条件</param>
        /// <param name="order">排序(如table.id desc)</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示</param>
        /// <returns></returns>
        public static string SelectMysqlPage(string key, string PrimaryKey, string table, string _where, string order, int pageIndex, int pageSize)
        {
            if (!string.IsNullOrEmpty(_where))
            {
                _where = " where " + _where;
            }

            if (!string.IsNullOrEmpty(order))
            {
                order = " order by " + order;
            }

            string sql = "select " + key + " from " + table + " inner join (select " + PrimaryKey + " from " + table + " " + _where + " " + order + " limit " + ((pageIndex - 1) * pageSize).ToString() + "," + pageSize.ToString() + ") as tmp on tmp." + PrimaryKey + "=" + table + "." + PrimaryKey + " " + order;

            return sql;
        }

        /// <summary>
        /// 获取查询数据的sql语句(MySql)
        /// </summary>
        /// <param name="top">查询记录数</param>
        /// <param name="key">字段(用,隔开)</param>
        /// <param name="table">目标表</param>
        /// <param name="_where">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns>sql语句</returns>
        public static string SelectMysql(string top, string key, string table, string _where, string order)
        {
            string key_ = "";

            //指定字段
            if (String.IsNullOrEmpty(key))
            {
                key_ = "*";
            }
            else
            {
                key_ = key;
            }

            StringBuilder sb = new StringBuilder("select " + key_ + " from " + table);

            //查询条件
            if (!String.IsNullOrEmpty(_where))
            {
                sb.Append(" where " + _where);
            }

            //数据排序
            if (!String.IsNullOrEmpty(order))
            {
                sb.Append(" order by " + order);
            }

            //查询指定数据
            if (!String.IsNullOrEmpty(top))
            {
                sb.Append(" limit " + top);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取统计数据的sql语句
        /// </summary>
        /// <param name="Func">统计函数</param>
        /// <param name="key">目标字段</param>
        /// <param name="table">目标表</param>
        /// <returns>sql语句</returns>
        public static string Func(string Func, string key, string table)
        {
            StringBuilder sb = new StringBuilder("select " + Func + "(" + key + ") from " + table);
            return sb.ToString();
        }       

        /// <summary>
        /// 获取统计数据的sql语句
        /// </summary>
        /// <param name="Func">统计函数</param>
        /// <param name="key">目标字段</param>
        /// <param name="table">目标表</param>
        /// <param name="_where">查询条件</param>
        /// <returns>sql语句</returns>
        public static string Func(string Func, string key, string table, string _where)
        {
            StringBuilder sb = new StringBuilder("select " + Func + "(" + key + ") from " + table);
            if (!String.IsNullOrEmpty(_where))
            {
                sb.Append(" where " + _where);
            }
            return sb.ToString();
        }       
                
        
        #region 取该ID的（上一条记录）
        /// <summary>
        /// 取该ID的（上一条记录）
        /// </summary>
        /// <param name="columns">目标字段</param>
        /// <param name="table">目标表</param>
        /// <param name="id">ID字段</param>
        /// <param name="value">ID字段值</param>
        /// <returns></returns>
        public static string RecordPrev(string columns, string table, string id, string value)
        {
            return "select top 1 " + columns + " from " + table + " where " + id + "<" + value + " order by " + id + " desc";
        }

        /// <summary>
        /// 取该ID的（上一条记录）
        /// </summary>
        /// <param name="columns">目标字段</param>
        /// <param name="table">目标表</param>
        /// <param name="id">ID字段</param>
        /// <param name="value">ID字段值</param>
        /// <param name="where">where查询</param>
        /// <returns></returns>
        public static string RecordPrev(string columns, string table, string id, string value, string where)
        {
            string _where = where + " and";
            return "select top 1 " + columns + " from " + table + " where " + _where + " " + id + "<" + value + " order by " + id + " desc";
        }
        #endregion

        #region 取该ID的（下一条记录）
        /// <summary>
        /// 取该ID的（下一条记录）
        /// </summary>
        /// <param name="columns">目标字段</param>
        /// <param name="table">目标表</param>
        /// <param name="id">ID字段</param>
        /// <param name="value">ID字段值</param>
        /// <returns></returns>
        public static string RecordNext(string columns, string table, string id, string value)
        {
            return "select top 1 " + columns + " from " + table + " where " + id + ">" + value + " order by " + id + " asc";
        }

        /// <summary>
        /// 取该ID的（下一条记录）
        /// </summary>
        /// <param name="columns">目标字段</param>
        /// <param name="table">目标表</param>
        /// <param name="id">ID字段</param>
        /// <param name="value">ID字段值</param>
        /// <param name="where">where查询</param>
        /// <returns></returns>
        public static string RecordNext(string columns, string table, string id, string value, string where)
        {
            string _where = where + " and";
            return "select top 1 " + columns + " from " + table + " where " + _where + " " + id + ">" + value + " order by " + id + " asc";
        }
        #endregion







        /// <summary>
        /// 这里是判断添加的值是否是数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected static string StrOrNum(object obj)
        {
            if (obj.GetType().Equals(typeof(string)))
            {
                return "'" + obj.ToString() + "'";
            }
            else
            {
                return obj.ToString();                
            }
        }


    }
}

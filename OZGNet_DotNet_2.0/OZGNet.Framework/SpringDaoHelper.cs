using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace OZGNet.Framework
{
    /// <summary>
    /// SpringDaoHelper数据访问类
    /// </summary>
    public class SpringDaoHelper : HibernateDaoSupport
    {
        ISession iSession = null;

        #region 直接执行SQL语句
        /// <summary>
        /// 直接执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        public int ExecuteNonQuery(string sql)
        {
            IDbConnection conn = Connection;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            int i = cmd.ExecuteNonQuery();
            CloseISession();
            return i;
        } 
        #endregion

        #region 执行SQL语句(事务)
        /// <summary>
        /// 执行SQL语句(事务)
        /// </summary>
        /// <param name="sqls"></param>
        public bool ExecuteNonQueryTransaction(List<string> sqls)
        {
            IDbConnection conn = Connection;
            IDbTransaction idbt = conn.BeginTransaction();
            int sql_count = sqls.Count;
            IDbCommand cmd;
            List<IDbCommand> cmds = new List<IDbCommand>();
            for (int i = 0; i < sql_count; i++)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = sqls[i];
                cmd.Transaction = idbt;
                cmds.Add(cmd);
            }

            bool isOK;
            try
            {
                for (int i = 0; i < sql_count; i++)
                {
                    cmds[i].ExecuteNonQuery();
                }
                idbt.Commit();
                isOK = true;
            }
            catch
            {
                idbt.Rollback();
                isOK = false;
            }
            CloseISession();
            return isOK;
        } 
        #endregion

        #region 增删改操作
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="obj"></param>
        public void Add(object obj)
        {
            HibernateTemplate.Save(obj);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="obj"></param>
        public void Delete(object obj)
        {
            HibernateTemplate.Delete(obj);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="obj"></param>
        public void Update(object obj)
        {
            HibernateTemplate.Update(obj);
        } 
        #endregion

        #region 使用HQL获取数据列表
        /// <summary>
        /// 使用HQL获取数据列表
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public ArrayList GetList(string hql)
        {
            IList list = GetIQuery(hql).List();
            CloseISession();
            return (ArrayList)list;
        } 
        #endregion

        #region 使用HQL获取数据列表(返回指定行)
        /// <summary>
        /// 使用HQL获取数据列表(返回指定行)
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public ArrayList GetList(string hql, int top)
        {
            IList list = GetIQuery(hql, top).List();
            CloseISession();
            return (ArrayList)list;
        } 
        #endregion

        #region 使用HQL获取数据列表(分页)
        /// <summary>
        /// 使用HQL获取数据列表(分页)
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ArrayList GetList(string hql, int pageIndex, int pageSize)
        {
            IList list = GetIQuery(hql, pageIndex, pageSize).List();
            CloseISession();
            return (ArrayList)list;
        } 
        #endregion

        #region 返回单个实体对象
        /// <summary>
        /// 返回单个实体对象
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object GetModel(string table, string where)
        {
            IList list = isession.CreateQuery("from " + table + " where " + where).List();
            CloseISession();
            return list[0];
        } 
        #endregion

        #region 返回第一行第一列
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="func"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public object ExecuteScalar(string table, string func, string fid)
        {
            IDbConnection conn = Connection;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select " + func + "(" + fid + ") from " + table;
            object obj = cmd.ExecuteScalar();
            CloseISession();
            return obj;
        } 
        #endregion

        #region 返回第一行第一列(带查询条件)
        /// <summary>
        /// 返回第一行第一列(带查询条件)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="func"></param>
        /// <param name="fid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object ExecuteScalar(string table, string func, string fid, string where)
        {
            IDbConnection conn = Connection;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select " + func + "(" + fid + ") from " + table + " where " + where;
            object obj = cmd.ExecuteScalar();
            CloseISession();
            return obj;
        } 
        #endregion

        #region 返回IQuery(强类型需要用到)
        /// <summary>
        /// 返回IQuery(强类型需要用到)
        /// </summary>
        /// <param name="hql">HQL语句</param>
        /// <returns></returns>
        public IQuery GetIQuery(string hql)
        {
            IQuery query = isession.CreateQuery(hql);
            return query;
        }
        #endregion

        #region 返回IQuery(返回指定行)(强类型需要用到)
        /// <summary>
        /// 返回IQuery(返回指定行)(强类型需要用到)
        /// </summary>
        /// <param name="hql">HQL语句</param>
        /// <param name="top">返回数据的行数</param>
        /// <returns></returns>
        public IQuery GetIQuery(string hql, int top)
        {
            IQuery query = isession.CreateQuery(hql);
            query.SetFirstResult(0);
            query.SetMaxResults(top);
            return query;
        }
        #endregion

        #region 返回IQuery(分页)(强类型需要用到)
        /// <summary>
        /// 返回IQuery(分页)(强类型需要用到)
        /// </summary>
        /// <param name="hql">HQL语句</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public IQuery GetIQuery(string hql, int pageIndex, int pageSize)
        {
            IQuery query = isession.CreateQuery(hql);
            int first = (pageIndex - 1) * pageSize;
            query.SetFirstResult(first);
            query.SetMaxResults(pageSize);
            return query;
        }
        #endregion
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        protected IDbConnection Connection
        {
            get
            {
                return isession.Connection;
            }
        }
        /// <summary>
        /// 获取ISession
        /// </summary>
        protected ISession isession
        {
            get
            {
                iSession = Session;
                return iSession;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void CloseISession()
        {
            iSession.Clear();
            iSession.Close();
            iSession.Dispose();
        }

    }
}

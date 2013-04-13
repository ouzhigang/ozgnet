using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace OZGNet.Framework
{
    /// <summary>
    /// NHibernate数据访问类
    /// </summary>
    public class NHibernateHelper
    {
        private static ISession session;
        private static NHibernateSessionFactoryFactory hibernate = new NHibernateSessionFactoryFactory();
        private string _AssemblyName;

        #region 构造函数
        /// <summary>
        /// 实例化NHibernateHelper
        /// </summary>
        public NHibernateHelper()
        { }
        /// <summary>
        /// 实例化NHibernateHelper
        /// </summary>
        /// <param name="_AssemblyName">程序集名称(DLL)</param>
        public NHibernateHelper(string _AssemblyName)
        {
            this._AssemblyName = _AssemblyName;
        }
        #endregion

        #region 相关属性
        /// <summary>
        /// 获取或设置程序集名称(DLL)
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return _AssemblyName;
            }
            set
            {
                _AssemblyName = value;
            }
        }
        #endregion

        #region 直接执行SQL语句
        /// <summary>
        /// 直接执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        public int ExecuteNonQuery(string sql)
        {
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IDbConnection conn = session.Connection;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            int i = cmd.ExecuteNonQuery();
            //hibernate.CloseSession();
            Close();
            return i;
        }
        #endregion

        #region 执行SQL语句(事务)
        /// <summary>
        /// 执行SQL语句(事务)
        /// </summary>
        /// <param name="sqls"></param>
        public bool ExecuteNonQueryTransaction(IList<string> sqls)
        {
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IDbConnection conn = session.Connection;
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
            //hibernate.CloseSession();
            Close();
            return isOK;
        }
        #endregion

        #region 添加数据
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="o"></param>
        public bool Add(Object o)
        {
            bool isOK;
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            ITransaction t = session.BeginTransaction();
            try
            {
                session.Save(o);
                t.Commit();
                isOK = true;
            }
            catch
            {
                t.Rollback();
                isOK = false;
            }
            //hibernate.CloseSession();
            Close();
            return isOK;
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="o"></param>
        public bool Delete(Object o)
        {
            bool isOK;
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            ITransaction t = session.BeginTransaction();
            try
            {
                session.Delete(o);
                t.Commit();
                isOK = true;
            }
            catch
            {
                t.Rollback();
                isOK = false;
            }
            //hibernate.CloseSession();
            Close();
            return isOK;
        }
        #endregion

        #region 更新数据
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="o"></param>
        public bool Update(Object o)
        {
            bool isOK;
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            ITransaction t = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(o);
                t.Commit();
                isOK = true;
            }
            catch
            {
                t.Rollback();
                isOK = false;
            }
            //hibernate.CloseSession();
            Close();
            return isOK;
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
            //hibernate.CloseSession();
            Close();
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
            //hibernate.CloseSession();
            Close();
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
            //hibernate.CloseSession();
            Close();
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
        public Object GetModel(string table, string where)
        {
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IList list = session.CreateQuery("from " + table + " where " + where).List();
            //hibernate.CloseSession();
            Close();
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
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IDbConnection conn = session.Connection;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select " + func + "(" + fid + ") from " + table;
            object obj = cmd.ExecuteScalar();
            //hibernate.CloseSession();
            Close();
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
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IDbConnection conn = session.Connection;
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select " + func + "(" + fid + ") from " + table + " where " + where;
            object obj = cmd.ExecuteScalar();
            //hibernate.CloseSession();
            Close();
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
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IQuery query = session.CreateQuery(hql);
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
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IQuery query = session.CreateQuery(hql);
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
            hibernate.AssemblyName = AssemblyName;
            session = hibernate.GetCurrentSession();
            IQuery query = session.CreateQuery(hql);
            int first = (pageIndex - 1) * pageSize;
            query.SetFirstResult(first);
            query.SetMaxResults(pageSize);
            return query;
        }
        #endregion

        #region 关闭ISession
        /// <summary>
        /// 关闭ISession
        /// </summary>
        public void Close()
        {
            hibernate.CloseSession();
        }
        #endregion

    }
}

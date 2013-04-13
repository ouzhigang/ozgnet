using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace OZGNet.Framework
{
    /// <summary>
    /// NHibernateSessionFactoryFactory(依赖NHibernate.dll)
    /// </summary>
    partial class NHibernateSessionFactoryFactory
    {
        string _AssemblyName;
        string CurrentSessionKey = "nhibernate.current_session";
        HttpContext context;
        ISession currentSession;
        ISessionFactory sessionFactory;

        public NHibernateSessionFactoryFactory()
        { }
        public NHibernateSessionFactoryFactory(string AssemblyName)
        {
            this._AssemblyName = AssemblyName;
        }

        /// <summary>
        /// 获取当前ISession
        /// </summary>
        /// <returns></returns>
        public ISession GetCurrentSession()
        {
            context = HttpContext.Current;
            currentSession = context.Items[CurrentSessionKey] as ISession;
            if (currentSession == null)
            {
                NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration().AddAssembly(this._AssemblyName);
                sessionFactory = config.BuildSessionFactory();
                currentSession = sessionFactory.OpenSession();
                context.Items.Add(CurrentSessionKey, currentSession);
            }
            return currentSession;
        }

        /// <summary>
        /// 关闭当前ISession
        /// </summary>
        public void CloseSession()
        {
            context = HttpContext.Current;
            currentSession = context.Items[CurrentSessionKey] as ISession;
            if (currentSession != null)
            {
                currentSession.Close();
                try
                {
                    context.Items.Remove(CurrentSessionKey);
                }
                catch
                { 
                    //不做处理
                }
            }
        }

        /// <summary>
        /// 关闭当前ISessionFactory
        /// </summary>
        public void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }

        public string AssemblyName
        {
            get
            {
                return this._AssemblyName;
            }
            set
            {
                this._AssemblyName = value;
            }
        }

    }
}

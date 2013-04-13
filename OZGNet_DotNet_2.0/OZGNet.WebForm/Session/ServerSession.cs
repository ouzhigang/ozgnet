using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

namespace OZGNet.WebForm.Session
{
    /// <summary>
    /// 服务器端Session会话
    /// </summary>
    public class ServerSession : ISession
    {
        #region ISession 成员
        /// <summary>
        /// 获取SessionID
        /// </summary>
        public string SessionID
        {
            get
            {
                return HttpContext.Current.Session.SessionID;
            }
        }

        /// <summary>
        /// 写入会话
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            HttpContext.Current.Session.Add(name, value);
        }
        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name)
        {
            try
            {
                return HttpContext.Current.Session[name];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }
        /// <summary>
        /// 会话是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            if (this.Get(name) != null)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}

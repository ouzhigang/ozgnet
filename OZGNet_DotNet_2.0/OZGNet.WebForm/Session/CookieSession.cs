using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

namespace OZGNet.WebForm.Session
{
    /// <summary>
    /// 客户端Cookie会话
    /// </summary>
    public class CookieSession : ISession
    {
        string _SessionID;
       
        public CookieSession()
        { 
            //暂时用服务器的SessionID
            _SessionID = HttpContext.Current.Session.SessionID;
        }

        #region ISession 成员
        /// <summary>
        /// 获取SessionID
        /// </summary>
        public string SessionID
        {
            get
            {
                return _SessionID;
            }
        }

        /// <summary>
        /// 写入会话
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            CookieHelper.SetCookieSerialize(name, value);
        }
        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name)
        {
            return CookieHelper.GetCookieSerialize(name);
        }
        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            CookieHelper.DelCookie(name);
        }
        /// <summary>
        /// 会话是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return CookieHelper.ExistsCookie(name);
        }

        #endregion
    }
}

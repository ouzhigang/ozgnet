using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebForm.Session
{
    /// <summary>
    /// 会话接口
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 获取SessionID
        /// </summary>
        string SessionID { get; }
        /// <summary>
        /// 写入会话
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void Set(string name, object value);
        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object Get(string name);
        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
        /// <summary>
        /// 会话是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Exists(string name);
    }
}

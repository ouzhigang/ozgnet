using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebForm.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 共用配置对象
        /// </summary>
        object ConfigObject { set; get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        DateTime ExpiryTime { set; get; }
        /// <summary>
        /// 写入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void Set(string key, object val);
        /// <summary>
        /// 写入缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiryTime">过期时间</param>
        void Set(string key, object val, DateTime expiryTime);
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        object Get(string key);
        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);
        /// <summary>
        /// 缓存对象是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);        
    }
}

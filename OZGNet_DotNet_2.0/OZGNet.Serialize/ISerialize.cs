using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace OZGNet.Serialize
{
    /// <summary>
    /// 序列化接口
    /// </summary>
    public interface ISerialize
    {
        /// <summary>
        /// 获取或设置文件的路径
        /// </summary>
        string FilePath { get; set; }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        void Serialize(object obj);
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <returns></returns>
        object DeSerialize();
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace OZGNet.WebForm.Serialize
{
    /// <summary>
    /// 序列化接口，使用WebForm的路径来保存文件
    /// </summary>
    public interface ISerialize
    {
        /// <summary>
        /// 获取或设置文件保存的路径
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj">序列化对象</param>
        void Serialize(object obj);

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <returns>反序列化对象</returns>
        object DeSerialize();
    }
}

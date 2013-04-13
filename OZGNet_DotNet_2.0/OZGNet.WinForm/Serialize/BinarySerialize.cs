using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OZGNet.WinForm.Serialize
{
    /// <summary>
    /// 使用二进制方式保存序列化文件，实现ISerialize接口
    /// </summary>
    public class BinarySerialize : ISerialize
    {
        private string _FilePath;
        OZGNet.Serialize.ISerialize serialize = new OZGNet.Serialize.BinarySerialize();

        /// <summary>
        /// 实例化BinarySerialize
        /// </summary>
        public BinarySerialize()
        { }
        /// <summary>
        /// 实例化BinarySerialize
        /// </summary>
        /// <param name="FilePath">设置文件保存的路径</param>
        public BinarySerialize(string FilePath)
        {
            this._FilePath = FilePath; 
        }
        /// <summary>
        /// 获取或设置文件保存的路径
        /// </summary>
        public string FilePath
        {
            get { return this._FilePath; }
            set { this._FilePath = value; }
        }        
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj">序列化对象</param>
        public void Serialize(object obj)
        {            
            serialize.FilePath = this._FilePath;
            serialize.Serialize(obj);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <returns>反序列化对象</returns>
        public object DeSerialize()
        {
            serialize.FilePath = this._FilePath;
            return serialize.DeSerialize();
        }

    }
}

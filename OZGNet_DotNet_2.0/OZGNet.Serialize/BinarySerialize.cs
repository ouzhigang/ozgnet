using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OZGNet.Serialize
{
    /// <summary>
    /// 二进制序列化
    /// </summary>
    public class BinarySerialize : ISerialize
    {
        private string _FilePath;
        /// <summary>
        /// 实例化BinarySerialize
        /// </summary>
        public BinarySerialize()
        { }
        /// <summary>
        /// 实例化BinarySerialize
        /// </summary>
        /// <param name="FilePath">文件的路径</param>
        public BinarySerialize(string FilePath)
        {
            this._FilePath = FilePath; 
        }
        /// <summary>
        /// 获取或设置文件的路径
        /// </summary>
        public string FilePath
        {
            get { return this._FilePath; }
            set { this._FilePath = value; }
        }        
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        public void Serialize(object obj)
        {
            if (File.Exists(this._FilePath))
            {
                File.Delete(this._FilePath);
            }

            using (FileStream fs = new FileStream(this._FilePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <returns></returns>
        public object DeSerialize()
        {
            object obj;
            using (FileStream fs = new FileStream(this._FilePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(fs);
            }
            return obj;
        }

    }
}

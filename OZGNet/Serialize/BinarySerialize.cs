using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OZGNet.Serialize
{
    /// <summary>
    /// 二进制序列化
    /// </summary>
    public class BinarySerialize : ISerialize
    {
        private string M_FilePath;
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
            this.M_FilePath = FilePath; 
        }
        /// <summary>
        /// 获取或设置文件的路径
        /// </summary>
        public string FilePath
        {
            get { return this.M_FilePath; }
            set { this.M_FilePath = value; }
        }        
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj"></param>
        public void Serialize(object obj)
        {
            if (File.Exists(this.M_FilePath))
            {
                File.Delete(this.M_FilePath);
            }

            using (FileStream fs = new FileStream(this.M_FilePath, FileMode.Create))
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
            using (FileStream fs = new FileStream(this.M_FilePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(fs);
            }
            return obj;
        }

    }
}

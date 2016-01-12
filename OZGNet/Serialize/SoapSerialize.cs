using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace OZGNet.Serialize
{
    /// <summary>
    /// Soap序列化
    /// </summary>
    public class SoapSerialize : ISerialize
    {
        private string M_FilePath;
        /// <summary>
        /// 实例化SoapSerialize
        /// </summary>
        public SoapSerialize()
        { }
        /// <summary>
        /// 实例化SoapSerialize
        /// </summary>
        /// <param name="FilePath">文件的路径</param>
        public SoapSerialize(string FilePath)
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
                SoapFormatter formatter = new SoapFormatter();
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
                SoapFormatter formatter = new SoapFormatter();
                obj = formatter.Deserialize(fs);
            }
            return obj;
        }

    }
}

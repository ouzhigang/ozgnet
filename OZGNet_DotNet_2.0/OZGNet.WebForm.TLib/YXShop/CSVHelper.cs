using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.YXShop
{    
    /// <summary>
    /// YXShop CSV帮助类
    /// </summary>
    public class CSVHelper
    {
        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePathName">CSV文件的绝对路径</param>
        /// <returns>输出内容：IList的每一行是内容的每一行，string[]的每一项则是用逗号隔开</returns>
        public static IList<string[]> Read(string filePathName)
        {
            IList<string[]> ls = new List<string[]>();
            StreamReader fileReader = new StreamReader(filePathName);
            string strLine = string.Empty;
            while (strLine != null)
            {
                strLine = fileReader.ReadLine();
                if ((strLine != null) && (strLine.Length > 0))
                {
                    ls.Add(strLine.Split(new char[] { ',' }));
                }
            }
            fileReader.Close();
            return ls;
        }
        /// <summary>
        /// 重新写入CSV文件
        /// </summary>
        /// <param name="filePathName">CSV文件的绝对路径</param>
        /// <param name="ls">输入内容：IList的每一行是内容的每一行，string[]的每一项则是用逗号隔开</param>
        public static void Write(string filePathName, IList<string[]> ls)
        {
            Write(filePathName, false, ls);
        }
        /// <summary>
        /// 写入CSV文件
        /// </summary>
        /// <param name="filePathName">CSV文件的绝对路径</param>
        /// <param name="append">是否追加原有的文件</param>
        /// <param name="ls">输入内容：IList的每一行是内容的每一行，string[]的每一项则是用逗号隔开</param>
        public static void Write(string filePathName, bool append, IList<string[]> ls)
        {
            StreamWriter fileWriter = new StreamWriter(filePathName, append, Encoding.Default);
            foreach (string[] strArr in ls)
            {
                fileWriter.WriteLine(string.Join(",", strArr));
            }
            fileWriter.Flush();
            fileWriter.Close();
        }
    }
}


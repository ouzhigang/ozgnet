using System;
using System.IO;
using System.Web;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{   
    /// <summary>
    /// DotBBS的IO工具类
    /// </summary>
    public class IOUtil
    {
        /// <summary>
        /// 使用日期为名称来建立目录
        /// </summary>
        /// <param name="path">目标目录</param>
        /// <returns></returns>
        public static string CreateDateTimeDir(string path)
        {
            DateTime now = DateTime.Now;
            string str = now.Year.ToString();
            string str2 = now.Month.ToString();
            string str3 = now.Day.ToString();
            string[] strArray = path.Split(new char[] { '/' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (((strArray[i] != "") && (strArray[i] != "..")) && (i != 0))
                {
                    string str4 = "../";
                    for (int j = 1; j <= i; j++)
                    {
                        str4 = str4 + strArray[j] + "/";
                    }
                    CreateDir(str4);
                }
            }
            CreateDir(path + "/" + str);
            CreateDir(path + "/" + str + "/" + str2);
            CreateDir(path + "/" + str + "/" + str2 + "/" + str3);
            return (path + "/" + str + "/" + str2 + "/" + str3);
        }
        /// <summary>
        /// 使用日期为名称来建立目录
        /// </summary>
        /// <param name="path">目标目录</param>
        /// <param name="pre"></param>
        /// <returns></returns>
        public static string CreateDateTimeDir(string path, string pre)
        {
            DateTime now = DateTime.Now;
            string str = now.Year.ToString();
            string str2 = now.Month.ToString();
            string str3 = now.Day.ToString();
            string[] strArray = path.Split(new char[] { '/' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (((strArray[i] != "") && (strArray[i] != pre)) && (i != 0))
                {
                    string str4 = pre + "/";
                    for (int j = 1; j <= i; j++)
                    {
                        str4 = str4 + strArray[j] + "/";
                    }
                    CreateDir(str4);
                }
            }
            CreateDir(path + "/" + str);
            CreateDir(path + "/" + str + "/" + str2);
            CreateDir(path + "/" + str + "/" + str2 + "/" + str3);
            return (path + "/" + str + "/" + str2 + "/" + str3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private static void CreateDir(string path)
        {
            string str = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
        }
    }
}


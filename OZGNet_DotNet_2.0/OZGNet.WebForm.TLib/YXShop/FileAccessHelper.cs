using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace OZGNet.WebForm.ThirdPart.YXShop
{   
    /// <summary>
    /// YXShop IO访问帮助类,默认编码为UTF-8
    /// </summary>
    public class FileAccessHelper
    {
        private static Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static bool Delete(string filePath)
        {
            try
            {
                new FileInfo(filePath).Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fu">System.Web.UI.WebControls.FileUpload实例</param>
        /// <param name="savePath">保存文件的绝对路径</param>
        /// <returns></returns>
        public static bool FileUpload(System.Web.UI.WebControls.FileUpload fu, string savePath)
        {
            return FileUpload(fu.PostedFile, savePath);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="hpf">System.Web.HttpPostedFile实例</param>
        /// <param name="savePath">保存文件的绝对路径</param>
        /// <returns></returns>
        public static bool FileUpload(System.Web.HttpPostedFile hpf, string savePath)
        {
            try
            {
                hpf.SaveAs(savePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 以UTF-8编码来读取文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <returns></returns>
        public static string ReadTextFile(string fileName)
        {
            return ReadTextFile(fileName, defaultEncoding);
        }
        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ReadTextFile(string fileName, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                return sr.ReadToEnd();
            }
        }
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <param name="text">写入内容</param>
        public static void WriteTextFile(string fileName, string text)
        {
            WriteTextFile(fileName, text, false, true, defaultEncoding);
        }
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <param name="text">写入内容</param>
        /// <param name="createDir">是否生成目录</param>
        public static void WriteTextFile(string fileName, string text, bool createDir)
        {
            WriteTextFile(fileName, text, false, createDir, defaultEncoding);
        }
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <param name="text">写入内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteTextFile(string fileName, string text, Encoding encoding)
        {
            WriteTextFile(fileName, text, false, true, encoding);
        }
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <param name="text">写入内容</param>
        /// <param name="createDir">是否生成目录</param>
        /// <param name="encoding">编码</param>
        public static void WriteTextFile(string fileName, string text, bool createDir, Encoding encoding)
        {
            WriteTextFile(fileName, text, false, createDir, encoding);
        }
        /// <summary>
        /// 写入文本文件
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        /// <param name="text">写入内容</param>
        /// <param name="append">是否在原有文件上追加内容</param>
        /// <param name="createDir">是否生成目录</param>
        /// <param name="encoding">编码</param>
        public static void WriteTextFile(string fileName, string text, bool append, bool createDir, Encoding encoding)
        {
            if (createDir)
            {
                string dirName = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
            }
            using (StreamWriter sw = new StreamWriter(fileName, append, encoding))
            {
                sw.Write(text);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="oldpath">目标文件</param>
        /// <param name="newpath">复制到新路径</param>
        /// <returns></returns>
        public static bool CopyTo(string oldpath, string newpath)
        {
            return CopyTo(oldpath, newpath, true);
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="oldpath">目标文件</param>
        /// <param name="newpath">复制到新路径</param>
        /// <param name="overwrite">是否覆盖原有文件</param>
        /// <returns></returns>
        public static bool CopyTo(string oldpath, string newpath, bool overwrite)
        {
            try
            {
                File.Copy(oldpath, newpath, overwrite);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldpath">原路径</param>
        /// <param name="newpath">新路径</param>
        /// <returns></returns>
        public static bool MoveTo(string oldpath, string newpath)
        {
            FileInfo fi = new FileInfo(oldpath);
            try
            {
                fi.MoveTo(newpath);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;

namespace OZGNet.WebForm
{
    /// <summary>
    /// WebForm IO帮助类
    /// </summary>
    public class UtilityIO : OZGNet.UtilityIO
    {
        /// <summary>
        /// 读取文件(编码为gb2312)
        /// </summary>
        /// <param name="file_path">目标文件(虚拟路径)</param>
        /// <returns></returns>
        public static string FileRead(string file_path)
        {
            return OZGNet.UtilityIO.FileRead(HttpContext.Current.Server.MapPath(file_path), "gb2312");
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file_path">目标文件(虚拟路径)</param>
        /// <param name="encoding">文件编码</param>
        /// <returns></returns>
        public static string FileRead(string file_path, string encoding)
        {
            return OZGNet.UtilityIO.FileRead(HttpContext.Current.Server.MapPath(file_path), encoding);
        }

        /// <summary>
        /// 写入文件(编码为gb2312)
        /// </summary>
        /// <param name="file_path">目标路径(虚拟路径)</param>
        /// <param name="content">写入内容</param>        
        public static void FileWrite(string file_path, string content)
        {
            OZGNet.UtilityIO.FileWrite(HttpContext.Current.Server.MapPath(file_path), content, "gb2312");
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="file_path">目标路径(虚拟路径)</param>
        /// <param name="content">写入内容</param>
        /// <param name="encoding">文件编码</param>
        public static void FileWrite(string file_path, string content, string encoding)
        {
            OZGNet.UtilityIO.FileWrite(HttpContext.Current.Server.MapPath(file_path), content, encoding);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file_path">目标文件(虚拟路径)</param>
        public static void FileDelete(string file_path)
        {
            OZGNet.UtilityIO.FileDelete(HttpContext.Current.Server.MapPath(file_path));
        }

        /// <summary>
        /// 删除该目录下的所有文件
        /// </summary>
        /// <param name="dir_path">目录路径(虚拟路径)</param>
        public static void FileDeleteInDir(string dir_path)
        {
            OZGNet.UtilityIO.FileDeleteInDir(HttpContext.Current.Server.MapPath(dir_path));
        }

        /// <summary>
        /// 删除该目录下的指定文件
        /// </summary>
        /// <param name="dir_path">目录路径</param>
        /// <param name="separator">分隔符</param>
        public static void FileDeleteInDir(string dir_path, string separator)
        {
            OZGNet.UtilityIO.FileDeleteInDir(HttpContext.Current.Server.MapPath(dir_path), separator);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir_path">目标文件夹(虚拟路径)</param>
        public static void DirDelete(string dir_path)
        {
            OZGNet.UtilityIO.DirDelete(HttpContext.Current.Server.MapPath(dir_path), true);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir_path">目标文件夹(虚拟路径)</param>
        /// <param name="recursive">是否清空目标文件夹的文件和文件夹</param>
        public static void DirDelete(string dir_path, bool recursive)
        {
            OZGNet.UtilityIO.DirDelete(HttpContext.Current.Server.MapPath(dir_path), recursive);
        }

        /// <summary>
        /// 复制文件夹和该文件夹下的所有内容到新的目录(相对路径)
        /// </summary>
        /// <param name="srcPath">文件夹源路径</param>
        /// <param name="aimPath">目标路径</param>
        public static void CopyDirectory(string srcPath, string aimPath)
        {
            OZGNet.UtilityIO.CopyDirectory(HttpContext.Current.Server.MapPath(srcPath), HttpContext.Current.Server.MapPath(aimPath));
        }

        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="srcPath">源文件夹路径</param>
        /// <param name="newName">新文件夹名</param>
        public static void ReNameDirectory(string srcPath, string newName)
        {
            //实例:ReNameDirectory("/Bin", "Bin2");

            OZGNet.UtilityIO.ReNameDirectory(HttpContext.Current.Server.MapPath(srcPath), newName);
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="newName">新文件名</param>
        public static void ReNameFile(string srcPath, string newName)
        {
            //实例:ReNameFile("/default.aspx", "default2.aspx");

            OZGNet.UtilityIO.ReNameFile(HttpContext.Current.Server.MapPath(srcPath), newName);
        }

        /// <summary>
        /// 转换FLV(大小为480*320)
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="input">输入视频路径</param>
        /// <param name="output">输出FLV路径</param>
        public static void ChangeFLV(string ffmpeg, string input, string output)
        {
            OZGNet.UtilityIO.ChangeFLV(HttpContext.Current.Server.MapPath(ffmpeg), input, HttpContext.Current.Server.MapPath(output), "480", "320");            
        }

        /// <summary>
        /// 转换FLV(要输入大小)
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="input">输入视频路径</param>
        /// <param name="output">输出FLV路径</param>
        /// <param name="width">FLV的宽</param>
        /// <param name="height">FLV的高</param>
        public static void ChangeFLV(string ffmpeg, string input, string output, string width, string height)
        {
            OZGNet.UtilityIO.ChangeFLV(HttpContext.Current.Server.MapPath(ffmpeg), input, HttpContext.Current.Server.MapPath(output), width, height);
        }

        /// <summary>
        /// 取视频其中的一帧然后生成图片
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="video_path">输入视频路径</param>
        /// <param name="outputIMG">输出图片路径</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高</param>
        /// <param name="ss">截取视频图片的当前秒</param>
        public static void VideoCreateIMG(string ffmpeg, string video_path, string outputIMG, string width, string height, string ss)
        {
            OZGNet.UtilityIO.VideoCreateIMG(HttpContext.Current.Server.MapPath(ffmpeg), HttpContext.Current.Server.MapPath(video_path), HttpContext.Current.Server.MapPath(outputIMG), width, height, ss);
        }

        #region 在线Rar压缩(针对单文件)
        /// <summary>
        /// 在线Rar压缩(针对单文件)
        /// </summary>
        /// <param name="workingDir">目标目录</param>
        /// <param name="inputFile">目标文件</param>
        /// <param name="rarPath">输出Rar路径</param>
        /// <returns>返回true，则表示成功</returns>
        public static bool Rar1(string workingDir, string inputFile, string rarPath)
        {
            bool isOK = false;
            //压缩
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRAR.exe\Shell\Open\Command");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                the_Info = " a -r -ep1 " + HttpContext.Current.Server.MapPath(rarPath) + " " + HttpContext.Current.Server.MapPath(workingDir) + inputFile;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath(workingDir);//获取或设置要启动的进程的初始目录。
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                isOK = true;
            }
            catch
            {
                isOK = false;
            }
            return isOK;
        } 
        #endregion

        #region 在线Rar压缩(针对多文件)
        /// <summary>
        /// 在线Rar压缩(针对多文件)
        /// </summary>
        /// <param name="workingDir">目标目录</param>
        /// <param name="inputFiles">目标文件列表</param>
        /// <param name="rarPath">输出Rar路径</param>
        /// <returns></returns>
        public static bool Rar1(string workingDir, ArrayList inputFiles, string rarPath)
        {
            bool isOK = false;
            //压缩
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                string tmpStr = "";
                for (int i = 0; i < inputFiles.Count; i++)
                {
                    tmpStr += HttpContext.Current.Server.MapPath(workingDir) + inputFiles[i].ToString();
                    if (i != inputFiles.Count - 1)
                    {
                        tmpStr += " ";
                    }
                }
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRAR.exe\Shell\Open\Command");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                the_Info = " a -r -ep1 " + HttpContext.Current.Server.MapPath(rarPath) + " " + tmpStr;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath(workingDir);//获取或设置要启动的进程的初始目录。
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                isOK = true;
            }
            catch
            {
                isOK = false;
            }
            return isOK;
        } 
        #endregion

        #region 在线Rar解压
        /// <summary>
        /// 在线Rar解压
        /// </summary>
        /// <param name="rarPath">目标Rar文件</param>
        /// <param name="outputDir">输出目录</param>
        /// <returns></returns>
        public static bool Rar2(string rarPath, string outputDir)
        {
            bool isOK = false;
            //解压缩
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRar.exe\Shell\Open\Command");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                the_Info = " X " + HttpContext.Current.Server.MapPath(rarPath) + " " + HttpContext.Current.Server.MapPath(outputDir);
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                isOK = true;
            }
            catch
            {
                isOK = false;
            }
            return isOK;
        } 
        #endregion

        /// <summary>
        /// 大文件复制
        /// </summary>
        /// <param name="fromFile">要复制的文件</param>
        /// <param name="toFile">要保存的位置</param>
        public static void CopyFile(string fromFile, string toFile)
        {
            //30M复制
            int fileLength = 30 * 1024 * 1024;
            OZGNet.UtilityIO.CopyFile(HttpContext.Current.Server.MapPath(fromFile), HttpContext.Current.Server.MapPath(toFile), fileLength);
        }

        /// <summary>
        /// 大文件复制
        /// </summary>
        /// <param name="fromFile">要复制的文件</param>
        /// <param name="toFile">要保存的位置</param>
        /// <param name="lengthEachTime">每次复制的长度(单位为字节)</param>
        public static void CopyFile(string fromFile, string toFile, int lengthEachTime)
        {
            OZGNet.UtilityIO.CopyFile(HttpContext.Current.Server.MapPath(fromFile), HttpContext.Current.Server.MapPath(toFile), lengthEachTime);
        }

        /// <summary>
        /// 保存远程图片函数
        /// </summary>
        /// <param name="imgUrl">目标URL图片</param>
        /// <param name="path">保存路径</param>
        /// <returns></returns>
        public static bool SaveImageFromWeb(string imgUrl, string path)
        {
            return OZGNet.UtilityIO.SaveImageFromWeb(imgUrl, HttpContext.Current.Server.MapPath(path));
        }


        #region 将 Stream 写入文件
        /// <summary>
        /// 将 Stream 写入文件 
        /// </summary>
        public static void FO_StreamToFile(Stream stream, string fileName)
        {
            OZGNet.UtilityIO.FO_StreamToFile(stream, HttpContext.Current.Server.MapPath(fileName));
        }
        #endregion

        #region 从文件读取 Stream
        /// <summary>
        /// 从文件读取 Stream
        /// </summary>
        public static Stream FO_FileToStream(string fileName)
        {
            return OZGNet.UtilityIO.FO_FileToStream(HttpContext.Current.Server.MapPath(fileName));
        }
        #endregion

        #region 读取文件的byte[]
        /// <summary>
        /// 读取文件的byte[]
        /// </summary>
        public static byte[] FO_ReadFile(string fileName)
        {
            return OZGNet.UtilityIO.FO_ReadFile(HttpContext.Current.Server.MapPath(fileName));
        }
        #endregion

        #region 用byte[]的方式写入文件
        /// <summary>
        /// 用byte[]的方式写入文件
        /// </summary>
        public static void FO_WriteFile(byte[] pReadByte, string fileName)
        {
            OZGNet.UtilityIO.FO_WriteFile(pReadByte, HttpContext.Current.Server.MapPath(fileName));
        }
        #endregion

        #region 获取一个文件夹的大小,单位为字节
        /// <summary>
        /// 获取一个文件夹的大小,单位为字节
        /// </summary>
        /// <param name="path">目标文件夹</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string path)
        {
            return OZGNet.UtilityIO.GetDirectoryLength(HttpContext.Current.Server.MapPath(path));
        } 
        #endregion

        #region 以服务器的本地路径方式，下载文件
        /// <summary>
        /// 以服务器的本地路径方式，下载文件
        /// </summary>
        /// <param name="ServerLocalPath"></param>
        public static void DownLocalFile(string ServerLocalPath)
        {
            string path = ServerLocalPath;
            FileInfo file_info = new FileInfo(path);
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(file_info.Name, Encoding.UTF8).Replace("+", " "));
        }
        #endregion

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace OZGNet
{
    /// <summary>
    /// Winform和Webform的IO工具类(WinForm和WebForm下面已使用)
    /// </summary>
    public class UtilityIO
    {
        private static StreamReader sr;
        private static StreamWriter sw;

        #region 读取文件
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file_path">目标文件</param>
        /// <param name="encoding">文件编码</param>
        /// <returns></returns>
        public static string FileRead(string file_path, string encoding)
        {
            string str = "";
            if (File.Exists(file_path))
            {
                try
                {
                    sr = new StreamReader(file_path, Encoding.GetEncoding(encoding));
                    str = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
                catch (Exception ex)
                {
                    str = ex.Message;
                }
            }
            else
            {
                if (encoding.ToLower() == "gb2312")
                {
                    str = "文件不存在";
                }
                else
                {
                    str = "File does not exist";
                }
            }
            return str;
        } 
        #endregion

        #region 写入文件
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="file_path">目标路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="encoding">文件编码</param>
        public static void FileWrite(string file_path, string content, string encoding)
        {
            if (File.Exists(file_path))
            {
                FileDelete(file_path);
            }

            try
            {
                sw = new StreamWriter(file_path, false, Encoding.GetEncoding(encoding));
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file_path">目标文件</param>
        public static void FileDelete(string file_path)
        {
            if (File.Exists(file_path))
            {
                try
                {
                    File.Delete(file_path);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        } 
        #endregion

        #region 删除该目录下的所有文件
        /// <summary>
        /// 删除该目录下的所有文件
        /// </summary>
        /// <param name="dir_path">目录路径</param>
        public static void FileDeleteInDir(string dir_path)
        {
            FileInfo[] file_list = (new DirectoryInfo(dir_path)).GetFiles();
            for (int i = 0; i < file_list.Length; i++)
            {
                try
                {
                    file_list[i].Delete();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        } 
        #endregion

        #region 删除该目录下的指定文件
        /// <summary>
        /// 删除该目录下的指定文件
        /// </summary>
        /// <param name="dir_path">目录路径</param>
        /// <param name="separator">分隔符,此字符存在这个文件名则删除</param>
        public static void FileDeleteInDir(string dir_path, string separator)
        {
            FileInfo[] file_list = (new DirectoryInfo(dir_path)).GetFiles();
            for (int i = 0; i < file_list.Length; i++)
            {
                if (file_list[i].Name.Contains(separator))
                {
                    try
                    {
                        file_list[i].Delete();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        } 
        #endregion

        #region 删除文件夹
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir_path">目标文件夹</param>
        /// <param name="recursive">是否清空目标文件夹的文件和文件夹</param>
        public static void DirDelete(string dir_path, bool recursive)
        {
            if (Directory.Exists(dir_path))
            {
                try
                {
                    Directory.Delete(dir_path, recursive);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        } 
        #endregion

        #region 复制文件夹和该文件夹下的所有内容到新的目录
        /// <summary>
        /// 复制文件夹和该文件夹下的所有内容到新的目录
        /// </summary>
        /// <param name="srcPath">文件夹源路径</param>
        /// <param name="aimPath">目标路径</param>
        public static void CopyDirectory(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath)) Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDirectory(file, aimPath + Path.GetFileName(file));
                    // 否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(aimPath);
                throw new Exception(ex.Message);
            }
        } 
        #endregion

        #region 重命名文件夹
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="srcPath">源文件夹路径</param>
        /// <param name="newName">新文件夹名</param>
        public static void ReNameDirectory(string srcPath, string newName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(srcPath);
            string newPath = dirInfo.Parent.FullName + "\\" + newName;
            dirInfo.MoveTo(newPath);
        }
        #endregion

        #region 重命名文件
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="newName">新文件名</param>
        public static void ReNameFile(string srcPath, string newName)
        {
            FileInfo fInfo = new FileInfo(srcPath);
            string newPath = fInfo.Directory.FullName + "\\" + newName;
            fInfo.MoveTo(newPath);
        }
        #endregion

        #region 转换FLV
        /// <summary>
        /// 转换FLV
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="input">输入视频路径</param>
        /// <param name="output">输出FLV路径</param>
        /// <param name="width">FLV的宽</param>
        /// <param name="height">FLV的高</param>
        public static void ChangeFLV(string ffmpeg, string input, string output, string width, string height)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = ffmpeg;
                process.StartInfo.Arguments = " -i " + input + " -ab 56 -ar 22050 -b 500 -r 15 -s " + width + "x" + height + " " + output;
                process.Start();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }  
        #endregion       

        #region 取视频其中的一帧然后生成图片
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
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = ffmpeg;
                process.StartInfo.Arguments = " -i " + video_path + " -y -f image2 -ss " + ss + " -vframes 1 -s " + width + "x" + height + " " + outputIMG;
                process.Start();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
        #endregion
                
        #region 复制大文件
        /// <summary>
        /// 复制大文件
        /// </summary>
        /// <param name="fromFile">要复制的文件</param>
        /// <param name="toFile">要保存的位置</param>
        /// <param name="lengthEachTime">每次复制的长度(单位为字节)</param>
        public static void CopyFile(string fromFile, string toFile, int lengthEachTime)
        {
            //如果保存的位置已存在文件，则删除
            if (File.Exists(toFile))
            {
                File.Delete(toFile);
            }

            FileStream fileToCopy = new FileStream(fromFile, FileMode.Open, FileAccess.Read);
            FileStream copyToFile = new FileStream(toFile, FileMode.Append, FileAccess.Write);
            int lengthToCopy;
            if (lengthEachTime < fileToCopy.Length)//如果分段拷贝，即每次拷贝内容小于文件总长度
            {
                byte[] buffer = new byte[lengthEachTime];
                int copied = 0;
                while (copied <= ((int)fileToCopy.Length - lengthEachTime))//拷贝主体部分
                {
                    lengthToCopy = fileToCopy.Read(buffer, 0, lengthEachTime);
                    fileToCopy.Flush();
                    copyToFile.Write(buffer, 0, lengthEachTime);
                    copyToFile.Flush();
                    copyToFile.Position = fileToCopy.Position;
                    copied += lengthToCopy;
                }
                int left = (int)fileToCopy.Length - copied;//拷贝剩余部分
                lengthToCopy = fileToCopy.Read(buffer, 0, left);
                fileToCopy.Flush();
                copyToFile.Write(buffer, 0, left);
                copyToFile.Flush();
            }
            else//如果整体拷贝，即每次拷贝内容大于文件总长度
            {
                byte[] buffer = new byte[fileToCopy.Length];
                fileToCopy.Read(buffer, 0, (int)fileToCopy.Length);
                fileToCopy.Flush();
                copyToFile.Write(buffer, 0, (int)fileToCopy.Length);
                copyToFile.Flush();
            }
            fileToCopy.Close();
            copyToFile.Close();
        }
        #endregion

        #region 保存远程图片函数
        /// <summary>
        /// 保存远程图片函数
        /// </summary>
        /// <param name="imgUrl">目标URL图片</param>
        /// <param name="path">保存路径</param>
        /// <returns></returns>
        public static bool SaveImageFromWeb(string imgUrl, string path)
        {
            string imgName = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("/") + 1);
            path = path + "\\" + imgName;
            string defaultType = ".jpg";
            string[] imgTypes = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            string imgType = imgUrl.ToString().Substring(imgUrl.ToString().LastIndexOf("."));
            foreach (string it in imgTypes)
            {
                if (imgType.ToLower().Equals(it))
                    break;
                if (it.Equals(".bmp"))
                    imgType = defaultType;
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
                request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (response.ContentType.ToLower().StartsWith("image/"))
                {
                    byte[] arrayByte = new byte[1024];
                    int imgLong = (int)response.ContentLength;
                    int l = 0;
                    // CreateDirectory(path);
                    FileStream fso = new FileStream(path, FileMode.Create);
                    while (l < imgLong)
                    {
                        int i = stream.Read(arrayByte, 0, 1024);
                        fso.Write(arrayByte, 0, i);
                        l += i;
                    }
                    fso.Close();
                    stream.Close();
                    response.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebException)
            {
                return false;
            }
            catch (UriFormatException)
            {
                return false;
            }
        }	 
        #endregion

        #region 将 Stream 转成 byte[]
        /// <summary>
        /// 将 Stream 转成 byte[] 
        /// </summary>
        /// <returns></returns>
        public static byte[] FO_StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            return bytes;
        }
        #endregion

        #region 将 byte[] 转成 Stream
        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream FO_BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        #endregion

        #region 将 Stream 写入文件
        /// <summary>
        /// 将 Stream 写入文件 
        /// </summary>
        public static void FO_StreamToFile(Stream stream, string fileName)
        {
            //如果保存的位置已存在文件，则删除
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
        #endregion

        #region 从文件读取 Stream
        /// <summary>
        /// 从文件读取 Stream
        /// </summary>
        public static Stream FO_FileToStream(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        #endregion

        #region 读取文件的byte[]
        /// <summary>
        /// 读取文件的byte[]
        /// </summary>
        public static byte[] FO_ReadFile(string fileName)
        {
            Stream stream = FO_FileToStream(fileName);
            return FO_StreamToBytes(stream);
        }
        #endregion

        #region 用byte[]的方式写入文件
        /// <summary>
        /// 用byte[]的方式写入文件
        /// </summary>
        public static void FO_WriteFile(byte[] pReadByte, string fileName)
        {
            Stream stream = UtilityIO.FO_BytesToStream(pReadByte);
            FO_StreamToFile(stream, fileName);
        }
        #endregion

        #region 获取一个文件夹的大小,单位为字节
        /// <summary>
        /// 获取一个文件夹的大小,单位为字节
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetDirectoryLength(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            long length = 0;
            foreach (FileSystemInfo fsi in directoryInfo.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    length += ((FileInfo)fsi).Length;
                }
                else
                {
                    length += GetDirectoryLength(fsi.FullName);
                }
            }
            return length;
        }
        #endregion

        #region 统计空间大小的单位
        /// <summary>
        /// 统计空间大小的单位
        /// </summary>
        /// <param name="by">字节</param>
        /// <returns></returns>
        public static string GetSize(long by)
        {
            if (by >= 1024)
            {
                return GetSize((int)by / 1024);
            }
            else
            {
                return by.ToString() + "Byte";
            }
        }
        /// <summary>
        /// 统计空间大小的单位
        /// </summary>
        /// <param name="kb">KB</param>
        /// <returns></returns>
        public static string GetSize(int kb)
        {
            if (kb >= 1024)
            {
                double mb = (double)kb / (double)1024;
                if (mb >= 1024)
                {
                    double gb = (double)mb / (double)1024;
                    if (gb >= 1024)
                    {
                        double tb = (double)gb / (double)1024;
                        return Utility.Get2BitDouble(tb).ToString() + "TB";
                    }
                    else
                    {
                        return Utility.Get2BitDouble(gb).ToString() + "GB";
                    }
                }
                else
                {
                    return Utility.Get2BitDouble(mb).ToString() + "MB";
                }
            }
            else
            {
                return kb.ToString() + "KB";
            }
        }
        #endregion

        #region 获取一个目录的所有FileInfo
        /// <summary>
        /// 获取一个目录的所有FileInfo
        /// </summary>
        /// <param name="dirpath"></param>
        /// <returns></returns>
        public static IList<FileInfo> GetFileInfoList(string dirpath)
        {
            return GetFileInfoList(dirpath, null);
        }
        /// <summary>
        /// 获取一个目录的所有FileInfo(私有方法)
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="fileList"></param>
        /// <returns></returns>
        protected static IList<FileInfo> GetFileInfoList(string dirpath, IList<FileInfo> fileList)
        {
            if (fileList == null)
            {
                fileList = new List<FileInfo>();
            }

            DirectoryInfo workDir = new DirectoryInfo(dirpath);
            foreach (FileInfo f in workDir.GetFiles())
            {
                fileList.Add(f);
            }

            foreach (DirectoryInfo dir in workDir.GetDirectories())
            {
                fileList = GetFileInfoList(dir.FullName, fileList);
            }

            return fileList;
        }
        #endregion

        #region 获取一个目录的所有DirectoryInfo
        /// <summary>
        /// 获取一个目录的所有DirectoryInfo
        /// </summary>
        /// <param name="dirpath"></param>
        /// <returns></returns>
        public static IList<DirectoryInfo> GetDirInfoList(string dirpath)
        {
            return GetDirInfoList(dirpath, null);
        }
        /// <summary>
        /// 获取一个目录的所有DirectoryInfo(私有方法)
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="dirList"></param>
        /// <returns></returns>
        protected static IList<DirectoryInfo> GetDirInfoList(string dirpath, IList<DirectoryInfo> dirList)
        {
            if (dirList == null)
            {
                dirList = new List<DirectoryInfo>();
            }

            DirectoryInfo workDir = new DirectoryInfo(dirpath);
            foreach (DirectoryInfo dir in workDir.GetDirectories())
            {
                dirList.Add(dir);
                dirList = GetDirInfoList(dir.FullName, dirList);
            }
            return dirList;
        }
        #endregion

        #region Bitmap保存到流中出错的解决方法
        /// <summary>
        /// Bitmap保存到流中出错的解决方法
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        public static void BitmapSaveToStream(Bitmap bmp, Stream stream, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            using (ms)
            {
                bmp.Save(ms, format);
                ms.WriteTo(stream);
                bmp.Dispose();
            }
        } 
        #endregion

        #region 替换字符
        /// <summary>
        /// 替换字符
        /// </summary>
        /// <param name="source">源字符</param>
        /// <param name="ht">替换内容</param>
        /// <returns></returns>
        public static string ReplaceContent(string source, Hashtable ht)
        {
            foreach (DictionaryEntry de in ht)
            {
                source = source.Replace(de.Key.ToString(), de.Value.ToString());
            }
            return source;
        } 
        #endregion


    }
}

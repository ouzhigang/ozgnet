using System;
using System.Data;
using System.Net;
using System.Text;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace OZGNet.Net
{
    /// <summary>
    /// FTP操作类
    /// </summary>
    public class FtpClient
    {
        string _ftpPath;
        string _name;
        string _pwd;
        FtpWebRequest request;
        FtpWebResponse response;
        StreamReader sr;

        /// <summary>
        /// 实例化FtpClient
        /// </summary>
        public FtpClient()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ftpPath">目标FTP目录或文件的路径</param>
        /// <param name="name">FTP用户名</param>
        /// <param name="pwd">FTP密码</param>
        public FtpClient(string ftpPath, string name, string pwd)
        {
            this._ftpPath = ftpPath;
            this._name = name;
            this._pwd = pwd;
        }

        /// <summary>
        /// 列表当前FTP目录的文件和文件夹(FTP服务器可以运行NLIST)
        /// </summary>
        /// <returns></returns>
        public string[] FileList()
        {
            return this.FileList(this.FtpPath);
        }

        /// <summary>
        /// 列表当前FTP目录的文件和文件夹(FTP服务器可以运行NLIST)
        /// </summary>
        /// <param name="ftpPath">目标FTP目录或文件的路径</param>
        /// <returns></returns>
        public string[] FileList(string ftpPath)
        {
            List<string> list = new List<string>();
            this.setFtpWebRequest(ftpPath, WebRequestMethods.Ftp.ListDirectoryDetails);
            this.response = (FtpWebResponse)this.request.GetResponse();
            this.sr = new StreamReader(this.response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
            string content = sr.ReadToEnd();
            string[] files = content.Split('\n');
            foreach (string file in files)
            {
                list.Add(file);
            }
            sr.Close();
            sr.Dispose();
            this.response.Close();
            return list.ToArray();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">目标文件的路径(客户端)</param>
        public void Upload(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            this.setFtpWebRequest(this.FtpPath + file.Name, WebRequestMethods.Ftp.UploadFile);
            this.request.KeepAlive = false;
            this.request.ContentLength = file.Length;

            //缓冲区大小 2048为2KB
            int buffLength=2048;
            byte[] buff = new byte[2048];

            int contentLength;
            FileStream fs = file.OpenRead();
            try
            {
                Stream strm = this.request.GetRequestStream();

                // 每次读文件流的2kb
                contentLength = fs.Read(buff, 0, buffLength);

                // 流内容没有结束
                while (contentLength != 0)
                {
                    // 把内容从file stream 写入 upload stream
                    strm.Write(buff, 0, contentLength);

                    contentLength = fs.Read(buff, 0, buffLength);
                }
                // 关闭流
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("上传失败", ex.Message);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">下载文件的文件名</param>
        /// <param name="clientPath">下载路径(客户端)</param>
        public void Download(string filePath, string clientPath)
        {
            FileStream outputStream = new FileStream(clientPath, FileMode.Create);
            this.setFtpWebRequest(this.FtpPath + filePath, WebRequestMethods.Ftp.DownloadFile);
            try
            {
                this.response = (FtpWebResponse)this.request.GetResponse();
                Stream ftpStream = this.response.GetResponseStream();
                long cl = this.response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                this.response.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("下载失败", ex.Message);
            }
        }

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        /// <param name="filePath">目标文件或文件夹名</param>
        public void Delete(string filePath)
        {
            if (OZGNet.Utility.StrSplit(filePath, ".").Count == 1)
            {
                this.setFtpWebRequest(this.FtpPath + filePath, WebRequestMethods.Ftp.RemoveDirectory);
            }
            else
            {
                this.setFtpWebRequest(this.FtpPath + filePath, WebRequestMethods.Ftp.DeleteFile);
            }
            this.request.KeepAlive = false;
            try
            {
                this.response = (FtpWebResponse)this.request.GetResponse();
                this.response.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("删除失败", ex.Message);
            }
        }

        /// <summary>
        /// 修改文件或文件夹
        /// </summary>
        /// <param name="oldName">旧文件名或旧文件夹名</param>
        /// <param name="newName">新文件名或新文件夹名</param>
        public void Rename(string oldName,string newName)
        {
            this.setFtpWebRequest(this.FtpPath + oldName, WebRequestMethods.Ftp.Rename);
            this.request.KeepAlive = false;
            try
            {
                request.RenameTo = newName;
                this.response = (FtpWebResponse)this.request.GetResponse();
                this.response.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("修改失败", ex.Message);
            }
        }

        /// <summary>
        /// 建立目录
        /// </summary>
        /// <param name="dirName">目录名称</param>
        public void MDir(string dirName)
        {
            this.setFtpWebRequest(this.FtpPath + dirName, WebRequestMethods.Ftp.MakeDirectory);
            this.request.KeepAlive = false;
            try
            {
                this.response = (FtpWebResponse)this.request.GetResponse();
                this.response.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("建立目录失败", ex.Message);
            }
        }

        /// <summary>
        /// 私有方法:设置必要的FtpWebRequest属性
        /// </summary>
        /// <param name="ftpPath">目标FTP目录或文件的路径</param>
        /// <param name="method">FTP命令</param>
        protected void setFtpWebRequest(string ftpPath, string method)
        {
            this.request = (FtpWebRequest)FtpWebRequest.Create(ftpPath);
            this.request.Credentials = new NetworkCredential(this.Name, this.Pwd);
            this.request.Method = method;
            this.request.UseBinary = true;
        }

        /// <summary>
        /// 目标FTP目录或文件的路径(最好是FTP目录)
        /// </summary>
        public string FtpPath
        {
            get
            {
                return this._ftpPath;
            }
            set
            {
                this._ftpPath = value;
            }
        }

        /// <summary>
        /// FTP用户名
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// FTP密码
        /// </summary>
        public string Pwd
        {
            get
            {
                return this._pwd;
            }
            set
            {
                this._pwd = value;
            }
        }

    }
}

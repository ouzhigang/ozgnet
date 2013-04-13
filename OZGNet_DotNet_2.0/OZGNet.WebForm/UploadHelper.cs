using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace OZGNet.WebForm
{
    /// <summary>
    /// 欧志上传帮助类
    /// </summary>
    public class UploadHelper
    {
        //web.config的maxRequestLength设置为2097151
        //实例
        /*
            UploadHelper upload = new UploadHelper(FileUpload1.PostedFile);
            upload.SavePath = "/upload/" + FileUpload1.FileName;
            upload.Extensions = ".txt,.jpg.,rar,.iso,.exe,.zip";
            upload.Save();
         */

        private HttpPostedFile _hpf;
        private int _UploadMaxLength = 10240000;
        private string _Extensions = ".rar,.zip,.jpeg,.jpg,.gif,.bmp,.png";
        private string _SavePath;
        private int _BufferSize = 1024;

        /// <summary>
        /// 实例化UploadHelper
        /// </summary>
        public UploadHelper()
        { }
        /// <summary>
        /// 实例化UploadHelper
        /// </summary>
        /// <param name="postFile">HttpPostedFile实例</param>
        public UploadHelper(HttpPostedFile postFile)
        {
            _hpf = postFile;
        }
        /// <summary>
        /// 获取或设置HttpPostedFile实例
        /// </summary>
        public HttpPostedFile PostFile
        {
            get
            {
                return _hpf;
            }
            set
            {
                _hpf = value;
            }
        }

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        public void Save()
        {
            Save(SavePath);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="FilePath">目标路径</param>
        public void Save(string FilePath)
        {
            //判断上传文件大小
            if (PostFile.ContentLength > UploadMaxLength)
            {
                throw new Exception("上传文件过大！");
            }

            //后缀名检测
            bool CheckExtension = false;
            string[] ExtensionList = Extensions.Split(',');
            foreach (string Extension in ExtensionList)
            {
                if (Extension.ToLower() == Path.GetExtension(PostFile.FileName).ToLower())
                {
                    CheckExtension = true;
                    break;
                }
            }
            if (!CheckExtension)
            {
                throw new Exception("禁止上传此文件，请检查上传文件的后缀名");
            }



            //开始上传
            Stream uploadStream = PostFile.InputStream;
            FileStream fileStream = null;

            //如果发现上传文件不存在，就新建文件
            FileInfo tmpFile = new FileInfo(HttpContext.Current.Server.MapPath(SavePath));
            if (!tmpFile.Exists)
            {
                fileStream = File.Create(HttpContext.Current.Server.MapPath(SavePath));
            }
            else
            {
                //文件存在的话，则进行以下两种处理
                //如果已存在的上传文件大于或等于目标文件的大小，则删除，然后重新生成文件，主要是大于或等于就意味着已经上传完毕
                //小于的话则继传
                if (tmpFile.Length >= PostFile.ContentLength)
                {
                    tmpFile.Delete();
                    fileStream = File.Create(HttpContext.Current.Server.MapPath(SavePath));
                }
                else
                {
                    //继传处理
                    fileStream = File.Open(HttpContext.Current.Server.MapPath(SavePath), FileMode.Append);
                    long seek = fileStream.Length;
                    uploadStream.Seek(seek, SeekOrigin.Begin);
                }
            }

            byte[] buffer = new byte[BufferSize];
            int readLength = 0;
            while ((readLength = uploadStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, readLength);
            }


            fileStream.Close();
            fileStream.Dispose();
            uploadStream.Close();
            uploadStream.Dispose();
        }
        #endregion        

        #region 获取上传文件的总大小
        /// <summary>
        /// 获取上传文件的总大小
        /// </summary>
        /// <returns></returns>
        public int GetUploadFileTotalLength()
        {
            return PostFile.ContentLength;
        }
        #endregion

        #region 获取或设置文件上传的大小(单位为字节),默认10240000(10M)
        /// <summary>
        /// 获取或设置文件上传的大小(单位为字节),默认10240000(10M)
        /// </summary>
        public int UploadMaxLength
        {
            get
            {
                return _UploadMaxLength;
            }
            set
            {
                _UploadMaxLength = value;
            }
        }
        #endregion

        #region 获取或设置文件上传的后缀名(如.rar,.zip,.jpg)，默认.rar,.zip,.jpeg,.jpg,.gif,.bmp,.png
        /// <summary>
        /// 获取或设置文件上传的后缀名(如.rar,.zip,.jpg)，默认.rar,.zip,.jpeg,.jpg,.gif,.bmp,.png
        /// </summary>
        public string Extensions
        {
            get
            {
                return _Extensions;
            }
            set
            {
                _Extensions = value;
            }
        }
        #endregion

        #region 获取或设置文件上传的路径（已包括了Server.MapPath）
        /// <summary>
        /// 获取或设置文件上传的路径（已包括了Server.MapPath）
        /// </summary>
        public string SavePath
        {
            get
            {
                return _SavePath;
            }
            set
            {
                _SavePath = value;
            }
        }
        #endregion

        #region 获取或设置上传缓冲区大小,默认为1024
        /// <summary>
        /// 获取或设置上传缓冲区大小,默认为1024
        /// </summary>
        public int BufferSize
        {
            get
            {
                return _BufferSize;
            }
            set
            {
                _BufferSize = value;
            }
        }
        #endregion

    }

}

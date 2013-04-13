using System;
using System.Collections;
using System.Web;

namespace OZGNet.WebForm.ThirdPart.DotBBS
{
    /// <summary>
    /// DotBBS 上传类
    /// </summary>
    public class DotUpload
    {
        private string _allowFileExtens;
        private string _filePath;
        private int _maxFileSize;
        private string _resultFileName;
        private string _resultMessage;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected string CaculatorSize(int s)
        {
            if (s < 0x400)
            {
                return (s + " B");
            }
            if ((s / 0x400) < 0x400)
            {
                return ((s / 0x400) + " KB");
            }
            if (((s / 0x400) / 0x400) < 0x400)
            {
                return (((s / 0x400) / 0x400) + " M");
            }
            if ((((s / 0x400) / 0x400) / 0x400) < 0x400)
            {
                return ((((s / 0x400) / 0x400) / 0x400) + " G");
            }
            return "";
        }
        /// <summary>
        /// 实例化DotUpload
        /// </summary>
        /// <param name="postedFile">HttpPostedFile实例</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool DoUpload(HttpPostedFile postedFile, string type)
        {
            bool flag = true;
            int num = this._maxFileSize;
            string allowExtens = this._allowFileExtens;
            bool flag2 = false;
            try
            {
                if (postedFile.ContentLength > 0)
                {
                    if (postedFile.ContentLength > (num * 0x3e8))
                    {
                        this._resultMessage = "上文件大小超过限定值!(最大<b>" + this.CaculatorSize(num * 0x3e8) + "</b>)";
                        flag2 = true;
                        this._resultFileName = "";
                        flag = false;
                    }
                    if (!flag2 && !this.IsAllowFileExtens(allowExtens, this.GetFileExtens(postedFile.FileName.ToLower())))
                    {
                        this._resultMessage = "上传的文件类型不正确!(只允许上传<b>" + allowExtens + "</b>)";
                        flag2 = true;
                        this._resultFileName = "";
                        flag = false;
                    }
                    if (!flag2 && !this.IsRightFile(postedFile))
                    {
                        this._resultMessage = "上传文件出错!";
                        flag2 = true;
                        this._resultFileName = "";
                        flag = false;
                    }
                    if (!flag2)
                    {
                        this._resultMessage = "上传成功!";
                        string str2 = CommUtil.GetDataTimeRandomFileName() + "." + this.GetFileExtens(postedFile.FileName);
                        string path = this._filePath + "/" + type + str2;
                        postedFile.SaveAs(HttpContext.Current.Server.MapPath(path));
                        this._resultFileName = path.Replace("~", "");
                        flag = true;
                    }
                    return flag;
                }
                this._resultMessage = "请选择文件!";
                this._resultFileName = "";
                return false;
            }
            catch
            {
                this._resultMessage = "上传文件出错!";
                this._resultFileName = "";
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileExtents"></param>
        /// <returns></returns>
        protected string GetExtentsFeatureString(string FileExtents)
        {
            Hashtable standardExtentsFeater = this.GetStandardExtentsFeater();
            foreach (DictionaryEntry entry in standardExtentsFeater)
            {
                if (entry.Key.ToString() == FileExtents)
                {
                    return entry.Value.ToString();
                }
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected string GetFileExtens(string p)
        {
            return p.Substring(p.LastIndexOf(@"\") + 1).Split(new char[] { '.' })[1].ToLower();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Hashtable GetStandardExtentsFeater()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("gif", "image/gif");
            hashtable.Add("jpg", "image/pjpeg");
            hashtable.Add("jpeg", "image/pjpeg");
            hashtable.Add("bmp", "image/bmp");
            hashtable.Add("png", "image/x-png");
            hashtable.Add("tif", "image/tiff");
            hashtable.Add("tiff", "image/tiff");
            hashtable.Add("zip", "application/x-zip-compressed");
            hashtable.Add("rar", "application/octet-stream");
            return hashtable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowExtens"></param>
        /// <param name="nowExtens"></param>
        /// <returns></returns>
        protected bool IsAllowFileExtens(string allowExtens, string nowExtens)
        {
            return (allowExtens.IndexOf(nowExtens) > -1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        protected bool IsRightFile(HttpPostedFile postedFile)
        {
            string fileExtens = this.GetFileExtens(postedFile.FileName);
            string extentsFeatureString = this.GetExtentsFeatureString(fileExtens);
            return (postedFile.ContentType == extentsFeatureString);
        }
        /// <summary>
        /// 
        /// </summary>
        public string AllowFileExtens
        {
            set
            {
                this._allowFileExtens = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            set
            {
                this._filePath = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaxFileSize
        {
            set
            {
                this._maxFileSize = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResultFileName
        {
            get
            {
                return this._resultFileName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResultMessage
        {
            get
            {
                return this._resultMessage;
            }
        }
    }
}


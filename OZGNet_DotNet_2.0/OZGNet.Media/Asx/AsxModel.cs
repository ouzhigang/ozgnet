using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Media.Asx
{
    /// <summary>
    /// Asx实体类
    /// </summary>
    public class AsxModel
    {
        private string _title;
        private string _author;
        private string _copyright;
        private string _url;
        private string _artist;
        private string _album;

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
        /// <summary>
        /// 获取或设置作者
        /// </summary>
        public string Author
        {
            get
            {
                return this._author;
            }
            set
            {
                this._author = value;
            }
        }
        /// <summary>
        /// 获取或设置版权
        /// </summary>
        public string Copyright
        {
            get
            {
                return this._copyright;
            }
            set
            {
                this._copyright = value;
            }
        }
        /// <summary>
        /// 获取或设置URL
        /// </summary>
        public string URL
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }
        /// <summary>
        /// 获取或设置Artist
        /// </summary>
        public string Artist
        {
            get
            {
                return this._artist;
            }
            set
            {
                this._artist = value;
            }
        }
        /// <summary>
        /// 获取或设置Album
        /// </summary>
        public string Album
        {
            get
            {
                return this._album;
            }
            set
            {
                this._album = value;
            }
        }

    }
}

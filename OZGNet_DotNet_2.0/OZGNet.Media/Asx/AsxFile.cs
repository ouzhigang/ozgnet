using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Media.Asx
{
    /// <summary>
    /// Asx帮助类
    /// </summary>
    public class AsxFile
    {
        IList<AsxModel> list = null;
        string title;
        string author;
        string _abstract;
        string copyright;
        /// <summary>
        /// 实例化AsxFile
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="author">作者</param>
        /// <param name="_abstract">提要</param>
        /// <param name="copyright">版权</param>
        public AsxFile(string title, string author, string _abstract, string copyright)
        {
            this.title = title;
            this.author = author;
            this._abstract = _abstract;
            this.copyright = copyright;
        }
        /// <summary>
        /// 实例化AsxFile
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="author">作者</param>
        /// <param name="_abstract">提要</param>
        /// <param name="copyright">版权</param>
        /// <param name="list">AsxModel列表</param>
        public AsxFile(string title, string author, string _abstract, string copyright, IList<AsxModel> list)
        {
            this.title = title;
            this.author = author;
            this._abstract = _abstract;
            this.copyright = copyright;
            this.list = list;
        }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
        /// <summary>
        /// 获取或设置作者
        /// </summary>
        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }
        /// <summary>
        /// 获取或设置版权
        /// </summary>
        public string Copyright
        {
            get
            {
                return this.copyright;
            }
            set
            {
                this.copyright = value;
            }
        }
        /// <summary>
        /// 获取或设置提要
        /// </summary>
        public string Abstract
        {
            get
            {
                return this._abstract;
            }
            set
            {
                this._abstract = value;
            }
        }
        /// <summary>
        /// 获取或设置AsxModel列表
        /// </summary>
        public IList<AsxModel> AsxList
        {
            get
            {
                return this.list;
            }
            set
            {
                this.list = value;
            }
        }

        /// <summary>
        /// 输出Asx内容
        /// </summary>
        /// <returns></returns>
        public string OutputContent()
        {
            string content = "<ASX version = \"3.0\">\r\n";
            content += "<TITLE>" + title + "</TITLE>\r\n";
            content += "<author>" + author + "</author>\r\n";
            content += "<abstract>" + _abstract + "</abstract>\r\n";
            content += "<copyright>" + copyright + "</copyright> \r\n";
            foreach (AsxModel model in this.list)
            {
                content += "<ENTRY> \r\n";
                content += "<TITLE>" + model.Title + "</TITLE>\r\n";
                content += "<AUTHOR>" + model.Author + "</AUTHOR>\r\n";
                content += "<copyright>" + model.Copyright + "</copyright>\r\n";
                content += "<REF HREF=\"" + model.URL + "\"/>\r\n";
                content += "<param name=\"Artist\" value=\"" + model.Artist + "\"/> \r\n";
                content += "<param name=\"Album\" value=\"" + model.Album + "\"/>\r\n";
                content += "<param name=\"Title\" value=\"" + model.Title + "\"/> \r\n";
                content += "</ENTRY>\r\n";
            }
            content += "</ASX>\r\n";
            return content;
        }

    }
}

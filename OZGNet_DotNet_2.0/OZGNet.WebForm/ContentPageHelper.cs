using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace OZGNet.WebForm
{
    /// <summary>
    /// 长文章內容分页类
    /// </summary>
    public class ContentPageHelper
    {
        string _PageIndexName = "p";
        HyperLink _first = null;
        HyperLink _prev = null;
        HyperLink _next = null;
        HyperLink _last = null;
        Button _first_btn = null;
        Button _prev_btn = null;
        Button _next_btn = null;
        Button _last_btn = null;
        LinkButton _first_lbtn = null;
        LinkButton _prev_lbtn = null;
        LinkButton _next_lbtn = null;
        LinkButton _last_lbtn = null;
        ImageButton _first_img = null;
        ImageButton _prev_img = null;
        ImageButton _next_img = null;
        ImageButton _last_img = null;
        string _query = null;
        string _alert1 = "已经是第一页了";
        string _alert2 = "已经是最后一页了";
        int _recordCount;
        int _pageSize;

        /// <summary>
        /// 实例化ContentPageHelper
        /// </summary>
        public ContentPageHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region 分页含含
        /// <summary>
        /// 分页含含
        /// </summary>
        /// <param name="Content">文章内容</param>
        /// <param name="PageSplit">分隔符</param>
        /// <returns></returns>
        public string Paging(string Content, string PageSplit)
        {
            return Paging(Content, PageSplit, this.Query);
        }
        /// <summary>
        /// 分页含含
        /// </summary>
        /// <param name="Content">文章内容</param>
        /// <param name="PageSplit">分隔符</param>
        /// <param name="Query">URL参数</param>
        /// <returns></returns>
        public string Paging(string Content, string PageSplit, string Query)
        {
            string[] ContentSplit = OZGNet.WebForm.Utility.StrSplit(Content, PageSplit).ToArray();
            Content = ContentSplit[this.PageIndex - 1];

            this._recordCount = ContentSplit.Length;
            this._pageSize = 1;


            this.Query = Query;

            if (RecordCount > 0)
            {
                if (PageIndex != 1)
                {
                    //首页
                    if (First != null)
                    {
                        First.NavigateUrl = filepath + "?" + PageIndexName + "=1" + Query;
                    }
                    else if (FirstBtn != null)
                    {
                        FirstBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=1" + Query + "';return false;";
                    }
                    else if (FirstLbtn != null)
                    {
                        FirstLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=1" + Query + "';return false;";
                    }
                    else if (FirstImg != null)
                    {
                        FirstImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=1" + Query + "';return false;";
                    }

                    //上一页
                    if (Prev != null)
                    {
                        Prev.NavigateUrl = filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + Query;
                    }
                    else if (PrevBtn != null)
                    {
                        PrevBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + Query + "';return false;";
                    }
                    else if (PrevLbtn != null)
                    {
                        PrevLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + Query + "';return false;";
                    }
                    else if (PrevImg != null)
                    {
                        PrevImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + Query + "';return false;";
                    }

                    //最后一页和下一页的按扭超出分页索引的处理
                    if (PageIndex >= PageCount)
                    {
                        string js = "alert('" + Alert2 + "');history.go(-1);";
                        if (NextBtn != null)
                        {
                            NextBtn.OnClientClick = js;
                        }
                        else if (NextLbtn != null)
                        {
                            NextLbtn.OnClientClick = js;
                        }
                        else if (NextImg != null)
                        {
                            NextImg.OnClientClick = js;
                        }

                        if (LastBtn != null)
                        {
                            LastBtn.OnClientClick = js;
                        }
                        else if (LastLbtn != null)
                        {
                            LastLbtn.OnClientClick = js;
                        }
                        else if (LastImg != null)
                        {
                            LastImg.OnClientClick = js;
                        }
                    }

                }
                if (PageIndex != PageCount)
                {
                    //下一页
                    if (Next != null)
                    {
                        Next.NavigateUrl = filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + Query;
                    }
                    else if (NextBtn != null)
                    {
                        NextBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + Query + "';return false;";
                    }
                    else if (NextLbtn != null)
                    {
                        NextLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + Query + "';return false;";
                    }
                    else if (NextImg != null)
                    {
                        NextImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + Query + "';return false;";
                    }

                    //末页
                    if (Last != null)
                    {
                        Last.NavigateUrl = filepath + "?" + PageIndexName + "=" + PageCount.ToString() + Query;
                    }
                    else if (LastBtn != null)
                    {
                        LastBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + PageCount.ToString() + Query + "';return false;";
                    }
                    else if (LastLbtn != null)
                    {
                        LastLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + PageCount.ToString() + Query + "';return false;";
                    }
                    else if (LastImg != null)
                    {
                        LastImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + PageCount.ToString() + Query + "';return false;";
                    }

                    //第一页和上一页的按扭超出分页索引的处理
                    if (PageIndex <= 1)
                    {
                        string js = "alert('" + Alert1 + "');history.go(-1);";
                        if (FirstBtn != null)
                        {
                            FirstBtn.OnClientClick = js;
                        }
                        else if (FirstLbtn != null)
                        {
                            FirstLbtn.OnClientClick = js;
                        }
                        else if (FirstImg != null)
                        {
                            FirstImg.OnClientClick = js;
                        }

                        if (PrevBtn != null)
                        {
                            PrevBtn.OnClientClick = js;
                        }
                        else if (PrevLbtn != null)
                        {
                            PrevLbtn.OnClientClick = js;
                        }
                        else if (PrevImg != null)
                        {
                            PrevImg.OnClientClick = js;
                        }
                    }

                }
            }

            return Content;
        }
        #endregion

        #region 当前请求文件
        /// <summary>
        /// 当前请求文件
        /// </summary>
        protected string filepath
        {
            get
            {
                return HttpContext.Current.Request.CurrentExecutionFilePath.Substring(HttpContext.Current.Request.CurrentExecutionFilePath.LastIndexOf("/")).Replace("/", "");
            }
        }
        #endregion

        #region url参数,最前面不用加&(读写)
        /// <summary>
        /// url参数,最前面不用加＆(读写)
        /// </summary>
        public string Query
        {
            get
            {
                if (!String.IsNullOrEmpty(_query))
                {
                    return "&" + _query;
                }
                else
                {
                    return _query;
                }
            }
            set
            {
                _query = value;
            }
        }
        #endregion

        #region 记录总数(只读)
        /// <summary>
        /// 记录总数(只读)
        /// </summary>
        public int RecordCount
        {
            get
            {
                return _recordCount;
            }
        }
        #endregion

        #region 当前页索引(只读)
        /// <summary>
        /// 当前页索引(只读)
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[PageIndexName]))
                {
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(HttpContext.Current.Request.QueryString[PageIndexName]);
                }
            }
        }
        #endregion

        #region 总页数(只读)
        /// <summary>
        /// 总页数(只读)
        /// </summary>
        public int PageCount
        {
            get
            {
                if (RecordCount % this._pageSize == 0)
                {
                    return RecordCount / this._pageSize;
                }
                else
                {
                    return (RecordCount / this._pageSize) + 1;
                }
            }
        }
        #endregion

        #region 第一页和上一页的按扭超出分页索引弹出的信息
        /// <summary>
        /// 第一页和上一页的按扭超出分页索引弹出的信息
        /// </summary>
        public string Alert1
        {
            get
            {
                return _alert1;
            }
            set
            {
                _alert1 = value;
            }
        }
        #endregion


        #region 最后一页和下一页的按扭超出分页索引弹出的信息
        /// <summary>
        /// 最后一页和下一页的按扭超出分页索引弹出的信息
        /// </summary>
        public string Alert2
        {
            get
            {
                return _alert2;
            }
            set
            {
                _alert2 = value;
            }
        }
        #endregion

        #region 获取或设置URL分页索引参数的名称,默认为p
        /// <summary>
        /// 获取或设置URL分页索引参数的名称,默认为p
        /// </summary>
        public string PageIndexName
        {
            get
            {
                return _PageIndexName;
            }
            set
            {
                _PageIndexName = value;
            }
        }
        #endregion

        #region 分页属性
        /// <summary>
        /// 第一页(普通连接方式)
        /// </summary>
        public HyperLink First
        {
            get
            {
                return _first;
            }
            set
            {
                _first = value;
            }
        }
        /// <summary>
        /// 上一页(普通连接方式)
        /// </summary>
        public HyperLink Prev
        {
            get
            {
                return _prev;
            }
            set
            {
                _prev = value;
            }
        }
        /// <summary>
        /// 下一页(普通连接方式)
        /// </summary>
        public HyperLink Next
        {
            get
            {
                return _next;
            }
            set
            {
                _next = value;
            }
        }
        /// <summary>
        /// 末页(普通连接方式)
        /// </summary>
        public HyperLink Last
        {
            get
            {
                return _last;
            }
            set
            {
                _last = value;
            }
        }

        /// <summary>
        /// 第一页(按钮方式)
        /// </summary>
        public Button FirstBtn
        {
            get
            {
                return _first_btn;
            }
            set
            {
                _first_btn = value;
            }
        }
        /// <summary>
        /// 上一页(按钮方式)
        /// </summary>
        public Button PrevBtn
        {
            get
            {
                return _prev_btn;
            }
            set
            {
                _prev_btn = value;
            }
        }
        /// <summary>
        /// 下一页(按钮方式)
        /// </summary>
        public Button NextBtn
        {
            get
            {
                return _next_btn;
            }
            set
            {
                _next_btn = value;
            }
        }
        /// <summary>
        /// 末页(按钮方式)
        /// </summary>
        public Button LastBtn
        {
            get
            {
                return _last_btn;
            }
            set
            {
                _last_btn = value;
            }
        }

        /// <summary>
        /// 第一页(连接按钮方式)
        /// </summary>
        public LinkButton FirstLbtn
        {
            get
            {
                return _first_lbtn;
            }
            set
            {
                _first_lbtn = value;
            }
        }
        /// <summary>
        /// 上一页(连接按钮方式)
        /// </summary>
        public LinkButton PrevLbtn
        {
            get
            {
                return _prev_lbtn;
            }
            set
            {
                _prev_lbtn = value;
            }
        }
        /// <summary>
        /// 下一页(连接按钮方式)
        /// </summary>
        public LinkButton NextLbtn
        {
            get
            {
                return _next_lbtn;
            }
            set
            {
                _next_lbtn = value;
            }
        }
        /// <summary>
        /// 末页(连接按钮方式)
        /// </summary>
        public LinkButton LastLbtn
        {
            get
            {
                return _last_lbtn;
            }
            set
            {
                _last_lbtn = value;
            }
        }

        /// <summary>
        /// 第一页(图片按钮方式)
        /// </summary>
        public ImageButton FirstImg
        {
            get
            {
                return _first_img;
            }
            set
            {
                _first_img = value;
            }
        }
        /// <summary>
        /// 上一页(图片按钮方式)
        /// </summary>
        public ImageButton PrevImg
        {
            get
            {
                return _prev_img;
            }
            set
            {
                _prev_img = value;
            }
        }
        /// <summary>
        /// 下一页(图片按钮方式)
        /// </summary>
        public ImageButton NextImg
        {
            get
            {
                return _next_img;
            }
            set
            {
                _next_img = value;
            }
        }
        /// <summary>
        /// 末页(图片按钮方式)
        /// </summary>
        public ImageButton LastImg
        {
            get
            {
                return _last_img;
            }
            set
            {
                _last_img = value;
            }
        }
        #endregion

    }
}

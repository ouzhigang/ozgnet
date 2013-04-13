using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

/*

实例:
public OZGNet.WebForm.PageHelper p;
protected void Page_Load(object sender, EventArgs e)
{
    p = new OZGNet.WebForm.PageHelper();
    p.first = first;
    p.prev = prev;
    p.next = next;
    p.last = last;
    //p.Query = "name=123&pwd=456";    
    p.pageSize = 10;

    //设置总记录数
    //p.recordCount;

    //显示数据部分

    p.paging();
}

*/

namespace OZGNet.WebForm
{
    /// <summary>
    /// 欧志分页类
    /// </summary>
    public class PageHelper
    {
        string _PageIndexName = "p";
        string _UrlRewritePath = null;
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
        int _recordCount;
        int _pageSize = 10;

        #region 构造函数
        /// <summary>
        /// 实例化PageHelper
        /// </summary>
        public PageHelper()
        { }
        /// <summary>
        /// 实例化PageHelper(普通连接方式)
        /// </summary>
        /// <param name="First">第一页</param>
        /// <param name="Prev">上一页</param>
        /// <param name="Next">下一页</param>
        /// <param name="Last">末页</param>
        public PageHelper(HyperLink First, HyperLink Prev, HyperLink Next, HyperLink Last)
        {
            _first = First;
            _prev = Prev;
            _next = Next;
            _last = Last;
        }
        /// <summary>
        /// 实例化PageHelper(按钮方式)
        /// </summary>
        /// <param name="First_BTN">第一页</param>
        /// <param name="Prev_BTN">上一页</param>
        /// <param name="Next_BTN">下一页</param>
        /// <param name="Last_BTN">末页</param>
        public PageHelper(Button First_BTN, Button Prev_BTN, Button Next_BTN, Button Last_BTN)
        {
            _first_btn = First_BTN;
            _prev_btn = Prev_BTN;
            _next_btn = Next_BTN;
            _last_btn = Last_BTN;
        }
        /// <summary>
        /// 实例化PageHelper(连接按钮方式)
        /// </summary>
        /// <param name="First_LBTN">第一页</param>
        /// <param name="Prev_LBTN">上一页</param>
        /// <param name="Next_LBTN">下一页</param>
        /// <param name="Last_LBTN">末页</param>
        public PageHelper(LinkButton First_LBTN, LinkButton Prev_LBTN, LinkButton Next_LBTN, LinkButton Last_LBTN)
        {
            _first_lbtn = First_LBTN;
            _prev_lbtn = Prev_LBTN;
            _next_lbtn = Next_LBTN;
            _last_lbtn = Last_LBTN;
        }
        /// <summary>
        /// 实例化PageHelper(图片按钮方式)
        /// </summary>
        /// <param name="First_IMG">第一页</param>
        /// <param name="Prev_IMG">上一页</param>
        /// <param name="Next_IMG">下一页</param>
        /// <param name="Last_IMG">末页</param>
        public PageHelper(ImageButton First_IMG, ImageButton Prev_IMG, ImageButton Next_IMG, ImageButton Last_IMG)
        {
            _first_img = First_IMG;
            _prev_img = Prev_IMG;
            _next_img = Next_IMG;
            _last_img = Last_IMG;
        } 
        #endregion

        #region 分页含含
        /// <summary>
        /// 分页含含
        /// </summary>
        public void Paging()
        {
            if (string.IsNullOrEmpty(UrlRewritePath))
            {
                pageGeneral();
            }
            else
            {
                pageUrlRewrite();
            }
        } 
        /// <summary>
        /// 分页含含
        /// </summary>
        /// <param name="Query">url参数,最前面不用加＆</param>
        public void Paging(string Query)
        {
            if (!String.IsNullOrEmpty(Query))
            {
                _query = Query;
            }

            if (string.IsNullOrEmpty(UrlRewritePath))
            {
                pageGeneral();
            }
            else
            {
                pageUrlRewrite();
            }
        } 
        #endregion

        #region 一般分页方法
        /// <summary>
        /// 分页方法
        /// </summary>
        protected void pageGeneral()
        {
            if (RecordCount > 0)
            {
                if (PageIndex != 1)
                {
                    //首页
                    if (First != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            First.NavigateUrl = filepath + "?" + PageIndexName + "=1";
                        }
                        else
                        {
                            First.NavigateUrl = filepath + "?" + Query + "&" + PageIndexName + "=1";
                        }
                    }
                    else if (FirstBtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            FirstBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=1" + "';return false;";
                        }
                        else
                        {
                            FirstBtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=1" + "';return false;";
                        }
                    }
                    else if (FirstLbtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            FirstLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=1" + "';return false;";
                        }
                        else
                        {
                            FirstLbtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=1" + "';return false;";
                        }
                    }
                    else if (FirstImg != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            FirstImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=1" + "';return false;";
                        }
                        else
                        {
                            FirstImg.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=1" + "';return false;";
                        }
                    }

                    //上一页
                    if (Prev != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            Prev.NavigateUrl = filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1);
                        }
                        else
                        {
                            Prev.NavigateUrl = filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex - 1);
                        }
                    }
                    else if (PrevBtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            PrevBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + "';return false;";
                        }
                        else
                        {
                            PrevBtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + "';return false;";
                        }
                    }
                    else if (PrevLbtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            PrevLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + "';return false;";
                        }
                        else
                        {
                            PrevLbtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + "';return false;";
                        }
                    }
                    else if (PrevImg != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            PrevImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + "';return false;";
                        }
                        else
                        {
                            PrevImg.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex - 1) + "';return false;";
                        }
                    }

                    //最后一页和下一页的按扭超出分页索引的处理
                    if (PageIndex >= PageCount)
                    {
                        if (NextBtn != null)
                        {
                            NextBtn.Enabled = false;
                        }
                        else if (NextLbtn != null)
                        {
                            NextLbtn.Enabled = false;
                        }
                        else if (NextImg != null)
                        {
                            NextImg.Enabled = false;
                        }

                        if (LastBtn != null)
                        {
                            LastBtn.Enabled = false;
                        }
                        else if (LastLbtn != null)
                        {
                            LastLbtn.Enabled = false;
                        }
                        else if (LastImg != null)
                        {
                            LastImg.Enabled = false;
                        }
                    }

                }
                if (PageIndex != PageCount)
                {
                    //下一页
                    if (Next != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            Next.NavigateUrl = filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1);
                        }
                        else
                        {
                            Next.NavigateUrl = filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex + 1);
                        }
                    }
                    else if (NextBtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            NextBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + "';return false;";
                        }
                        else
                        {
                            NextBtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + "';return false;";
                        }
                    }
                    else if (NextLbtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            NextLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + "';return false;";
                        }
                        else
                        {
                            NextLbtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + "';return false;";
                        }
                    }
                    else if (NextImg != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            NextImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + "';return false;";
                        }
                        else
                        {
                            NextImg.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + Convert.ToString(PageIndex + 1) + "';return false;";
                        }
                    }

                    //末页
                    if (Last != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            Last.NavigateUrl = filepath + "?" + PageIndexName + "=" + PageCount.ToString();
                        }
                        else
                        {
                            Last.NavigateUrl = filepath + "?" + Query + "&" + PageIndexName + "=" + PageCount.ToString();
                        }
                    }
                    else if (LastBtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            LastBtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + PageCount.ToString() + "';return false;";
                        }
                        else
                        {
                            LastBtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + PageCount.ToString() + "';return false;";
                        }
                    }
                    else if (LastLbtn != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            LastLbtn.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + PageCount.ToString() + "';return false;";
                        }
                        else
                        {
                            LastLbtn.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + PageCount.ToString() + "';return false;";
                        }
                    }
                    else if (LastImg != null)
                    {
                        if (string.IsNullOrEmpty(Query))
                        {
                            LastImg.OnClientClick = "self.location='" + filepath + "?" + PageIndexName + "=" + PageCount.ToString() + "';return false;";
                        }
                        else
                        {
                            LastImg.OnClientClick = "self.location='" + filepath + "?" + Query + "&" + PageIndexName + "=" + PageCount.ToString() + "';return false;";
                        }
                    }

                    //第一页和上一页的按扭超出分页索引的处理
                    if (PageIndex <= 1)
                    {
                        if (FirstBtn != null)
                        {
                            FirstBtn.Enabled = false;
                        }
                        else if (FirstLbtn != null)
                        {
                            FirstLbtn.Enabled = false;
                        }
                        else if (FirstImg != null)
                        {
                            FirstImg.Enabled = false;
                        }

                        if (PrevBtn != null)
                        {
                            PrevBtn.Enabled = false;
                        }
                        else if (PrevLbtn != null)
                        {
                            PrevLbtn.Enabled = false;
                        }
                        else if (PrevImg != null)
                        {
                            PrevImg.Enabled = false;
                        }
                    }
                }
            }
        } 
        #endregion

        #region URL重写分页
        /// <summary>
        /// URL重写分页
        /// </summary>
        protected void pageUrlRewrite()
        {
            if (RecordCount > 0)
            {
                if (PageIndex != 1)
                {
                    //首页
                    if (First != null)
                    {
                        First.NavigateUrl = GetUrlRewritePath(1);
                    }
                    else if (FirstBtn != null)
                    {
                        FirstBtn.OnClientClick = "self.location='" + GetUrlRewritePath(1) + "';return false;";
                    }
                    else if (FirstLbtn != null)
                    {
                        FirstLbtn.OnClientClick = "self.location='" + GetUrlRewritePath(1) + "';return false;";
                    }
                    else if (FirstImg != null)
                    {
                        FirstImg.OnClientClick = "self.location='" + GetUrlRewritePath(1) + "';return false;";
                    }

                    //上一页
                    if (Prev != null)
                    {
                        Prev.NavigateUrl = GetUrlRewritePath(PageIndex - 1);
                    }
                    else if (PrevBtn != null)
                    {
                        PrevBtn.OnClientClick = "self.location='" + GetUrlRewritePath(PageIndex - 1) + "';return false;";
                    }
                    else if (PrevLbtn != null)
                    {
                        PrevLbtn.OnClientClick = "self.location='" + GetUrlRewritePath(PageIndex - 1) + "';return false;";
                    }
                    else if (PrevImg != null)
                    {
                        PrevImg.OnClientClick = "self.location='" + GetUrlRewritePath(PageIndex - 1) + "';return false;";
                    }

                    //最后一页和下一页的按扭超出分页索引的处理
                    if (PageIndex >= PageCount)
                    {
                        if (NextBtn != null)
                        {
                            NextBtn.Enabled = false;
                        }
                        else if (NextLbtn != null)
                        {
                            NextLbtn.Enabled = false;
                        }
                        else if (NextImg != null)
                        {
                            NextImg.Enabled = false;
                        }

                        if (LastBtn != null)
                        {
                            LastBtn.Enabled = false;
                        }
                        else if (LastLbtn != null)
                        {
                            LastLbtn.Enabled = false;
                        }
                        else if (LastImg != null)
                        {
                            LastImg.Enabled = false;
                        }
                    }

                }
                if (PageIndex != PageCount)
                {
                    //下一页
                    if (Next != null)
                    {
                        Next.NavigateUrl = GetUrlRewritePath(PageIndex + 1);
                    }
                    else if (NextBtn != null)
                    {
                        NextBtn.OnClientClick = "self.location='" + GetUrlRewritePath(PageIndex + 1) +"';return false;";
                    }
                    else if (NextLbtn != null)
                    {
                        NextLbtn.OnClientClick = "self.location='" + GetUrlRewritePath(PageIndex + 1) + "';return false;";
                    }
                    else if (NextImg != null)
                    {
                        NextImg.OnClientClick = "self.location='" + GetUrlRewritePath(PageIndex + 1) + "';return false;";
                    }

                    //末页
                    if (Last != null)
                    {
                        Last.NavigateUrl = GetUrlRewritePath(PageCount);
                    }
                    else if (LastBtn != null)
                    {
                        LastBtn.OnClientClick = "self.location='" + GetUrlRewritePath(PageCount) + "';return false;";
                    }
                    else if (LastLbtn != null)
                    {
                        LastLbtn.OnClientClick = "self.location='" + GetUrlRewritePath(PageCount) + "';return false;";
                    }
                    else if (LastImg != null)
                    {
                        LastImg.OnClientClick = "self.location='" + GetUrlRewritePath(PageCount) + "';return false;";
                    }

                    //第一页和上一页的按扭超出分页索引的处理
                    if (PageIndex <= 1)
                    {
                        if (FirstBtn != null)
                        {
                            FirstBtn.Enabled = false;
                        }
                        else if (FirstLbtn != null)
                        {
                            FirstLbtn.Enabled = false;
                        }
                        else if (FirstImg != null)
                        {
                            FirstImg.Enabled = false;
                        }

                        if (PrevBtn != null)
                        {
                            PrevBtn.Enabled = false;
                        }
                        else if (PrevLbtn != null)
                        {
                            PrevLbtn.Enabled = false;
                        }
                        else if (PrevImg != null)
                        {
                            PrevImg.Enabled = false;
                        }
                    }
                }
            }
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
                //if (!String.IsNullOrEmpty(_query))
                //{
                //    return _query + "&";
                //}
                //else
                //{
                //    return _query;
                //}
                return _query;
            }
            set
            {
                _query = value;
            }
        } 
        #endregion

        #region 记录总数(读写)
        /// <summary>
        /// 记录总数(读写)
        /// </summary>
        public int RecordCount
        {
            get
            {
                return _recordCount;
            }
            set
            {
                _recordCount = value;
            }
        } 
        #endregion

        #region 每页记录数(读写)
        /// <summary>
        /// 每页记录数(读写)
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
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
                if (RecordCount % PageSize == 0)
                {
                    return RecordCount / PageSize;
                }
                else
                {
                    return (RecordCount / PageSize) + 1;
                }
            }
        } 
        #endregion

        #region 使用URL重写
        /// <summary>
        /// 使用URL重写（这里的$n参数和Query的顺序一致）
        /// </summary>
        /// <param name="CurrentPageIndex"></param>
        /// <returns></returns>
        protected string GetUrlRewritePath(int CurrentPageIndex)
        {
            //xxx-$1-$2-$3-{PageIndex}.html
            //转成
            //xxx-1-2-3-1.html
            
            string TmpUrlRewritePath = UrlRewritePath;

            TmpUrlRewritePath = TmpUrlRewritePath.Replace("{PageIndex}", CurrentPageIndex.ToString());

            if (!TmpUrlRewritePath.Contains("$") || string.IsNullOrEmpty(Query))
            {
                return TmpUrlRewritePath;
            }

            int i = 1;
            Dictionary<string, string> QueryMap = UrlStrToMap(Query);
            foreach (KeyValuePair<string, string> kv in QueryMap)
            {
                TmpUrlRewritePath = TmpUrlRewritePath.Replace("$" + i.ToString(), kv.Value);
                i++;
            }

            return TmpUrlRewritePath;
        }
        #endregion

        #region url参数转成map
        /// <summary>
        /// url参数转成map
        /// </summary>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        protected Dictionary<string, string> UrlStrToMap(string urlStr)
        {
            string[] strs = urlStr.Split('&');
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (string item in strs)
            {
                try
                {
                    string[] kv = item.Split('=');
                    map.Add(kv[0], kv[1]);
                }
                catch
                {

                }
            }
            return map;
        } 
        #endregion

        #region 属性
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

        /// <summary>
        /// 获取或设置UrlRewritePath，格式xxx-$1-$2-$3-{PageIndex}.html (最终转成xxx-1-2-3-1.html)
        /// </summary>
        public string UrlRewritePath
        {
            get
            {
                return _UrlRewritePath;
            }
            set
            {
                _UrlRewritePath = value;
            }
        }

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

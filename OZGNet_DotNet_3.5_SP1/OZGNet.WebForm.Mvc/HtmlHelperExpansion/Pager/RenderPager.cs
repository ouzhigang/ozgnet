using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OZGNet.WebForm.Mvc.HtmlHelperExpansion.Pager
{
    public class RenderPager
    {
        #region 字段

        /// <summary>   
        /// 当前页面的ViewContext   
        /// </summary>   
        private ViewContext viewContext;
        /// <summary>   
        /// 当前页码   
        /// </summary>   
        private int currentPage;
        /// <summary>   
        /// 页面要显示的数据条数   
        /// </summary>   
        private int pageSize;
        /// <summary>   
        /// 总的记录数   
        /// </summary>   
        private int totalCount;
        /// <summary>   
        /// Pager Helper 要显示的页数   
        /// </summary>   
        private int toDisplayCount;

        private string pagelink;

        private string separated = " ";

        #endregion

        #region 构造函数
        public RenderPager(ViewContext viewContext, int currentPage, int pageSize, int totalCount, int toDisplayCount)
        {
            Init(viewContext, currentPage, pageSize, totalCount, toDisplayCount, separated);
        }
        public RenderPager(ViewContext viewContext, int currentPage, int pageSize, int totalCount, int toDisplayCount, string separated)
        {
            Init(viewContext, currentPage, pageSize, totalCount, toDisplayCount, separated);
        }

        protected void Init(ViewContext viewContext, int currentPage, int pageSize, int totalCount, int toDisplayCount, string separated)
        {
            this.viewContext = viewContext;
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.totalCount = totalCount;
            this.toDisplayCount = toDisplayCount;
            this.separated = separated;

            string reqUrl = viewContext.RequestContext.HttpContext.Request.RawUrl;
            string link = "";

            Regex re = new Regex(@"page=(\d+)|page=", RegexOptions.IgnoreCase);

            MatchCollection results = re.Matches(reqUrl);

            if (results.Count > 0)
            {
                link = reqUrl.Replace(results[0].ToString(), "page=[%page%]");
            }
            else if (reqUrl.IndexOf("?") < 0)
            {
                link = reqUrl + "?page=[%page%]";
            }
            else
            {
                link = reqUrl + "&page=[%page%]";
            }
            this.pagelink = link;
        }
        #endregion

        #region 方法

        int pageCount;
        public string RenderHtml()
        {
            if (totalCount <= pageSize)
                return string.Empty;

            //总页数   
            pageCount = (int)Math.Ceiling(this.totalCount / (double)this.pageSize);


            //起始页   
            int start = 1;

            //结束页   
            int end = toDisplayCount;
            if (pageCount < toDisplayCount) end = pageCount;

            //中间值   
            int centerNumber = toDisplayCount / 2;

            if (pageCount > toDisplayCount)
            {

                //显示的第一位   
                int topNumber = currentPage - centerNumber;

                if (topNumber > 1)
                {
                    start = topNumber;
                }

                if (topNumber > pageCount - toDisplayCount)
                {
                    start = pageCount - toDisplayCount + 1;
                }

                //显示的最后一位   
                int endNumber = currentPage + centerNumber;

                if (currentPage >= pageCount - centerNumber)
                {
                    end = pageCount;
                }
                else
                {
                    if (endNumber > toDisplayCount)
                    {
                        end = endNumber;
                    }
                }

            }

            StringBuilder sb = new StringBuilder();

            //Previous   
            if (this.currentPage > 1)
            {
                //sb.Append(GeneratePageLink("< Prev", this.currentPage - 1));
                sb.Append(GeneratePageLink("首页", 1));
                sb.Append(GeneratePageLink("上一页", this.currentPage - 1));
            }

            //if (start > 1)
            //{
            //    sb.Append(GeneratePageLink("1", 1));
            //    sb.Append("...");
            //}

            //end Previous   

            //for (int i = start; i <= end; i++)
            //{
            //    if (i == this.currentPage)
            //    {
            //        sb.AppendFormat("<span class=\"current\">{0}</span>", i);
            //    }
            //    else
            //    {
            //        sb.Append(GeneratePageLink(i.ToString(), i));
            //    }
            //}

            //Next   
            //if (end < pageCount)
            //{
            //    sb.Append("...");
            //    sb.Append(GeneratePageLink(pageCount.ToString(), pageCount));
            //}

            if (this.currentPage < pageCount)
            {
                //sb.Append(GeneratePageLink("Next >", this.currentPage + 1));
                sb.Append(GeneratePageLink("下一页", this.currentPage + 1));
                sb.Append(GeneratePageLink("末页", pageCount));
            }
            //end Next   

            //sb.Append("<span><input type=\"text\" class=\"pagerInput\" id=\"pagerInput\" maxlength=\"4\" onkeypress=\"return event.keyCode>=48&&event.keyCode<=57\"/></span>");
            //sb.Append("<span><input type=\"button\" value=\"跳转\" class=\"pagerButton\" onclick=\"if (!isNaN(parseInt(pagerInput.value))) window.location='" + this.pagelink.Replace("[%page%]", "") + "'+pagerInput.value;\"/></span>");

            return sb.ToString();
        }

        /// <summary>   
        /// 生成Page的链接   
        /// </summary>   
        /// <param name="linkText">文字</param>   
        /// <param name="PageNumber">页码</param>   
        /// <returns></returns>   
        private string GeneratePageLink(string linkText, int PageNumber)
        {
            string linkFormat = null;
            if (PageNumber - 1 < pageCount)
            {
                linkFormat = string.Format("<a href=\"{0}\">{1}</a>" + this.separated, this.pagelink.Replace("[%page%]", PageNumber.ToString()), linkText);
            }
            else
            {
                linkFormat = string.Format("<a href=\"{0}\">{1}</a>", this.pagelink.Replace("[%page%]", PageNumber.ToString()), linkText);
            }
            return linkFormat;

        }

        #endregion
    }
}

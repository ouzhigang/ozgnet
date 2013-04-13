using System;
using System.Text;

namespace OZGNet.WebForm.ThirdPart.Jumbot
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlPager
    {
        /// <summary>
        /// 分页导航
        /// </summary>
        /// <param name="mode">支持1=simple,2=normal,3=full</param>
        /// <param name="stype">html OR js</param>
        /// <param name="stepNum">步数</param>
        /// <param name="countNum">记录总数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static string GetPageBar(int mode, string stype, int stepNum, int countNum, int pageSize, int currentPage, string Http)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='p_btns'>");
            if (countNum > pageSize)
            {
                int pageCount = countNum % pageSize == 0 ? countNum / pageSize : countNum / pageSize + 1;
                currentPage = currentPage > pageCount ? pageCount : currentPage;
                currentPage = currentPage == 0 ? 1 : currentPage;
                int stepageSize = stepNum * 2 + 1;
                int pageRoot = 1;
                int pageFoot = pageCount;
                pageCount = pageCount == 0 ? 1 : pageCount;

                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' class='p_bar'><tr>");
                if (mode > 1)
                    sb.Append("<td style='white-space:nowrap;'>&nbsp;共有" + countNum.ToString() + "条记录，当前第" + currentPage.ToString() + "/" + pageCount.ToString() + "页&nbsp;&nbsp;&nbsp;</td>");
                sb.Append("<td width='100%'><table width='100%' border='0' align='left' cellpadding='0' cellspacing='0'><tr><td width='100%'>");
                if (pageCount - stepageSize < 0)//总数不足5
                    pageRoot = 1;
                else
                {
                    pageRoot = currentPage % stepageSize == 0 ? currentPage - stepageSize + 1 : currentPage - currentPage % stepageSize + 1;
                }
                if (pageCount - stepageSize < 0)//总数不足5
                    pageFoot = pageCount;
                else
                {
                    pageFoot = currentPage % stepageSize == 0 ? currentPage : (currentPage / stepageSize + 1) * stepageSize;
                }
                if (pageFoot > pageCount) pageFoot = pageCount;
                if (pageRoot != 1)//只要不是第一页
                {
                    if (mode > 2)
                        sb.Append("<a href='" + Http.Replace("<#page#>", "1") + "'>第一页</a>");
                    sb.Append("<a href='" + Http.Replace("<#page#>", Convert.ToString(currentPage - 1)) + "'>上一页</a>");
                }
                if (stepNum > 0)
                {
                    for (int i = pageRoot; i <= pageFoot; i++)
                    {
                        if (i == currentPage)
                        {
                            sb.Append("<a style='font-weight:bold;'>" + i.ToString() + "</a>");
                        }
                        else
                        {
                            sb.Append("<a href='" + Http.Replace("<#page#>", i.ToString()) + "'>" + i.ToString() + "</a>");
                        }
                        if (i == pageCount)
                            break;
                    }
                }

                if (pageFoot != pageCount)//只要不是最后一页
                {
                    sb.Append("<a href='" + Http.Replace("<#page#>", Convert.ToString(currentPage + 1)) + "'>下一页</a>");
                    if (mode > 2)
                        sb.Append("<a href='" + Http.Replace("<#page#>", pageCount.ToString()) + "'>最末页</a>");
                }
                if (mode > 2)
                {
                    if (stype == "html") sb.Append(" 转到第 <input type='text' name='custompage' size='2' onkeyup=\"this.value=this.value.replace(/\\D/g,'')\" onafterpaste=\"this.value=this.value.replace(/\\D/g,'')\" onkeydown=\"if(event.keyCode==13) {window.location='" + Http + "'.replace('<#page#>',this.value); return false;}\" /> 页");
                }
                sb.Append("</td></tr></table></td>");
                sb.Append("</tr></table>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}

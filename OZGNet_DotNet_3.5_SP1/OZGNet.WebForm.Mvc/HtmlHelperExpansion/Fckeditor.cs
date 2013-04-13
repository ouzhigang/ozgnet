using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace OZGNet.WebForm.Mvc.HtmlHelperExpansion
{
    /// <summary>
    /// 目录所在位置/fckeditor/
    /// 要引用/fckeditor/fckeditor.js
    /// </summary>
    public static class Fckeditor
    {
        /// <summary>
        /// Fckeditor的HTMLHelper,可以与同名 ViewData绑定
        /// </summary>
        /// <param name="html">HtmlHelper</param>
        /// <param name="name">Html的NAME</param>
        /// <returns></returns>
        public static string FckTextBox(this HtmlHelper html, string name)
        {
            return html.FckTextBox(name, null);
        }

        /// <summary>
        /// Fckeditor的HTMLHelper
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">Html name </param>
        /// <param name="value">内容</param>
        /// <returns></returns>
        public static string FckTextBox(this HtmlHelper html, string name, object value)
        {
            return html.FckTextBox(name, value.ToString());
        }
        /// <summary>
        /// Fckeditor的HTMLHelper
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name">Html name</param>
        /// <param name="value">内容</param>
        /// <returns></returns>
        public static string FckTextBox(this HtmlHelper html, string name, string value)
        {
            if (value == null)
            {
                value = Convert.ToString(html.ViewDataContainer.ViewData[name], CultureInfo.InvariantCulture);
            }

            return string.Format(@"<textarea name=""{0}"" id=""{0}"" rows=""50"" cols=""80"" style=""width:100%; height: 600px"">{1}</textarea>
　　<script type=""text/javascript"">
　　var oFCKeditor = new FCKeditor('{0}') ;
    if(sBasePath)
　　    oFCKeditor.BasePath    = sBasePath ;
    
　　oFCKeditor.Height=400;
　　oFCKeditor.ReplaceTextarea() ;
　　</script>
　　", name, value);

        }

    }
}

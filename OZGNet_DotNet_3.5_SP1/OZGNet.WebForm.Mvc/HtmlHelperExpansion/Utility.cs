using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace OZGNet.WebForm.Mvc.HtmlHelperExpansion
{
    public static class Utility
    {
        //1.RadioButton與Label的結合--->RadioLabel() 代碼如下：
        #region 擴展Radiobutton 帶有增加屬性
        /// <summary>
        /// 擴展Radiobutton
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="id">radio id</param>
        /// <param name="name">radio name</param>

        /// <param name="value">radio value</param>
        /// <param name="text">labe text</param>
        /// <param name="htmlAttribute">radio attr</param>
        /// <returns>string</returns>
        public static string RadioLabel(this HtmlHelper helper, string id, string name, string value, string text, object htmlAttribute)
        {
            var redio = "";
            var buliderRDO = new TagBuilder("input");
            buliderRDO.Attributes.Add("type", "radio");
            buliderRDO.GenerateId(id);
            buliderRDO.Attributes.Add("value", value);
            buliderRDO.Attributes.Add("name", name);
            buliderRDO.MergeAttributes(new RouteValueDictionary(htmlAttribute));
            redio += buliderRDO.ToString(TagRenderMode.SelfClosing);

            var buliderLabel = new TagBuilder("label");
            buliderLabel.Attributes.Add("for", id);
            buliderLabel.InnerHtml = text;
            redio += buliderLabel.ToString(TagRenderMode.Normal);

            return redio;
        }
        #endregion

        //2.CheckBox與Label 控件的結合 CheckBoxLabel()代碼 如下：
        #region 擴展checkbox
        /// <summary>
        /// 擴展checkbox
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="id">checkbox id</param>
        /// <param name="name">checkbox name</param>
        /// <param name="value">checkbox value</param>
        /// <param name="text">label text</param>
        /// <param name="htmlAttribute">checkbox attr</param>
        /// <returns></returns>
        public static string CheckBoxLabel(this HtmlHelper helper, string id, string name, string value, string text, object htmlAttribute)
        {
            string checkbox = "";
            var buliderCKB = new TagBuilder("input");
            buliderCKB.GenerateId(id);
            buliderCKB.Attributes.Add("type", "checkbox");
            buliderCKB.Attributes.Add("name", name);
            buliderCKB.MergeAttributes(new RouteValueDictionary(htmlAttribute));
            checkbox += buliderCKB.ToString(TagRenderMode.SelfClosing);

            var buliderLabel = new TagBuilder("label");
            buliderLabel.Attributes.Add("for", id);
            buliderLabel.InnerHtml = text;
            checkbox += buliderLabel.ToString(TagRenderMode.Normal);

            return checkbox;
        }
        #endregion

        //上面兩個方法多可以自行重載 重載象如下：
        #region 擴展checkbox 無法添加屬性
        /// <summary>
        ///  擴展checkbox 無法添加屬性
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">checkbox id</param>
        /// <param name="name">checkbox name</param>
        /// <param name="value">checkbox value</param>
        /// <param name="text">checkbox text</param>
        /// <returns></returns>
        public static string CheckBoxLabel(this HtmlHelper helper, string id, string name, string value, string text)
        {
            return CheckBoxLabel(helper, id, name, value, text, null);
        }
        #endregion



    }
}

using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace OZGNet.Net
{
    /// <summary>
    /// 欧志身份证查询类
    /// </summary>
    public class SFZ
    {
        public const string Version = "1.0";

        string _DistributionAddres;
        string _BirthDay;
        string _Gender;
        string _Number;

        public SFZ(string checknum)
        {
            string url = "http://www.youdao.com/smartresult-xml/search.s?type=id&q=" + checknum;
            string getContent = OZGNet.Net.Utility.FormGET(url, "GBK");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(getContent);

            _DistributionAddres = doc.SelectSingleNode("smartresult/product/location").InnerText;
            _BirthDay = doc.SelectSingleNode("smartresult/product/birthday").InnerText;
            _Gender = doc.SelectSingleNode("smartresult/product/gender").InnerText;
            _Number = doc.SelectSingleNode("smartresult/product/code").InnerText;

        }


        /// <summary>
        /// 发证地
        /// </summary>
        public string DistributionAddres
        {
            get { return _DistributionAddres; }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDay
        {
            get { return _BirthDay; }
        }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        {
            get
            {
                //m为男,f为女
                return _Gender == "m" ? "男" : "女";
            }
        }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string Number
        {
            get { return _Number; }
        }

    }

}
using System;
using System.Net;
using System.Text;
using System.Net.Json;
using System.Web;

namespace OZGNet.Net
{
    /// <summary>
    /// 输入IP，然后获取经纬度，地方名称
    /// </summary>
    public class GeoIP
    {
        private double _lat = 0;
        private double _lng = 0;
        private int _zoom = 9;
        private string _address = null;
        private string _ip;
        private string _content;
        private string _contentjson;

        /// <summary>
        /// 实例化GeoIP
        /// </summary>
        /// <param name="ip">输入IP</param>
        public GeoIP(string ip)
        {
            try
            {
                _ip = ip;
                _content = OZGNet.Net.Utility.FormGET("http://dituren-service.appspot.com/services/ip_lookup?c=onIpLookupLoaded&ip=" + ip, "UTF-8");

                //Json处理
                StringBuilder sb = new StringBuilder(_content);
                sb.Remove(0, sb.ToString().IndexOf("("));
                _contentjson = sb.ToString(1, sb.Length - 2);

                JsonTextParser parser = new JsonTextParser();
                JsonObjectCollection json = (JsonObjectCollection)parser.Parse(_contentjson);
                _ip = Convert.ToString(json["ip"].GetValue());
                _lat = Convert.ToDouble(json["lat"].GetValue());
                _lng = Convert.ToDouble(json["lon"].GetValue());
                _zoom = Convert.ToInt32(json["zoom"].GetValue());
                _address = Convert.ToString(json["address"].GetValue());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n输入IP:" + ip);
            }
            
        }

        /// <summary>
        /// 获取纬度
        /// </summary>
        public double Lat
        {
            get { return _lat; }
        }

        /// <summary>
        /// 获取经度
        /// </summary>
        public double Lng
        {
            get { return _lng; }
        }

        /// <summary>
        /// 获取放大级别
        /// </summary>
        public int Zoom
        {
            get { return _zoom; }
        }

        /// <summary>
        /// 获取目标地址
        /// </summary>
        public string Address
        {
            get { return _address; }
        }

        /// <summary>
        /// 获取目标IP
        /// </summary>
        public string IP
        {
            get { return _ip; }
        }

        /// <summary>
        /// 输出Json部分
        /// </summary>
        public string ContentJson
        {
            get { return _contentjson; }
        }

        /// <summary>
        /// 输出全部内容
        /// </summary>
        public string Content
        {
            get { return _content; }
        }

    }
}

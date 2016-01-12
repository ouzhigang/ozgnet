using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace OZGNet.Net
{
    /// <summary>
    /// 调用方法: Geo g = new Geo("地名的拼音");
    /// </summary>
    [Serializable]
    public class Geo
    {
        private string _latitude = "";

        private string _longtitude = "";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">地名的拼音</param>
        public Geo(string location)
        {
            string output = "csv";
            string url = string.Format("http://maps.google.com/maps/geo?q={0}&output={1}", location, output);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                string[] tmpArray = sr.ReadToEnd().Split(',');
                _latitude = tmpArray[2];
                _longtitude = tmpArray[3];
            }
        }

        /// <summary>
        /// get latitude(纬度)
        /// </summary>
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// get longtitude(经度)
        /// </summary>
        public string Longtitude
        {
            get { return _longtitude; }
            set { _longtitude = value; }
        }
    }
}
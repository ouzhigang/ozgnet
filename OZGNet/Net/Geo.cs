using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace OZGNet.Net
{
    /// <summary>
    /// ���÷���: Geo g = new Geo("������ƴ��");
    /// </summary>
    [Serializable]
    public class Geo
    {
        private string _latitude = "";

        private string _longtitude = "";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">������ƴ��</param>
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
        /// get latitude(γ��)
        /// </summary>
        public string Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// get longtitude(����)
        /// </summary>
        public string Longtitude
        {
            get { return _longtitude; }
            set { _longtitude = value; }
        }
    }
}
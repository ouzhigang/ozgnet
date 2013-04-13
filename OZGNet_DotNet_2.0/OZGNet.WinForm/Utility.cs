using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Drawing;

namespace OZGNet.WinForm
{
    /// <summary>
    /// WinForm专用工具类
    /// </summary>
    public class Utility : OZGNet.Utility
    {
        #region 返回一个url的xmldoc
        /// <summary>
        /// 返回一个url的xmldoc
        /// </summary>
        /// <param name="url">目标url</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static XmlDocument GetUrlXmlDoc(string url)
        {
            return Utility.GetUrlXmlDoc(url, "gb2312");
        }
        public static XmlDocument GetUrlXmlDoc(string url,string encoding)
        {
            return Utility.GetUrlXmlDoc(url, encoding);
        } 
        #endregion

        #region 获取CheckedListBox的选定状态
        /// <summary>
        /// 获取CheckedListBox的选定状态
        /// </summary>
        /// <param name="chkList"></param>
        /// <returns></returns>
        public static IList<bool> CheckedListBoxCheckState(CheckedListBox chkList)
        {
            IList<bool> tmp = new List<bool>();
            for (int i = 0; i < chkList.Items.Count; i++)
            {
                bool b = chkList.GetItemChecked(i);
                tmp.Add(b);
            }
            return tmp;
        } 
        #endregion

        #region 获取本机IP
        /// <summary>
        /// 获取第一个本机IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            return GetIP(0);
        }
        /// <summary>
        /// 获取指定本机IP
        /// </summary>
        /// <param name="AddressIndex"></param>
        /// <returns></returns>
        public static string GetIP(int AddressIndex)
        {
            return GetAddressList()[AddressIndex].ToString();
        }
        /// <summary>
        /// 获取指定本机AddressList
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetAddressList()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList;
        }
        #endregion        

        #region 获取当前执行程序的文件名
        /// <summary>
        /// 获取当前执行程序的文件名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFile()
        {
            string tmpValue = Application.ExecutablePath;
            tmpValue = tmpValue.Substring(tmpValue.LastIndexOf("\\") + 1);
            return tmpValue;
        } 
        #endregion

        #region 判断当前程序是否正在运行
        /// <summary>
        /// 判断当前程序是否正在运行
        /// </summary>
        /// <returns></returns>
        public static bool HasProcessName()
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (Process.GetCurrentProcess().ProcessName == p.ProcessName)
                {
                    return true;
                }
            }
            return false;
        } 
        #endregion

        

    }
}

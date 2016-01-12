using System;
using System.Data;
using System.Configuration;
using Microsoft.Win32;

namespace OZGNet.SysInfo
{
    /// <summary>
    /// 获取操作系统当前版本信息
    /// </summary>
    public static class OS
    {
        private static RegistryKey rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
        /// <summary>
        /// 
        /// </summary>
        public static string BuildLab
        {
            get
            {
                return GetValue("BuildLab");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CommonFilesDir
        {
            get
            {
                return GetValue("CommonFilesDir");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CSDBuildNumber
        {
            get
            {
                return GetValue("CSDBuildNumber");
            }
        }
        /// <summary>
        /// 获取Windows SP补丁信息
        /// </summary>
        public static string CSDVersion
        {
            get
            {
                return GetValue("CSDVersion");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentBuild
        {
            get
            {
                return GetValue("CurrentBuild");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentBuildNumber
        {
            get
            {
                return GetValue("CurrentBuildNumber");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentType
        {
            get
            {
                return GetValue("CurrentType");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentVersion
        {
            get
            {
                return GetValue("CurrentVersion");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DevicePath
        {
            get
            {
                return GetValue("DevicePath");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string MediaPath
        {
            get
            {
                return GetValue("MediaPath");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string OldWinDir
        {
            get
            {
                return GetValue("OldWinDir");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string OtherDevicePath
        {
            get
            {
                return GetValue("OtherDevicePath");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string PathName
        {
            get
            {
                return GetValue("PathName");
            }
        }
        /// <summary>
        /// 获取Windows ID
        /// </summary>
        public static string ProductId
        {
            get
            {
                return GetValue("ProductId");
            }
        }
        /// <summary>
        /// 获取Windows版本信息
        /// </summary>
        public static string ProductName
        {
            get
            {
                return GetValue("ProductName");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string ProgramFilesDir
        {
            get
            {
                return GetValue("ProgramFilesDir");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string RegDone
        {
            get
            {
                return GetValue("RegDone");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string RegisteredOrganization
        {
            get
            {
                return GetValue("RegisteredOrganization");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string RegisteredOwner
        {
            get
            {
                return GetValue("RegisteredOwner");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string SoftwareType
        {
            get
            {
                return GetValue("SoftwareType");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string SourcePath
        {
            get
            {
                return GetValue("SourcePath");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string SystemRoot
        {
            get
            {
                return GetValue("SystemRoot");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string WallPaperDir
        {
            get
            {
                return GetValue("WallPaperDir");
            }
        }
        /// <summary>
        /// 获取本机ODBC驱动列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetOdbcList()
        {
            string _root = "SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers";
            RegistryKey _rk = Registry.LocalMachine.OpenSubKey(_root);
            try
            {
                string[] tmp_strs = _rk.GetValueNames();
                string[] list = new string[tmp_strs.Length];

                return tmp_strs;
            }
            catch
            {
                string[] list = new string[1];
                list[0] = "未知";
                return list;
            }
        }
        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetValue(string name)
        {
            try
            {
                return rk.GetValue(name).ToString();
            }
            catch (Exception ex)
            {
                return "未知 [" + ex.Message + "]";
            }
        }



    }
}
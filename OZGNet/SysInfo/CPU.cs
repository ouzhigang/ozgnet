using System;
using System.Data;
using System.Configuration;
using Microsoft.Win32;

namespace OZGNet.SysInfo
{
    /// <summary>
    /// 获取CPU基本信息
    /// </summary>
    public class CPU
    {
        private static RegistryKey _rk = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor");

        /// <summary>
        /// 
        /// </summary>
        public static string[] Identifiers
        {
            get
            {
                string[] strs = new string[ProcessorCount];
                for (int i = 0; i < strs.Length; i++)
                {
                    strs[i] = GetValue("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + _rk.GetSubKeyNames()[i], "Identifier");
                }
                return strs;
            }
        }

        /// <summary>
        /// 获取CPU名称
        /// </summary>
        public static string[] ProcessorNames
        {
            get
            {
                string[] strs = new string[ProcessorCount];
                for (int i = 0; i < strs.Length; i++)
                {
                    strs[i] = GetValue("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + _rk.GetSubKeyNames()[i], "ProcessorNameString");
                }
                return strs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string[] VendorIdentifiers
        {
            get
            {
                string[] strs = new string[ProcessorCount];
                for (int i = 0; i < strs.Length; i++)
                {
                    strs[i] = GetValue("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + _rk.GetSubKeyNames()[i], "VendorIdentifier");
                }
                return strs;
            }
        }

        /// <summary>
        /// 获取CPU个数
        /// </summary>
        public static int ProcessorCount
        {
            get
            {
                return _rk.GetSubKeyNames().Length;
            }
        }




        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetValue(string path, string name)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                return rk.GetValue(name).ToString();
            }
            catch (Exception ex)
            {
                return "未知 [" + ex.Message + "]";
            }
        }


    }
}

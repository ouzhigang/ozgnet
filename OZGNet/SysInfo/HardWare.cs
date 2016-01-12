using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OZGNet.SysInfo
{
    /// <summary>
    /// 获取硬件信息
    /// </summary>
    public class HardWare
    {
        /// <summary>
        /// CPU实例
        /// </summary>
        /// <param name="cpuinfo"></param>
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void GetSystemInfo(ref CPU_INFO cpuinfo);

        /// <summary>
        /// 内存实例
        /// </summary>
        /// <param name="meminfo"></param>
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        /// <summary>
        /// 获取系统CPU信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CPU_INFO
        {
            /// <summary>
            /// 
            /// </summary>
            public uint dwOemId;
            /// <summary>
            /// 
            /// </summary>
            public uint dwPageSize;
            /// <summary>
            /// 
            /// </summary>
            public uint lpMinimumApplicationAddress;
            /// <summary>
            /// 
            /// </summary>
            public uint lpMaximumApplicationAddress;
            /// <summary>
            /// 
            /// </summary>
            public uint dwActiveProcessorMask;
            /// <summary>
            /// 
            /// </summary>
            public uint dwNumberOfProcessors;
            /// <summary>
            /// 
            /// </summary>
            public uint dwProcessorType;
            /// <summary>
            /// 
            /// </summary>
            public uint dwAllocationGranularity;
            /// <summary>
            /// 
            /// </summary>
            public uint dwProcessorLevel;
            /// <summary>
            /// 
            /// </summary>
            public uint dwProcessorRevision;
        }

        /// <summary>
        /// 获取系统内存
        /// HardWare.MEMORY_INFO info = new HardWare.MEMORY_INFO();
        /// HardWare.GlobalMemoryStatus(ref info);
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            /// <summary>
            /// 
            /// </summary>
            public uint dwLength;
            /// <summary>
            /// 正在使用的内存,单位为百分比
            /// </summary>
            public uint dwMemoryLoad;
            /// <summary>
            /// 物理内存总数,单位为M   (dwTotalPhys / 0x100000)
            /// </summary>
            public uint dwTotalPhys;
            /// <summary>
            /// 可用物理内存,单位为M   (dwAvailPhys / 0x100000)
            /// </summary>
            public uint dwAvailPhys;
            /// <summary>
            /// 交换文件大小,单位为M   (dwTotalPageFile / 0x100000)
            /// </summary>
            public uint dwTotalPageFile;
            /// <summary>
            /// 交换文件可用大小,单位为M   (dwAvailPageFile / 0x100000)
            /// </summary>
            public uint dwAvailPageFile;
            /// <summary>
            /// 总虚拟内存,单位为M      (dwTotalVirtual / 0x100000)
            /// </summary>
            public uint dwTotalVirtual;
            /// <summary>
            /// 剩余虚拟内存,单位为M     (dwAvailVirtual / 0x100000)
            /// </summary>
            public uint dwAvailVirtual;
        }
    }



}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace OZGNet.WinForm
{
    /// <summary>
    /// WinForm IO帮助类
    /// </summary>
    public class UtilityIO : OZGNet.UtilityIO
    {
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir_path">目标文件夹(绝对路径)</param>
        public static void DirDelete(string dir_path)
        {
            OZGNet.UtilityIO.DirDelete(dir_path, true);
        }

        /// <summary>
        /// 转换FLV(大小为480*320)
        /// </summary>
        /// <param name="ffmpeg">ffmpeg的路径</param>
        /// <param name="input">输入视频路径</param>
        /// <param name="output">输出FLV路径</param>
        public static void ChangeFLV(string ffmpeg, string input, string output)
        {
            OZGNet.UtilityIO.ChangeFLV(ffmpeg, input, output, "480", "320");
        }

        /// <summary>
        /// 大文件复制
        /// </summary>
        /// <param name="fromFile">要复制的文件</param>
        /// <param name="toFile">要保存的位置</param>
        public static void CopyFile(string fromFile, string toFile)
        {
            //30M复制
            int fileLength = 30 * 1024 * 1024;
            OZGNet.UtilityIO.CopyFile(fromFile, toFile, fileLength);
        }



    }
}

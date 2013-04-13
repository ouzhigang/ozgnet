using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace OZGNet.WebForm
{
    /// <summary>
    /// 向客户端输出Js内容
    /// </summary>
    public class UtilityJs
    {
        #region 弹出信息并重定向
        /// <summary>
        ///  弹出信息并重定向
        /// </summary>
        /// <param name="msg_str">信息(可以为空)</param>
        public static void ShowMessageBox(string msg_str)
        {
            ShowMessageBox(msg_str, null);
        }
        /// <summary>
        /// 弹出信息并重定向
        /// </summary>
        /// <param name="msg_str">信息(可以为空)</param>
        /// <param name="url">重定向路径</param>
        public static void ShowMessageBox(string msg_str, string url)
        {
            ShowMessageBox(msg_str, url, Options.RedirectTarget.Current);
        }
        /// <summary>
        /// 弹出信息并重定向
        /// </summary>
        /// <param name="msg_str">信息(可以为空)</param>
        /// <param name="url">重定向路径</param>
        /// <param name="target">目标框架</param>
        public static void ShowMessageBox(string msg_str, string url, Options.RedirectTarget target)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            if (!String.IsNullOrEmpty(msg_str))
            {
                sb.Append("alert(\"" + msg_str + "\");");
            }

            if (String.IsNullOrEmpty(url))
            {
                sb.Append("history.go(-1);");
            }
            else
            {
                if ((int)target == 1)
                {
                    sb.Append("window.location.href='" + url + "';");
                }
                else if ((int)target == 2)
                {
                    sb.Append("window.parent.location.replace(\"" + url + "\");");
                }                
            }

            sb.Append("</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 重定向
        /// <summary>
        /// 当前页面重定向
        /// </summary>
        /// <param name="url">重定向连接,null为后退</param>
        public static void Redirect(string url)
        {
            Redirect(url, Options.RedirectTarget.Current);
        }
        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="url">重定向连接,null为后退,且后面的参数无效</param>
        /// <param name="target">目标框架</param>
        public static void Redirect(string url, Options.RedirectTarget target)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'>");
            if (String.IsNullOrEmpty(url))
            {
                sb.Append("history.back();");
            }
            else
            {
                if ((int)target == 1)
                {
                    sb.Append("window.location.replace(\"" + url + "\");");
                }
                else if ((int)target == 2)
                {
                    sb.Append("window.parent.location.replace(\"" + url + "\");");
                }
            }
            sb.Append("</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        } 
        #endregion

        #region 显示一个FusionCharts
        /// <summary>
        /// 显示一个FusionCharts
        /// </summary>
        /// <param name="divID">显示FusionCharts的Div</param>
        /// <param name="dataFile">数据文件</param>
        /// <param name="FusionChartsFile">加载的FusionCharts文件</param>
        /// <param name="width">显示FusionCharts的宽</param>
        /// <param name="height">显示FusionCharts的高</param>
        public static void ShowFusionCharts(string divID, string dataFile, Options.FusionCharts FusionChartsFile, int width, int height)
        {
            ShowFusionCharts(divID, dataFile, FusionChartsFile, width, height, false);
        }
        /// <summary>
        /// 显示一个FusionCharts
        /// </summary>
        /// <param name="divID">显示FusionCharts的Div</param>
        /// <param name="dataFile">数据文件</param>
        /// <param name="FusionChartsFile">加载的FusionCharts文件</param>
        /// <param name="width">显示FusionCharts的宽</param>
        /// <param name="height">显示FusionCharts的高</param>
        /// <param name="useJS">是否使用FusionCharts.js</param>
        public static void ShowFusionCharts(string divID, string dataFile, Options.FusionCharts FusionChartsFile, int width, int height, bool useJS)
        {
            string FusionChartsDir = "~/images/FusionCharts/";
            string FusionChartsFilePath = FusionChartsDir + GetFusionChartsFile(FusionChartsFile);
            string FusionChartsJS = "~/JS/FusionCharts.js";

            if (!File.Exists(HttpContext.Current.Server.MapPath(FusionChartsFilePath)))
            {
                throw new Exception("系统找不到文件 " + FusionChartsFilePath);
            }

            if (!File.Exists(HttpContext.Current.Server.MapPath(FusionChartsJS)))
            {
                throw new Exception("系统找不到文件 " + FusionChartsJS);
            }

            if (useJS)
            {
                FusionChartsJS = FusionChartsJS.Replace("~/", "/");
                HttpContext.Current.Response.Write("<script type='text/javascript' src='" + FusionChartsJS + "'></script>\r\n");
            }

            FusionChartsFilePath = FusionChartsFilePath.Replace("~/", "/");

            HttpContext.Current.Response.Write("<script type='text/javascript'>\r\n");
            HttpContext.Current.Response.Write("var chart = new FusionCharts('" + FusionChartsFilePath + "', '" + divID + "', '" + width.ToString() + "', '" + height.ToString() + "', '0', '1');\r\n");
            HttpContext.Current.Response.Write("chart.setDataURL('" + dataFile + "');\r\n");
            HttpContext.Current.Response.Write("chart.render('" + divID + "');\r\n");
            HttpContext.Current.Response.Write("</script>\r\n");
        }         
        #region 获取FusionCharts文件名
        /// <summary>
        /// 获取FusionCharts文件名
        /// </summary>
        /// <param name="FusionChartsFile">选择FusionCharts</param>
        /// <returns></returns>
        protected static string GetFusionChartsFile(Options.FusionCharts FusionChartsFile)
        {
            if ((int)FusionChartsFile == 1)
            {
                return "Area2D.swf";
            }
            else if ((int)FusionChartsFile == 2)
            {
                return "Bar2D.swf";
            }
            else if ((int)FusionChartsFile == 3)
            {
                return "Bubble.swf";
            }
            else if ((int)FusionChartsFile == 4)
            {
                return "Column2D.swf";
            }
            else if ((int)FusionChartsFile == 5)
            {
                return "Column3D.swf";
            }
            else if ((int)FusionChartsFile == 6)
            {
                return "Doughnut2D.swf";
            }
            else if ((int)FusionChartsFile == 7)
            {
                return "Doughnut3D.swf";
            }
            else if ((int)FusionChartsFile == 8)
            {
                return "Line.swf";
            }
            else if ((int)FusionChartsFile == 9)
            {
                return "MSArea.swf";
            }
            else if ((int)FusionChartsFile == 10)
            {
                return "MSBar2D.swf";
            }
            else if ((int)FusionChartsFile == 11)
            {
                return "MSBar3D.swf";
            }
            else if ((int)FusionChartsFile == 12)
            {
                return "MSColumn2D.swf";
            }
            else if ((int)FusionChartsFile == 13)
            {
                return "MSColumn3D.swf";
            }
            else if ((int)FusionChartsFile == 14)
            {
                return "MSColumn3DLineDY.swf";
            }
            else if ((int)FusionChartsFile == 15)
            {
                return "MSColumnLine3D.swf";
            }
            else if ((int)FusionChartsFile == 16)
            {
                return "MSCombi2D.swf";
            }
            else if ((int)FusionChartsFile == 17)
            {
                return "MSCombi3D.swf";
            }
            else if ((int)FusionChartsFile == 18)
            {
                return "MSCombiDY2D.swf";
            }
            else if ((int)FusionChartsFile == 19)
            {
                return "MSLine.swf";
            }
            else if ((int)FusionChartsFile == 20)
            {
                return "MSStackedColumn2D.swf";
            }
            else if ((int)FusionChartsFile == 21)
            {
                return "MSStackedColumn2DLineDY.swf";
            }
            else if ((int)FusionChartsFile == 22)
            {
                return "Pie2D.swf";
            }
            else if ((int)FusionChartsFile == 23)
            {
                return "Pie3D.swf";
            }
            else if ((int)FusionChartsFile == 24)
            {
                return "Scatter.swf";
            }
            else if ((int)FusionChartsFile == 25)
            {
                return "ScrollArea2D.swf";
            }
            else if ((int)FusionChartsFile == 26)
            {
                return "ScrollColumn2D.swf";
            }
            else if ((int)FusionChartsFile == 27)
            {
                return "ScrollCombi2D.swf";
            }
            else if ((int)FusionChartsFile == 28)
            {
                return "ScrollCombiDY2D.swf";
            }
            else if ((int)FusionChartsFile == 29)
            {
                return "ScrollLine2D.swf";
            }
            else if ((int)FusionChartsFile == 30)
            {
                return "ScrollStackedColumn2D.swf";
            }
            else if ((int)FusionChartsFile == 31)
            {
                return "SSGrid.swf";
            }
            else if ((int)FusionChartsFile == 32)
            {
                return "StackedArea2D.swf";
            }
            else if ((int)FusionChartsFile == 33)
            {
                return "StackedBar2D.swf";
            }
            else if ((int)FusionChartsFile == 34)
            {
                return "StackedBar3D.swf";
            }
            else if ((int)FusionChartsFile == 35)
            {
                return "StackedColumn2D.swf";
            }
            else if ((int)FusionChartsFile == 36)
            {
                return "StackedColumn3D.swf";
            }
            else if ((int)FusionChartsFile == 37)
            {
                return "StackedColumn3DLineDY.swf";
            }
            return null;
        } 
        #endregion
        #endregion

        #region FLV播放器1，（FLV路径需要绝对路径,可以同时播放N个FLV）
        /// <summary>
        /// FLV播放器1，（FLV路径需要绝对路径，路径为~/images/flvplayer/vcastr.swf）
        /// </summary>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer1(string flvFile, int width, int height)
        {
            Dictionary<string, string> Attributes = new Dictionary<string, string>();
            Attributes.Add("IsAutoPlay", "1");
            Attributes.Add("IsShowBar", "1");
            ShowFLVPlayer1(flvFile, Attributes, width, height);
        }
        /// <summary>
        /// FLV播放器1，（FLV路径需要绝对路径，路径为~/images/flvplayer/vcastr.swf）
        /// </summary>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="Attributes">其他属性（属性需要参照相关文档）</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer1(string flvFile, Dictionary<string, string> Attributes, int width, int height)
        {
            List<string> flvFiles = new List<string>();
            flvFiles.Add(flvFile);
            ShowFLVPlayer1(flvFiles, Attributes, width, height);
        }
        /// <summary>
        /// FLV播放器1，（FLV路径需要绝对路径，路径为~/images/flvplayer/vcastr.swf）
        /// </summary>
        /// <param name="flvFiles">FLV文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer1(IList<string> flvFiles, int width, int height)
        {
            Dictionary<string, string> Attributes = new Dictionary<string, string>();
            Attributes.Add("IsAutoPlay", "1");
            Attributes.Add("IsShowBar", "1");
            ShowFLVPlayer1(flvFiles, Attributes, width, height);
        }
        /// <summary>
        /// FLV播放器1，（FLV路径需要绝对路径，路径为~/images/flvplayer/vcastr.swf）
        /// </summary>
        /// <param name="flvFiles">FLV文件路径</param>
        /// <param name="Attributes">其他属性（属性需要参照相关文档）</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer1(IList<string> flvFiles, Dictionary<string, string> Attributes, int width, int height)
        {
            string file1 = "~/images/flvplayer/vcastr.swf";
            if (!File.Exists(HttpContext.Current.Server.MapPath(file1)))
            {
                throw new Exception("系统找不到文件 " + file1);
            }
            file1 = file1.Replace("~/", "/");

            string files = null;
            foreach (string item in flvFiles)
            {
                files += item + "|";
            }
            files = files.Substring(0, files.Length - 1);

            string attr = null;
            if (Attributes != null)
            {
                foreach (KeyValuePair<string, string> item in Attributes)
                {
                    attr += item.Key + "=" + item.Value + "&";
                }
                attr = attr.Substring(0, attr.Length - 1);

                attr = "vcastr_file=" + files + "&" + attr;
            }
            else
            {
                attr = "vcastr_file=" + files;
            }


            HttpContext.Current.Response.Write("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0' width='" + width.ToString() + "' height='" + height.ToString() + "'>\r\n");
            HttpContext.Current.Response.Write("<param name='movie' value='" + file1 + "'>\r\n");
            HttpContext.Current.Response.Write("<param name='quality' value='high'>\r\n");
            HttpContext.Current.Response.Write("<param name='allowFullScreen' value='true' />\r\n");
            HttpContext.Current.Response.Write("<param name='FlashVars' value='" + attr + "' />\r\n");
            HttpContext.Current.Response.Write("<embed src='" + file1 + "' allowFullScreen='true' FlashVars='" + attr + "' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='" + width.ToString() + "' height='" + height.ToString() + "'></embed>\r\n");
            HttpContext.Current.Response.Write("</object>\r\n");
        } 
        #endregion

        #region FLV播放器2，（FLV路径需要绝对路径）
        /// <summary>
        /// FLV播放器2，（FLV路径需要绝对路径，路径为~/images/flvplayer/vcastrplayer.swf）
        /// </summary>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer2(string flvFile, int width, int height)
        {
            ShowFLVPlayer2("VideoPlayback", flvFile, width, height);
        }
        /// <summary>
        /// FLV播放器2，（FLV路径需要绝对路径，路径为~/images/flvplayer/vcastrplayer.swf）
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer2(string objID, string flvFile, int width, int height)
        {
            string file1 = "~/images/flvplayer/vcastrplayer.swf";
            if (!File.Exists(HttpContext.Current.Server.MapPath(file1)))
            {
                throw new Exception("系统找不到文件 " + file1);
            }

            file1 = file1.Replace("~/", "/");

            HttpContext.Current.Response.Write("<object type='application/x-shockwave-flash' width='" + width.ToString() + "' height='" + height.ToString() + "' id='" + objID + "'>\r\n");
            //HttpContext.Current.Response.Write("\r\n");
            HttpContext.Current.Response.Write("<param name='allowScriptAccess' value='sameDomain'>\r\n");
            HttpContext.Current.Response.Write("<param name='movie' value='" + file1 + "?vcastr_flie=" + flvFile + "'>\r\n");
            HttpContext.Current.Response.Write("<param name='quality' value='best'>\r\n");
            HttpContext.Current.Response.Write("<param name='bgcolor' value='#ffffff'>\r\n");
            HttpContext.Current.Response.Write("<param name='scale' value='noScale'>\r\n");
            HttpContext.Current.Response.Write("</object>\r\n");
        } 
        #endregion

        #region FLV播放器3，（FLV路径需要绝对路径,这个是Google的播放器）
        /// <summary>
        /// FLV播放器3，自动播放（FLV路径需要绝对路径,这个是Google的播放器，路径为~/images/flvplayer/googleplayer.swf）
        /// </summary>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer3(string flvFile, int width, int height)
        {
            ShowFLVPlayer3("VideoPlayback", flvFile, width, height);
        }
        /// <summary>
        /// FLV播放器3，自动播放（FLV路径需要绝对路径,这个是Google的播放器，路径为~/images/flvplayer/googleplayer.swf）
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer3(string objID, string flvFile, int width, int height)
        {
            ShowFLVPlayer3(objID, flvFile, true, width, height);
        }
        /// <summary>
        /// FLV播放器3，（FLV路径需要绝对路径,这个是Google的播放器，路径为~/images/flvplayer/googleplayer.swf）
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="flvFile">FLV文件路径</param>
        /// <param name="autoPlay">自动播放</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowFLVPlayer3(string objID, string flvFile, bool autoPlay, int width, int height)
        {
            string file1 = "~/images/flvplayer/googleplayer.swf";
            if (!File.Exists(HttpContext.Current.Server.MapPath(file1)))
            {
                throw new Exception("系统找不到文件 " + file1);
            }

            file1 = file1.Replace("~/", "/");

            HttpContext.Current.Response.Write("<object id='" + objID + "' width='" + width.ToString() + "' height='" + height.ToString() + "' align='middle' type='application/x-shockwave-flash' data='" + file1 + "?videoUrl=" + flvFile + "&playerMode=normal&autoPlay=" + autoPlay.ToString().ToLower() + "'>\r\n");
            HttpContext.Current.Response.Write("<param name='allowScriptAccess' value='sameDomain' />\r\n");
            HttpContext.Current.Response.Write("<param name='movie' value='" + file1 + "?videoUrl=" + flvFile + "&playerMode=normal&autoPlay=" + autoPlay.ToString().ToLower() + "'/>\r\n");
            HttpContext.Current.Response.Write("<param name='quality' value='best' />\r\n");
            HttpContext.Current.Response.Write("<param name='bgcolor' value='#ffffff' />\r\n");
            HttpContext.Current.Response.Write("<param name='scale' value='noScale' />\r\n");
            HttpContext.Current.Response.Write("<param name='wmode' value='window' />\r\n");
            HttpContext.Current.Response.Write("<param name='salign' value='TL' />\r\n");
            HttpContext.Current.Response.Write("</object>\r\n");
        } 
        #endregion

        #region MediaPalyer播放器，（加载asx文件，可播放多个文件）
        /// <summary>
        /// MediaPalyer播放器，（加载asx文件，可播放多个文件）
        /// </summary>
        /// <param name="url">ASX文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowMediaPalyers(string url, int width, int height)
        {
            ShowMediaPalyers("MediaPalyer", url, width, height);
        }
        /// <summary>
        /// MediaPalyer播放器，（加载asx文件，可播放多个文件）
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="url">ASX文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowMediaPalyers(string objID, string url, int width, int height)
        {
            HttpContext.Current.Response.Write("<object id=\"" + objID + "\" width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" classid=\"CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6\" type=application/x-oleobject standby=\"Loading Windows Media Player components...\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"URL\" value=\"" + url + "\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"rate\" value=\"1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"balance\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"currentPosition\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"defaultFrame\" value=\"\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"playCount\" value=\"100\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"autoStart\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"currentMarker\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"invokeURLs\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"baseURL\" value=\"\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"volume\" value=\"100\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"mute\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"uiMode\" value=\"full\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"stretchToFit\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"windowlessVideo\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"enabled\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"enableContextMenu\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"fullScreen\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SAMIStyle\" value=\"\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SAMILang\" value=\"\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SAMIFilename\" value=\"\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"captioningID\" value=\"\">\r\n");
            HttpContext.Current.Response.Write("</object>\r\n");
        } 
        #endregion
        #region MediaPalyer播放器，（单播）
        /// <summary>
        /// MediaPalyer播放器，（单播）
        /// </summary>
        /// <param name="playFile">播放文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowMediaPalyer(string playFile, int width, int height)
        {
            ShowMediaPalyer("MediaPalyer", playFile, width, height);
        }
        /// <summary>
        /// MediaPalyer播放器，（单播）
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="playFile">播放文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowMediaPalyer(string objID, string playFile, int width, int height)
        {
            HttpContext.Current.Response.Write("<OBJECT id=\"" + objID + "\" name=\"" + objID + "\" classid=\"CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95\" border=\"0\" width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" type=\"application/x-oleobject\" standby=\"Loading Windows Media Player components...\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Filename\" value='" + playFile + "'>\r\n");
            HttpContext.Current.Response.Write("<param name=\"AudioStream\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"AutoSize\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"AutoStart\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"AnimationAtStart\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"AllowScan\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"AllowChangeDisplaySize\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"AutoRewind\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Balance\" value=\"10\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"BaseURL\" value>\r\n");
            HttpContext.Current.Response.Write("<param name=\"BufferingTime\" value=\"5\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"CaptioningID\" value>\r\n");
            HttpContext.Current.Response.Write("<param name=\"ClickToPlay\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"CursorType\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"CurrentPosition\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"CurrentMarker\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"DefaultFrame\" value>\r\n");
            HttpContext.Current.Response.Write("<param name=\"DisplayBackColor\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"DisplayForeColor\" value=\"16777215\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"DisplayMode\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"DisplaySize\" value=\"4\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Enabled\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"EnableContextMenu\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"EnablePositionControls\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"EnableFullScreenControls\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"EnableTracker\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"InvokeURLs\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Language\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Mute\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"PlayCount\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"PreviewMode\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Rate\" value=\"1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SAMILang\" value>\r\n");
            HttpContext.Current.Response.Write("<param name=\"SAMIStyle\" value>\r\n");
            HttpContext.Current.Response.Write("<param name=\"SAMIFileName\" value>\r\n");
            HttpContext.Current.Response.Write("<param name=\"SelectionStart\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SelectionEnd\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendOpenStateChangeEvents\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendWarningEvents\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendErrorEvents\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendKeyboardEvents\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendMouseClickEvents\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendMouseMoveEvents\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"SendPlayStateChangeEvents\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowCaptioning\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowControls\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowAudioControls\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowDisplay\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowGotoBar\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowPositionControls\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowStatusBar\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"ShowTracker\" value=\"-1\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"TransparentAtStart\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"VideoBorderWidth\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"VideoBorderColor\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"VideoBorder3D\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"Volume\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<param name=\"WindowlessVideo\" value=\"0\">\r\n");
            HttpContext.Current.Response.Write("<embed type=\"application/x-mplayer2\" pluginspage=\"http://www.microsoft.com/windows/mediaplayer/download/default.asp\" Name=\"" + objID + "_embed\" width=\"360\" height=\"70\" border=\"0\" SHOWSTATUSBAR=\"-1\" SHOWCONTROLS=\"0\" SHOWGOTOBAR=\"0\" SHOWDISPLAY=\"-1\" INVOKEURLS=\"-1\" AUTOSTART=\"1\" CLICKTOPLAY=\"0\" DisplayForeColor=\"12945678\">\r\n");
            HttpContext.Current.Response.Write("</OBJECT>\r\n");
        } 
        #endregion

        #region Real播放器，（不能播放多个文件，但可以通过JavaScript实现播放多个文件）
        /// <summary>
        /// Real播放器，播放一个文件
        /// </summary>
        /// <param name="rmFile">RM文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowRealPlayer(string rmFile, int width, int height)
        {
            ShowRealPlayer("rmPlayer", rmFile, width, height);
        }
        /// <summary>
        /// Real播放器，播放一个文件
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="rmFile">RM文件路径</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowRealPlayer(string objID, string rmFile, int width, int height)
        {
            IList<string> rmFiles = new List<string>();
            rmFiles.Add(rmFile);
            ShowRealPlayer(objID, rmFiles, width, height);
        }
        /// <summary>
        /// Real播放器，播放多个文件
        /// </summary>
        /// <param name="rmFiles">RM文件路径列表</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowRealPlayer(IList<string> rmFiles, int width, int height)
        {
            ShowRealPlayer("rmPlayer", rmFiles, width, height);
        }
        /// <summary>
        /// Real播放器，播放多个文件
        /// </summary>
        /// <param name="objID">对象ID</param>
        /// <param name="rmFiles">RM文件路径列表</param>
        /// <param name="width">播放器宽度</param>
        /// <param name="height">播放器高度</param>
        public static void ShowRealPlayer(string objID, IList<string> rmFiles, int width, int height)
        {
            string src = null;
            if (rmFiles != null)
            {
                foreach (string rmFile in rmFiles)
                {
                    src += rmFile + ",";
                }
                src = src.Substring(0, src.Length - 1);
            }

            HttpContext.Current.Response.Write("<OBJECT id=\"" + objID + "\" width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\">\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"src\" VALUE='" + src + "'>\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"autostart\" VALUE=\"1\">\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"controls\" VALUE=\"imagewindow\">\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"console\" VALUE=\"kingege\">\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"loop\" VALUE=\"-1\">\r\n");
            HttpContext.Current.Response.Write("</OBJECT>\r\n");
            HttpContext.Current.Response.Write("<BR>\r\n");
            HttpContext.Current.Response.Write("<OBJECT width=\"" + width.ToString() + "\" height=\"30\" classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\">\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"controls\" VALUE=\"controlpanel\">\r\n");
            HttpContext.Current.Response.Write("<PARAM NAME=\"console\" VALUE=\"kingege\">\r\n");
            HttpContext.Current.Response.Write("</OBJECT>\r\n");
            HttpContext.Current.Response.Write("<script type='text/javascript'>\r\n");
            HttpContext.Current.Response.Write("document.getElementById(\"" + objID + "\").SetEnableContextMenu(false);\r\n");
            HttpContext.Current.Response.Write("document.getElementById(\"" + objID + "\").SetWantErrors(true);\r\n");
            HttpContext.Current.Response.Write("</script>\r\n");
            
            
        } 
        #endregion

        




    }
}

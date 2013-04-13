using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace OZGNet.WebFormControls.ThirdPart.Winson.AlertAndPopup
{
    /// <summary>
    /// 自动弹出窗口控件
    /// </summary>
    [DefaultProperty("Message"), ToolboxData("<{0}:PopupWin runat=server></{0}:PopupWin>")]
    [Designer(typeof(PopupWinDesigner))]
    public class PopupWin : System.Web.UI.WebControls.WebControl, IPostBackEventHandler
    {
        #region 私有变量、常数和构造器

        private string msg, fullmsg, divDesign, cntStyle, sPopup, spopStyle,
          cntStyleN, cntStyleI, aStyle, aCommands, hdrStyle, title, closeHtml;

        private string popupBackground, popupBorderDark, popupBorderLight,
          cntBorderDark, cntBorderLight, cntBackground, gradientStart,
          gradientEnd, textColor, xButton, xButtonOver, sLink, sTarget;

        private PopupColorStyle clrStyle;
        private PopupDocking popDock;
        private PopupAction popAction;

        int xOffset, yOffset, iSpeed = 20;
        Size winSize;
        bool winScroll = true, bAutoShow = true, bDragDrop = false, bShowLink = true;

        int iHide, startTime;

        const string sScript = @"<script type=""text/javascript"">
      //<![CDATA[
        var [id]oldonloadHndlr=window.onload, [id]popupHgt, [id]actualHgt, [id]tmrId=-1, [id]resetTimer;
        var [id]titHgt, [id]cntDelta, [id]tmrHide=-1, [id]hideAfter=[hide], [id]hideAlpha, [id]hasFilters=[ie];
        var [id]nWin, [id]showBy=null, [id]dxTimer=-1, [id]popupBottom, [id]oldLeft;
        var [id]nText,[id]nMsg,[id]nTitle,[id]bChangeTexts=false;
        window.onload=[id]espopup_winLoad;

        var [id]oldonscrollHndr=window.onscroll;
        window.onscroll=[id]espopup_winScroll;
        [id]nText=""[popup]"";

        function [id]espopup_winScroll()
        {
          if ([id]oldonscrollHndr!=null) [id]oldonscrollHndr();
          if ([id]tmrHide!=-1)
          {
            el=document.getElementById('[id]');
            el.style.display='none'; el.style.display='block';
          }
        }

        function [id]espopup_ShowPopup(show)
        {
          if ([id]dxTimer!=-1) { el.filters.blendTrans.stop(); }

          if (([id]tmrHide!=-1) && ((show!=null) && (show==[id]showBy)))
          {
            clearInterval([id]tmrHide);
            [id]tmrHide=setInterval([id]espopup_tmrHideTimer,[id]hideAfter);
            return;
          }
          if ([id]tmrId!=-1) return;
          [id]showBy=show;

          elCnt=document.getElementById('[id]_content')
          elTit=document.getElementById('[id]_header');
          el=document.getElementById('[id]');
          el.style.left=[id]oldLeft;
          el.style.top='';
          el.style.filter='';

          if ([id]tmrHide!=-1) clearInterval([id]tmrHide); [id]tmrHide=-1;

          document.getElementById('[id]_header').style.display='none';
          document.getElementById('[id]_content').style.display='none';

          if (navigator.userAgent.indexOf('Opera')!=-1)
            el.style.bottom=(document.body.scrollHeight*1-document.body.scrollTop*1
                            -document.body.offsetHeight*1+1*[id]popupBottom)+'px';
          
          if ([id]bChangeTexts)
          {
            [id]bChangeTexts=false;
            document.getElementById('[id]aCnt').innerHTML=[id]nMsg;
            document.getElementById('[id]titleEl').innerHTML=[id]nTitle;
          }

          [id]actualHgt=0; el.style.height=[id]actualHgt+'px';
          el.style.visibility='';
          if (![id]resetTimer) el.style.display='';
          [id]tmrId=setInterval([id]espopup_tmrTimer,([id]resetTimer?[stime]:20));
        }

        function [id]espopup_winLoad()
        {
          if ([id]oldonloadHndlr!=null) [id]oldonloadHndlr();

          elCnt=document.getElementById('[id]_content')
          elTit=document.getElementById('[id]_header');
          el=document.getElementById('[id]');
					[id]oldLeft=el.style.left;
          [id]popupBottom=el.style.bottom.substr(0,el.style.bottom.length-2);
          
          [id]titHgt=elTit.style.height.substr(0,elTit.style.height.length-2);
          [id]popupHgt=el.style.height;
          [id]popupHgt=[id]popupHgt.substr(0,[id]popupHgt.length-2); [id]actualHgt=0;
          [id]cntDelta=[id]popupHgt-(elCnt.style.height.substr(0,elCnt.style.height.length-2));

          if ([autoshow])
          {
            [id]resetTimer=true;
            [id]espopup_ShowPopup(null);
          }
        }

        function [id]espopup_tmrTimer()
        {
          el=document.getElementById('[id]');
          if ([id]resetTimer)
          {
            el.style.display='';
            clearInterval([id]tmrId); [id]resetTimer=false;
            [id]tmrId=setInterval([id]espopup_tmrTimer,[speed]);
          }
          [id]actualHgt+=5;
          if ([id]actualHgt>=[id]popupHgt)
          {
            [id]actualHgt=[id]popupHgt; clearInterval([id]tmrId); [id]tmrId=-1;
            document.getElementById('[id]_content').style.display='';
            if ([id]hideAfter!=-1) [id]tmrHide=setInterval([id]espopup_tmrHideTimer,[id]hideAfter);
          }
          if ([id]titHgt<[id]actualHgt-6)
            document.getElementById('[id]_header').style.display='';
          if (([id]actualHgt-[id]cntDelta)>0)
          {
            elCnt=document.getElementById('[id]_content')
            elCnt.style.display='';
            elCnt.style.height=([id]actualHgt-[id]cntDelta)+'px';
          }
          el.style.height=[id]actualHgt+'px';
        }
        
        function [id]espopup_tmrHideTimer()
        {
          clearInterval([id]tmrHide); [id]tmrHide=-1;
          el=document.getElementById('[id]');
          if ([id]hasFilters)
          {
            backCnt=document.getElementById('[id]_content').innerHTML;
            backTit=document.getElementById('[id]_header').innerHTML;
            document.getElementById('[id]_content').innerHTML='';
            document.getElementById('[id]_header').innerHTML='';
            el.style.filter='blendTrans(duration=1)';
            el.filters.blendTrans.apply();
            el.style.visibility='hidden';
            el.filters.blendTrans.play();
            document.getElementById('[id]_content').innerHTML=backCnt;
            document.getElementById('[id]_header').innerHTML=backTit;
          }
          else el.style.visibility='hidden';
        }
        
        function [id]espopup_dxTimer()
        {
          clearInterval([id]dxTimer); [id]dxTimer=-1;
        }
     
        function [id]espopup_Close()
        {
          if ([id]tmrId==-1)
          {
            el=document.getElementById('[id]');
            el.style.filter='';
            el.style.display='none';
            if ([id]tmrHide!=-1) clearInterval([id]tmrHide); [id]tmrHide=-1;
            [sclose]
          }
        }
    
        function [id]espopup_ShowWindow()
        {
          [slink]
          if ([id]nWin!=null) [id]nWin.close();
          [id]nWin=window.open('','[id]nWin','[winstyle], '+
            'menubar=no, resizable=no, status=no, toolbar=no, location=no');
          [id]nWin.document.write([id]nText);
        }

        var [id]mousemoveBack,[id]mouseupBack;
        var [id]ofsX,[id]ofsY;
        function [id]espopup_DragDrop(e)
        {
          [id]mousemoveBack=document.body.onmousemove;
          [id]mouseupBack=document.body.onmouseup;
          ox=(e.offsetX==null)?e.layerX:e.offsetX;
          oy=(e.offsetY==null)?e.layerY:e.offsetY;
          [id]ofsX=ox; [id]ofsY=oy;

          document.body.onmousemove=[id]espopup_DragDropMove;
          document.body.onmouseup=[id]espopup_DragDropStop;
          if ([id]tmrHide!=-1) clearInterval([id]tmrHide);
        }

        function [id]espopup_DragDropMove(e)
        {
          el=document.getElementById('[id]');          
          if (e==null&&event!=null)
          {
            el.style.left=(event.clientX*1+document.body.scrollLeft-[id]ofsX)+'px';
            el.style.top=(event.clientY*1+document.body.scrollTop-[id]ofsY)+'px';
            event.cancelBubble=true;
          }
          else
          {
            el.style.left=(e.pageX*1-[id]ofsX)+'px';
            el.style.top=(e.pageY*1-[id]ofsY)+'px';
            e.cancelBubble=true;
          }
          if ((event.button&1)==0) [id]espopup_DragDropStop();
        }

        function [id]espopup_DragDropStop()
        {
          document.body.onmousemove=[id]mousemoveBack;
          document.body.onmouseup=[id]mouseupBack;
        }

      //]]>
      </script>";

        /// <summary>
        /// 初始化脚本和颜色
        /// </summary>
        public PopupWin()
        {
            // {0} = E5EDFA - popupBackground  
            // {1} = 455690 - popupBorderDark  
            // {2} = A6B4CF - popupBorderLight 
            // {3} = 728EB8 - cntBorderDark    
            // {4} = B9C9EF - cntBorderLight   
            // {5} = E9EFF9 - cntBackground    
            // {6} = E0E9F8 - gradientStart    
            // {7} = FFFFFF - gradientEnd      
            // {8} = 1F336B - textColor        
            // {9} = 6A87B2 - xButton          
            // {10}= 45638F - xButtonOver      

            divDesign = @"background:#{0}; border-right:1px solid #{1}; border-bottom:1px solid #{1};
                  border-left:1px solid #{2}; border-top:1px solid #{2}; position:absolute;
                  z-index:9999; ";

            cntStyle = @"border-left:1px solid #{3}; border-top:1px solid #{3};
                 border-bottom:1px solid #{4}; border-right:1px solid #{4};
                 background:#{5}; padding:2px; overflow:hidden; text-align:center;
                 filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,
                 StartColorStr='#FF{6}', EndColorStr='#FF{7}');";
            cntStyleI = @"position:absolute; left:2px; width:{0}px; top:20px; height:{1}px;";
            cntStyleN = @"position:absolute; left:2px; width:{0}px; top:20px; height:{1}px;";

            aStyle = @"font:12px arial,sans-serif; color:#{8}; text-decoration:none;";
            aCommands = @"onmouseover=""style.textDecoration='underline';""
                  onmouseout=""style.textDecoration='none';""
                  href=""[cmd]""";

            hdrStyle = @"position:absolute; left:2px; width:[wid]px; top:2px; height:14px;
                 filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,
                 StartColorStr='#FF{6}', EndColorStr='#FF{7}');";

            title = "这里显示标题";
            msg = "在弹出窗口里显示的信息";
            fullmsg = "在新窗口里显示的文本";

            closeHtml = @"<span style=""position:absolute; right:0px; top:0px; cursor:pointer; color:#{9}; font:bold 12px arial,sans-serif; 
                  position:absolute; right:3px;""
                  onclick=""[id]espopup_Close()""
                  onmousedown=""event.cancelBubble=true;""
                  onmouseover=""style.color='#{10}';""
                  onmouseout=""style.color='#{9}';"">X</span>";

            sPopup = "<head><title>{1}</title><style type=\\\"text/css\\\">{2}</style></head>" +
              "<body><h1>{1}</h1><p>{0}</p></body>";

            spopStyle = "body {" +
              "    background:#[gs]; padding:5px;" +
              "    filter:progid:DXImageTransform.Microsoft.Gradient(" +
              "     GradientType=0,StartColorStr='#FF[gs]', EndColorStr='#FF[ge]');" +
              "  }" +
              "  h1 {" +
              "    font:bold 16px arial,sans-serif; color:#[clr]; " +
              "    text-align:center; margin:0px;" +
              "  }" +
              "  p {" +
              "    font:14px arial,sans-serif; color:#[clr];" +
              "  }";

            ColorStyle = PopupColorStyle.Blue;
            xOffset = yOffset = 15; popDock = PopupDocking.BottomRight;
            iHide = 5000; winSize = new Size(400, 250);
            Width = new Unit("200px");
            Height = new Unit("100px");
            startTime = 1000;
            popAction = PopupAction.MessageWindow;
        }

        #endregion

        #region 属性


        /// <summary>
        /// 设置或获取动作类型
        /// </summary>
        [Category("Action"), Description("动作类型(点击连接后).")]
        public PopupAction ActionType
        {
            get { return popAction; }
            set { popAction = value; }
        }


        /// <summary>
        /// 设置或获取新窗口里要显示的文本
        /// (当用户点击弹出窗口时)
        /// </summary>
        [Bindable(true), Category("Action"), Description("这里的文本将在新的窗口显示")]
        public string Text
        {
            get { return fullmsg; }
            set { fullmsg = value.Replace("\\", "\\\\").Replace("\"", "\\\""); }
        }


        /// <summary>
        /// 设置或获取点击连接时打开的地址或者js脚本
        /// </summary>
        [Bindable(true), Category("Action"), Description("点击连接时打开的地址或者js脚本")]
        public string Link
        {
            get { return sLink; }
            set { sLink = value; }
        }


        /// <summary>
        /// 设置或获取连接打开的目标方式
        /// </summary>
        [Bindable(true), Category("Action"), Description("连接打开的目标方式")]
        public string LinkTarget
        {
            get { return sTarget; }
            set { sTarget = value; }
        }


        /// <summary>
        /// 设置或获取颜色样式
        /// </summary>
        [Category("Design"), Description("定义颜色样式")]
        public PopupColorStyle ColorStyle
        {
            get { return clrStyle; }
            set
            {
                clrStyle = value;
                switch (value)
                {
                    case PopupColorStyle.Blue:
                        textColor = "1F336B";
                        xButtonOver = popupBorderDark = "455690";
                        xButton = cntBorderDark = "728EB8";
                        popupBorderLight = cntBorderLight = "B9C9EF";
                        popupBackground = cntBackground = gradientStart = "E0E9F8";
                        gradientEnd = "FFFFFF";
                        break;

                    case PopupColorStyle.Violet:
                        textColor = "200040";
                        xButtonOver = popupBorderDark = "400080";
                        xButton = cntBorderDark = "7D5AA0";
                        popupBorderLight = cntBorderLight = "B9AAC8";
                        popupBackground = cntBackground = gradientStart = "D2C8DC";
                        gradientEnd = "FFFFFF";
                        break;

                    case PopupColorStyle.Green:
                        textColor = "004000";
                        xButtonOver = popupBorderDark = "008000";
                        xButton = cntBorderDark = "5AA05A";
                        popupBorderLight = cntBorderLight = "AAC8AA";
                        popupBackground = cntBackground = gradientStart = "C8DCC8";
                        gradientEnd = "FFFFFF";
                        break;

                    case PopupColorStyle.Red:
                        textColor = "400000";
                        xButtonOver = popupBorderDark = "800000";
                        xButton = cntBorderDark = "A05A5A";
                        popupBorderLight = cntBorderLight = "C8AAAA";
                        popupBackground = cntBackground = gradientStart = "DCC8C8";
                        gradientEnd = "FFFFFF";
                        break;
                }
            }
        }


        /// <summary>
        /// 设置或获取弹出窗口显示的信息
        /// </summary>
        [Bindable(true), Category("Texts"), Description("弹出窗口显示的信息")]
        public string Message
        {
            get { return msg; }
            set { msg = value; }
        }


        /// <summary>
        /// 设置或获取弹出窗口和新窗口的标题
        /// </summary>
        [Bindable(true), Category("Texts"), Description("T弹出窗口和新窗口的标题")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        /// <summary>
        /// 设置或获取亮度的颜色
        /// </summary>
        [Bindable(true), Category("Design"), Description("亮度的颜色")]
        public Color GradientLight
        {
            get { return ColorFromString(gradientEnd); }
            set { ColorStyle = PopupColorStyle.Custom; gradientEnd = ColorToString(value); }
        }


        /// <summary>
        /// 设置或获取暗度的颜色(在Mozilla里即背景色)
        /// </summary>
        [Bindable(true), Category("Design"), Description("暗度的颜色(在Mozilla里即背景色)")]
        public Color GradientDark
        {
            get { return ColorFromString(gradientStart); }
            set { ColorStyle = PopupColorStyle.Custom; popupBackground = cntBackground = gradientStart = ColorToString(value); }
        }


        /// <summary>
        /// 设置或获取文本颜色
        /// </summary>
        [Bindable(true), Category("Design"), Description("文本颜色")]
        public Color TextColor
        {
            get { return ColorFromString(textColor); }
            set { ColorStyle = PopupColorStyle.Custom; textColor = ColorToString(value); }
        }


        /// <summary>
        /// 设置或获取亮度阴影的颜色
        /// </summary>
        [Bindable(true), Category("Design"), Description("阴影的颜色")]
        public Color LightShadow
        {
            get { return ColorFromString(popupBorderLight); }
            set { ColorStyle = PopupColorStyle.Custom; popupBorderLight = cntBorderLight = ColorToString(value); }
        }


        /// <summary>
        /// 设置或获取暗度阴影的颜色
        /// </summary>
        [Bindable(true), Category("Design"), Description("暗度阴影的颜色")]
        public Color DarkShadow
        {
            get { return ColorFromString(xButtonOver); }
            set { ColorStyle = PopupColorStyle.Custom; xButtonOver = popupBorderDark = ColorToString(value); }
        }


        /// <summary>
        /// 设置或获取阴影颜色
        /// </summary>
        [Bindable(true), Category("Design"), Description("阴影颜色")]
        public Color Shadow
        {
            get { return ColorFromString(xButton); }
            set { ColorStyle = PopupColorStyle.Custom; xButton = cntBorderDark = ColorToString(value); }
        }


        /// <summary>
        /// 设置或获取弹出窗口的收缩状态
        /// </summary>
        [Category("Layout"), Description("弹出窗口的收缩状态")]
        public PopupDocking DockMode
        {
            get { return popDock; }
            set { popDock = value; }
        }


        /// <summary>
        /// 设置或获取X轴的偏移坐标（从左或右）
        /// </summary>
        [Category("Layout"), Description("X轴的偏移坐标（从左或右）"), DefaultValue(15)]
        public int OffsetX
        {
            get { return xOffset; }
            set { xOffset = value; }
        }


        /// <summary>
        /// 设置或获取Y轴的偏移坐标（从底部）
        /// </summary>
        [Category("Layout"), Description("Y轴的偏移坐标（从底部）"), DefaultValue(15)]
        public int OffsetY
        {
            get { return yOffset; }
            set { yOffset = value; }
        }


        /// <summary>
        /// 设置或获取窗口显示的时间，默认为500毫秒（-1为无限时间）
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(500),
       Description("窗口显示的时间，默认为500毫秒（-1为无限时间）")]
        public int HideAfter
        {
            get { return iHide; }
            set { iHide = value; }
        }


        /// <summary>
        /// 设置或获取弹出的速度
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(20),
       Description("设置或获取弹出的速度")]
        public int PopupSpeed
        {
            get { return iSpeed; }
            set { iSpeed = value; }
        }


        /// <summary>
        /// 设置或获取显示弹出窗口之前的延迟时间，默认为1000毫秒
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(1000),
      Description("显示弹出窗口之前的延迟时间，默认为1000毫秒")]
        public int ShowAfter
        {
            get { return startTime; }
            set { startTime = value; }
        }


        /// <summary>
        /// 页面加载时自动显示弹出窗口（在设置的ShowAfter属性之后）
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("页面加载时自动显示弹出窗口（在设置的ShowAfter属性之后）")]
        public bool AutoShow
        {
            get { return bAutoShow; }
            set { bAutoShow = value; }
        }


        /// <summary>
        /// 设置或获取是否允许拖动弹出窗口
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("是否允许拖动弹出窗口")]
        public bool DragDrop
        {
            get { return bDragDrop; }
            set { bDragDrop = value; }
        }


        /// <summary>
        /// 设置或获取打开窗口大小
        /// </summary>
        [Bindable(true), Category("Window"), Description("打开窗口大小")]
        public Size WindowSize
        {
            get { return winSize; }
            set { winSize = value; }
        }


        /// <summary>
        /// 设置或获取新窗口是否允许滚动条
        /// </summary>
        [Bindable(true), Category("Window"), Description("新窗口是否允许滚动条"),
            DefaultValue(true)]
        public bool WindowScroll
        {
            get { return winScroll; }
            set { winScroll = value; }
        }


        /// <summary>
        /// 是否在弹出窗口中显示连接和启用动作
        /// </summary>
        [Bindable(true), DefaultValue(true),
           Category("Action"), Description("是否在弹出窗口中显示连接和启用动作")]
        public bool ShowLink
        {
            get { return bShowLink; }
            set { bShowLink = value; }
        }


        #endregion

        #region 方法

        /// <summary>
        /// 将颜色转为字符串（不能写颜色名字）
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>字符串（没有#号）</returns>
        private string ColorToString(Color color)
        {
            return color.R.ToString("x").PadLeft(2, '0') +
              color.G.ToString("x").PadLeft(2, '0') + color.B.ToString("x").PadLeft(2, '0');
        }


        /// <summary>
        /// 将字符串转换为RGB格式的颜色
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>颜色</returns>
        private Color ColorFromString(string str)
        {
            return ColorTranslator.FromHtml("#" + str);
        }


        /// <summary>
        /// 将{0}..{10}替换成颜色
        /// </summary>
        /// <param name="html">Html代码</param>
        private string PutColors(string html)
        {
            return String.Format(html, popupBackground, popupBorderDark, popupBorderLight,
              cntBorderDark, cntBorderLight, cntBackground, gradientStart,
              gradientEnd, textColor, xButton, xButtonOver);
        }

        /// <summary>
        /// 使用指定的输出参数呈现此控件
        /// </summary>
        /// <param name="output">输出HTML代码</param>
        protected override void Render(HtmlTextWriter output)
        {
            string br = Page.Request.Browser.Browser;
            string script = sScript, sps = spopStyle;

            string acmd = "";
            switch (popAction)
            {
                case PopupAction.MessageWindow:
                    acmd = aCommands.Replace("[cmd]", "javascript:[id]espopup_ShowWindow();");
                    script = script.Replace("[slink]", "");
                    script = script.Replace("[sclose]", "");
                    break;
                case PopupAction.RaiseEvents:
                    acmd = aCommands.Replace("[cmd]", "javascript:[id]espopup_ShowWindow();");
                    string scriptClick = (LinkClicked == null) ? "" :
                      (Page.GetPostBackClientEvent(this, "C") + "; return;");
                    string scriptClose = (PopupClosed == null) ? "" :
                      (Page.GetPostBackClientEvent(this, "X") + "; return;");
                    script = script.Replace("[slink]", scriptClick);
                    script = script.Replace("[sclose]", scriptClose);
                    break;
                case PopupAction.OpenLink:
                    acmd = aCommands.Replace("[cmd]", sLink);
                    script = script.Replace("[slink]", "");
                    script = script.Replace("[sclose]", "");
                    if (sTarget != "") acmd += " target=\"" + sTarget + "\"";
                    break;
            }

            sps = sps.Replace("[gs]", gradientStart);
            sps = sps.Replace("[ge]", gradientEnd);
            sps = sps.Replace("[clr]", textColor);

            script = script.Replace("[winstyle]", String.Format(
              "width={0},height={1},scrollbars={2}", winSize.Width,
              winSize.Height, (winScroll ? "yes" : "no")));
            script = script.Replace("[hide]", iHide.ToString());
            script = script.Replace("[stime]", startTime.ToString());
            script = script.Replace("[id]", ClientID);
            script = script.Replace("[ie]", (br == "IE" ? "true" : "false"));
            script = script.Replace("[popup]", String.Format(sPopup, fullmsg, title, sps));
            script = script.Replace("[autoshow]", bAutoShow.ToString().ToLower());
            script = script.Replace("[speed]", iSpeed.ToString());

            string divPos = String.Format("width:{0}; height:{1}; ", Width, Height);
            string cntsI = String.Format(cntStyleI, Width.Value - 10, Height.Value - 27);
            string cntsN = String.Format(cntStyleN, Width.Value - 10, Height.Value - 28);

            string sDragDrop = "";
            if (bDragDrop)
                sDragDrop = " onmousedown=\"return " + ClientID + "espopup_DragDrop(event);\" ";
            if (popDock == PopupDocking.BottomLeft) divPos += "left:"; else divPos += "right:";
            divPos += string.Format("{0}px; bottom:{1}px;", xOffset, yOffset);
            output.Write(script + String.Format("<div id=\"{0}\" " +
              "style=\"display:none; {1} {2}\" onselectstart=\"return false;\" {4}>" +
              "<div id=\"{3}\" style=\"cursor:default; display:none; {5}\">{6}</div>" +
              "<div id=\"{7}\" onmousedown=\"event.cancelBubble=true;\" style=\"display:none; {8}\">" +
              ((bShowLink == true) ?
                "<a style=\"{9}\" {10} id=\"{11}\">{12}</a></div></div>" :
                "<span style=\"{9}\" id=\"{11}\">{12}</span></div></div>"),
              ClientID, PutColors(divDesign), divPos, ClientID + "_header", sDragDrop, PutColors(hdrStyle).
              Replace("[wid]", (Width.Value - 6).ToString()) + PutColors(aStyle),
              "<span id=\"" + ClientID + "titleEl\">" + title + "</span>" + PutColors(closeHtml).Replace("[id]", ClientID), ClientID + "_content",
              PutColors(cntStyle) + ((br != "Netscape" && br != "Mozilla") ? cntsI : cntsN), PutColors(aStyle),
              acmd.Replace("[id]", ClientID), ClientID + "aCnt", msg));
        }


        /// <summary>
        /// 在新窗口中产生HTML代码
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="text">文本</param>
        /// <returns>在新窗口中产生的HTML代码</returns>
        internal string GetWinText(string title, string text)
        {
            string sps = spopStyle;
            sps = sps.Replace("[gs]", gradientStart);
            sps = sps.Replace("[ge]", gradientEnd);
            sps = sps.Replace("[clr]", textColor);

            return String.Format(sPopup, text.Replace("\\", "\\\\").Replace("\"", "\\\""), title, sps);
        }

        /// <summary>
        /// 返回HTML代码
        /// </summary>
        internal string GetDesignCode()
        {
            string divPos = String.Format("width:{0}; height:{1}; ",
              Width, Height);
            string cntsI = String.Format(cntStyleI, Width.Value - 6, Height.Value - 24);

            return String.Format("<div id=\"{0}\" " +
              "style=\"{1} {2}; left:0px; top:0px; \">" +
              "<div id=\"{3}\" style=\"{4}\">{5}</div>" +
              "<div id=\"{6}\" style=\"{7}\">" +
              "<a style=\"{8}\" {9}>{10}</a></div></div>",
              ClientID, PutColors(divDesign), divPos, ClientID + "_header", PutColors(hdrStyle).
              Replace("[wid]", (Width.Value - 6).ToString()) +
              PutColors(aStyle), title + PutColors(closeHtml).Replace("[id]", ClientID), ClientID + "_content",
              PutColors(cntStyle) + cntsI, PutColors(aStyle), aCommands.Replace("[id]", ClientID), msg);
        }

        #endregion

        #region 事件和句柄

        /// <summary>
        /// 上升事件
        /// </summary>
        /// <param name="eventArgument">在连接上点击或者关闭</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "C") LinkClicked(this, EventArgs.Empty);
            if (eventArgument == "X") PopupClosed(this, EventArgs.Empty);
        }


        /// <summary>
        /// 在弹出窗口中点击连接
        /// </summary>
        [Category("Popup"), Description("在弹出窗口中点击连接")]
        public event EventHandler LinkClicked;


        /// <summary>
        /// 点击弹出窗口中的'X'
        /// </summary>
        [Category("Popup"), Description("点击弹出窗口中的'X'")]
        public event EventHandler PopupClosed;

        #endregion
    }

    /// <summary>
    /// 弹出窗口设计类
    /// </summary>
    public class PopupWinDesigner : ControlDesigner
    {
        #region Overriden methods

        /// <summary>
        /// 返回html代码
        /// </summary>
        public override string GetDesignTimeHtml()
        {
            try
            {
                return ((PopupWin)Component).GetDesignCode();
            }
            catch (Exception er)
            {
                return GetErrorDesignTimeHtml(er);
            }
        }

        #endregion
    }


    /// <summary>
    /// 定义颜色样式
    /// </summary>
    public enum PopupColorStyle
    {
        #region 成员

        /// <summary> 蓝色 (默认) </summary>
        Blue,
        /// <summary> 红色 </summary>
        Red,
        /// <summary> 绿色 </summary>
        Green,
        /// <summary> 紫色 </summary>
        Violet,
        /// <summary> 自定义颜色 </summary>
        Custom

        #endregion
    }


    /// <summary>
    /// 弹出窗口显示位置
    /// </summary>
    public enum PopupDocking
    {
        #region 成员

        /// <summary>控件出现在左下方 </summary>
        BottomLeft,
        /// <summary>控件出现在在右下方 </summary>
        BottomRight

        #endregion
    }


    /// <summary>
    /// 点击连接后执行的事件
    /// </summary>
    public enum PopupAction
    {
        #region 成员

        /// <summary> 上升事件 </summary>
        RaiseEvents,
        /// <summary> 打开新的窗口显示信息 </summary>
        MessageWindow,
        /// <summary> 打开连接或者js脚本 </summary>
        OpenLink

        #endregion
    }
}

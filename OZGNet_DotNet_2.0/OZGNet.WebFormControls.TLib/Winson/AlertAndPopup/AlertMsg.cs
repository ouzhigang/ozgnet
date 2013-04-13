using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Drawing;


#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("Winson.WControls.AlertAndPopup.App_Themes.bg_dialog.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("Winson.WControls.AlertAndPopup.App_Themes.delete.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("Winson.WControls.AlertAndPopup.App_Themes.icon_big_info.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("Winson.WControls.AlertAndPopup.App_Themes.tit_dialog.gif", "image/gif")]
//[assembly: System.Web.UI.WebResource("Winson.WControls.AlertAndPopup.css.TreeAlertMsg.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("Winson.WControls.AlertAndPopup.js.TreeAlertMsg.js", "text/javascript")]
#endregion

namespace OZGNet.WebFormControls.ThirdPart.Winson.AlertAndPopup
{

    ////////////////////////////////////////////////////////////
    ////                                                    ////
    ////                 TreeAlertControls                  ////
    ////    Namespace:  TreeControls.AlertMsg               ////
    ////    CreateName: Tree                                ////
    ////    Version:    1.0 bate2                           ////
    ////    CreateDate: 2007-5-9                            ////
    ////                                                    ////
    ////////////////////////////////////////////////////////////

    /// <summary>
    /// 提示框控件
    /// </summary>
    public class AlertMsg : System.Web.UI.WebControls.CompositeControl, IPostBackEventHandler
    {
        #region 事件
        /// <summary>
        /// 选择枚举
        /// </summary>
        public enum SelectEventType
        {
            Confirm,
            Cancel
        }

        private static SelectEventType selectEventType;

        /// <summary>
        /// 获得或设置客户选择是否（使用时最好不要使用设置功能，有可能改变客户选择）
        /// </summary>
        public SelectEventType TreeAlertSelectEventType
        {
            get
            {
                return selectEventType; ;
            }
            set
            {
                selectEventType = value;
            }
        }


        private static readonly object EventSelect = new object();
        /// <summary>
        /// 如确定所提交的事件
        /// </summary>
        public event EventHandler OnSelect
        {
            add
            {
                Events.AddHandler(EventSelect, value);
            }
            remove
            {
                Events.RemoveHandler(EventSelect, value);
            }
        }

        /// <summary>
        /// 选择事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void Select(EventArgs e)
        {
            EventHandler ehEvent = (EventHandler)Events[EventSelect];
            if (ehEvent != null)
            {
                ehEvent(this, e);
            }
        }

        /// <summary>
        /// 回调事件
        /// </summary>
        /// <param name="eventName"></param>
        public void RaisePostBackEvent(string eventName)
        {
            if (eventName == "OnConfirm")
            {
                this.TreeAlertSelectEventType = SelectEventType.Confirm;
                Select(EventArgs.Empty);
            }
            else if (eventName == "OnCancel")
            {
                this.TreeAlertSelectEventType = SelectEventType.Cancel;
                Select(EventArgs.Empty);
            }
        }
        #endregion

        /// <summary>
        /// 对话框类型枚举
        /// </summary>
        public enum TreeAlertType
        {
            Alert,
            Confirm
        }

        #region 变量
        System.Web.UI.WebControls.Button btConfirm;// = new System.Web.UI.WebControls.Button();
        System.Web.UI.WebControls.Button btCancel;// = new System.Web.UI.WebControls.Button();

        //确定按钮文字
        private static string sConfirmButtonText = "确  定";
        //取消按钮文字
        private static string sCancelButtonText = "取  消";
        //确定按钮宽度
        private static int iConfirmButtonWidth = 70;
        //取消按钮宽度
        private static int iCancelButtonWidth = 70;
        //对话框类型
        private static TreeAlertType eTreeAlertType = TreeAlertType.Alert;
        //错误信息
        private static string sMsg = "错误信息";
        //标题
        private static string sTitle = "【系统提示信息】";
        //是否显示
        private static bool bShowType = false;
        //遮盖层透明度
        private static int iOverlayBackColorOpacity = 30;
        //遮盖层z-index
        private static int iBackColorz_index = 99999;
        //遮盖层颜色
        private static string sOverlayBackColor = "#DDDDFF";
        //对话框标题头背景图片Url
        private static string sAlertMsgHeadImgUrl = "";
        //关闭按钮图片Url
        private static string sAlertMsgColseImgUrl = "";
        //关闭图片高
        private static float fCloseImgHeight = 16.0f;
        //关闭图片宽
        private static float fCloseImgWidth = 16.0f;
        //消息背景图片Url
        private static string sAlertMsgBackImgUrl = "";
        //左侧警告图片Url
        private static string sAlertMsgAdmonitionImgUrl = "";
        //警告图片高(120以内)
        private static float fAdmonitionImgHeight = 52.0f;
        //警告图片宽(70以内)
        private static float fAdmonitionImgWidth = 64.0f;
        #endregion

        #region 构造函数
        public AlertMsg()
        {
            eTreeAlertType = TreeAlertType.Alert;
            sConfirmButtonText = "确  定";
            sCancelButtonText = "取  消";
            iConfirmButtonWidth = 70;
            iCancelButtonWidth = 70;
            sMsg = "错误信息";
            sTitle = "【系统提示信息】";
            bShowType = false;
            iOverlayBackColorOpacity = 30;
            iBackColorz_index = 99999;
            sOverlayBackColor = "#36CDFF";
            sAlertMsgHeadImgUrl = "";
            sAlertMsgColseImgUrl = "";
            fCloseImgHeight = 16.0f;
            fCloseImgWidth = 16.0f;
            sAlertMsgBackImgUrl = "";
            sAlertMsgAdmonitionImgUrl = "";
            fAdmonitionImgHeight = 52.0f;
            fAdmonitionImgWidth = 64.0f;
            bShowType = false;
        }
        #endregion

        #region 属性
        public TreeAlertType TreeAlertShowType
        {
            get
            {
                return eTreeAlertType;
            }
            set
            {
                eTreeAlertType = value;
            }
        }

        /// <summary>
        /// 设置或获得标题信息
        /// </summary>
        public string Msg
        {
            get
            {
                return sMsg;
            }
            set
            {
                sMsg = value;
            }
        }

        /// <summary>
        /// 设置或获得错误信息属性
        /// </summary>
        public string Title
        {
            get
            {
                return sTitle;
            }
            set
            {
                sTitle = "【" + value + "】";
            }
        }

        /// <summary>
        /// 设置或获得遮盖层透明度
        /// </summary>
        public int OverlayBackColorOpacity
        {
            get
            {
                return iOverlayBackColorOpacity;
            }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new Exception("AlertMsg控件遮盖层透明度应在0～100之间");
                }
                else
                {
                    iOverlayBackColorOpacity = value;
                }
            }
        }

        /// <summary>
        /// 设置或获得遮盖层z_index
        /// </summary>
        public int BackColorz_index
        {
            get
            {
                return iBackColorz_index;
            }
            set
            {
                iBackColorz_index = value;
            }
        }

        /// <summary>
        /// 获得或设置遮盖层背景颜色(格式：#FFFFFF)
        /// </summary>
        public string OverlayBackColor
        {
            get
            {
                return sOverlayBackColor;
            }
            set
            {
                if (CheckColorStr(value))
                {
                    sOverlayBackColor = value;
                }
                else
                {
                    throw new Exception("AlertMsg控件请输入正确的颜色");
                }
            }
        }

        /// <summary>
        /// 设置对话况标题头背景图片(标题26px高)
        /// </summary>
        public string AlertMsgHeadImgUrl
        {
            set
            {
                sAlertMsgHeadImgUrl = value;
            }
        }


        #endregion

        #region 私有方法
        /// <summary>
        /// 验证颜色字串格式
        /// </summary>
        /// <param name="sColor"></param>
        /// <returns></returns>
        private bool CheckColorStr(string sColor)
        {
            string sCheck = "0123456789ABCDEFabcdef";

            if (sColor.Length != 7)
            {
                return false;
            }

            if (sColor[0].ToString() != "#")
            {
                return false;
            }

            for (int i = 1; i < 7; i++)
            {
                if (!sCheck.Contains(sColor[i].ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 呈现前
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Page != null)
            {
                AlertMsg existingInstance = AlertMsg.GetCurrent(this.Page);

                if (existingInstance != null)
                {
                    throw new Exception("每个页面只能存在一个AlertMsg控件");
                }
                Page.Items[typeof(AlertMsg)] = this;
                this.Page.ClientScript.RegisterClientScriptResource(this.GetType(), "Winson.WControls.AlertAndPopup.js.TreeAlertMsg.js");
            }
            else
            {
                throw new Exception("请验证页面");
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// 创建子控件
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            btConfirm = new System.Web.UI.WebControls.Button();
            btCancel = new System.Web.UI.WebControls.Button();
            btConfirm.Attributes.Add("onclick", "javascript:return TreeAlertMsgHiddenMsg();");

            btConfirm.Click += new EventHandler(TreeAlertMsgbtConfirm_Click);
            btConfirm.ID = "TreeAlertMsgTable_Ok";
            btConfirm.Width = iConfirmButtonWidth;
            btConfirm.Attributes.Add("onmousemove", "this.className='TreeAlertMsgbt_mouseover'");
            btConfirm.Attributes.Add("onmouseout", "this.className='TreeAlertMsgbt_mouseout'");
            btConfirm.Attributes.Add("onmousedown", "this.className='TreeAlertMsgbt_mousedown'");
            btConfirm.Attributes.Add("onmouseup", "this.className='TreeAlertMsgbt_mouseup'");
            btConfirm.CssClass = "TreeAlertMsgbt_mouseout";
            btConfirm.Text = sConfirmButtonText;

            btCancel.Attributes.Add("onclick", "javascript:return TreeAlertMsgHiddenMsg();");
            btCancel.Click += new EventHandler(TreeAlertMsgbtCancel_Click);
            btCancel.ID = "TreeAlertMsgTable_No";
            btCancel.Width = iCancelButtonWidth;
            btCancel.Attributes.Add("onmousemove", "this.className='TreeAlertMsgbt_mouseover'");
            btCancel.Attributes.Add("onmouseout", "this.className='TreeAlertMsgbt_mouseout'");
            btCancel.Attributes.Add("onmousedown", "this.className='TreeAlertMsgbt_mousedown'");
            btCancel.Attributes.Add("onmouseup", "this.className='TreeAlertMsgbt_mouseup'");
            btCancel.CssClass = "TreeAlertMsgbt_mouseout";
            btCancel.Text = sCancelButtonText;

            this.Controls.Add(btConfirm);
            this.Controls.Add(btCancel);
        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeAlertMsgbtConfirm_Click(object sender, EventArgs e)
        {
            this.TreeAlertSelectEventType = SelectEventType.Confirm;
            Select(EventArgs.Empty);
        }

        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeAlertMsgbtCancel_Click(object sender, EventArgs e)
        {
            this.TreeAlertSelectEventType = SelectEventType.Cancel;
            Select(EventArgs.Empty);
        }

        #endregion

        #region 输出
        /// <summary>
        /// 重写Html输出函数
        /// </summary>
        /// <param name="writer">要写出到的 HTML 编写器</param>
        protected override void Render(HtmlTextWriter writer)
        {
            //AddAttributesToRender(writer);

            //base.Render(writer);
            writer.Write("<span name='" + this.UniqueID + "'>");

            #region ButtonCss
            writer.Write("<style type='text/css'>");
            writer.Write("   .TreeAlertMsgbt_mouseout {");
            writer.Write("       BORDER-RIGHT: #2C59AA 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #2C59AA 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 12px; FILTER: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr=#ffffff, EndColorStr=#C3DAF5); BORDER-LEFT: #2C59AA 1px solid; CURSOR: hand; COLOR: black; PADDING-TOP: 2px; BORDER-BOTTOM: #2C59AA 1px solid;");
            writer.Write("   }");
            writer.Write("   .TreeAlertMsgbt_mouseover {");
            writer.Write("       BORDER-RIGHT: #2C59AA 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #2C59AA 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 12px; FILTER: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr=#ffffff, EndColorStr=#D7E7FA); BORDER-LEFT: #2C59AA 1px solid; CURSOR: hand; COLOR: black; PADDING-TOP: 2px; BORDER-BOTTOM: #2C59AA 1px solid;");
            writer.Write("   }");
            writer.Write("   .TreeAlertMsgbt_mousedown {");
            writer.Write("       BORDER-RIGHT: #FFE400 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #FFE400 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 12px; FILTER: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr=#ffffff, EndColorStr=#C3DAF5); BORDER-LEFT: #FFE400 1px solid; CURSOR: hand; COLOR: black; PADDING-TOP: 2px; BORDER-BOTTOM: #FFE400 1px solid;");
            writer.Write("   }");
            writer.Write("   .TreeAlertMsgbt_mouseup {");
            writer.Write("       BORDER-RIGHT: #2C59AA 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #2C59AA 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 12px; FILTER: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr=#ffffff, EndColorStr=#C3DAF5); BORDER-LEFT: #2C59AA 1px solid; CURSOR: hand; COLOR: black; PADDING-TOP: 2px; BORDER-BOTTOM: #2C59AA 1px solid;");
            writer.Write("   }");
            writer.Write("  .TreeOverlayBackgroundDivCss {");
            writer.Write("      display: none;position: absolute; top: 0px; margin: 0px; float: left;left: 0px; width: document.documentElement.clientWidth; height: document.body.clientHeight+document.body.scrollHeight; background: " + sOverlayBackColor + ";opacity: " + (iOverlayBackColorOpacity / 100).ToString() + " !important; filter: alpha(opacity=" + (iOverlayBackColorOpacity).ToString() + "); z-index: " + iBackColorz_index.ToString() + ";");
            writer.Write("  }");
            writer.Write("  .TreeAlertMsgDivCss {");
            writer.Write("      display: none;position: absolute;width:398px;z-index: " + (iBackColorz_index + 1).ToString() + ";height:206px;");
            writer.Write("  }");
            writer.Write("  .TreeAlertMsgTableCss {");
            writer.Write("      border: 1px solid #000000;position: absolute;");
            writer.Write("  }");
            writer.Write("  .TreeAlertMsgHeadTdCss {");
            if (sAlertMsgHeadImgUrl == string.Empty)
            {
                writer.Write("      background: url(" + this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Winson.WControls.AlertAndPopup.App_Themes.tit_dialog.gif") + ") repeat-x;");
            }
            else
            {
                writer.Write("      background: url(" + sAlertMsgHeadImgUrl + ") repeat-x;");
            }
            writer.Write("  }");
            writer.Write("  .TreeAlertMsgHeadTableCss {");
            writer.Write("      -moz-user-select:none;' onselectstart='return false;");
            writer.Write("  }");
            writer.Write("</style>");
            #endregion

            #region 遮盖层
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeOverlayBackgroundDiv");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeOverlayBackgroundDiv");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "TreeOverlayBackgroundDivCss");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
            #endregion

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeAlertMsgDiv");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeAlertMsgDiv");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "TreeAlertMsgDivCss");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeAlertMsgTable");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeAlertMsgTable");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "398px");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "TreeAlertMsgTableCss");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

            #region Head
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "26px");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "TreeAlertMsgHeadTdCss");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "TreeAlertMsgHeadTableCss");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 20px; width: 6px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeAlertMsgTable_Title");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeAlertMsgTable_Title");
            writer.AddAttribute("onmousedown", "return TreeAlertMsgmoveStart(event);");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 20px; font-size: 10pt; font-family: Arial, 宋体;cursor: move;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.Write(sTitle);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeAlertMsgTable_Close");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeAlertMsgTable_Close");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 27px; height: 20px;cursor: pointer;");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return TreeAlertMsgHiddenMsg();");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddAttribute(HtmlTextWriterAttribute.Width, fCloseImgWidth.ToString() + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, fCloseImgHeight.ToString() + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            if (sAlertMsgColseImgUrl == string.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Winson.WControls.AlertAndPopup.App_Themes.delete.gif"));
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, sAlertMsgColseImgUrl);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();

            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 20px; width: 6px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            #endregion

            #region Body
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            if (sAlertMsgBackImgUrl == string.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "background: url(" + this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Winson.WControls.AlertAndPopup.App_Themes.bg_dialog.gif") + ") repeat-x; height: 180px;");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "background: url(" + sAlertMsgBackImgUrl + ") repeat-x; height: 180px;");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:10px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 10px; height: 125px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 125px;width : 10%;");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            if (sAlertMsgAdmonitionImgUrl == string.Empty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Winson.WControls.AlertAndPopup.App_Themes.icon_big_info.gif"));
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, sAlertMsgAdmonitionImgUrl);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Msg");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, fAdmonitionImgHeight.ToString() + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, fAdmonitionImgWidth.ToString() + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "font-size: 9pt; font-family: Arial, 宋体;height: 125px;");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeAlertMsgTable_MsgTd");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeAlertMsgTable_MsgTd");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:125px; width: 280px; padding-right: 10px; overflow-y: auto; padding-left: 10px;azimuth: center; text-align: center;");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Width, "98%");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "TreeAlertMsgTable_Table_MsgTd");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "TreeAlertMsgTable_Table_MsgTd");
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "middle");
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "left");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:120px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(sMsg);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 10px; height: 125px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
            writer.RenderEndTag();
            #endregion

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:10px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            if (eTreeAlertType == TreeAlertType.Confirm)
            {
                /////////////////////选择按钮

                btConfirm.RenderControl(writer);

                for (int i = 0; i < 20; i++)
                {
                    writer.Write("&nbsp;");
                }

                btCancel.RenderControl(writer);
            }
            else
            {
                btConfirm.RenderControl(writer);
            }

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:10px;");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "4");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.Write("</span>");
            if (!this.Page.ClientScript.IsStartupScriptRegistered("javascriptname1"))
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "javascriptname1", this.GetJavascript(), false);
            }
        }

        /// <summary>
        /// 输出javascript
        /// </summary>
        /// <returns></returns>
        protected string GetJavascript()
        {
            JavaScriptWriter js = new JavaScriptWriter(true);

            if (bShowType)
            {
                js.AddLine("TreeAlertMsgShowMsg('" + sMsg + "');");
            }
            return js.ToString();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置关闭按钮图片(Max标题26*26宽)
        /// </summary>
        /// <param name="sImgUrl">图片URL</param>
        /// <param name="fHeight">图片高</param>
        /// <param name="fWidth">图片宽</param>
        public void AlertMsgColseImg(string sImgUrl, float fHeight, float fWidth)
        {
            if (fHeight < 0 || fHeight > 20)
            {
                throw new Exception("AlertMsg控件关闭按钮图片高度应在0～20之间");
            }

            if (fWidth < 0 || fWidth > 20)
            {
                throw new Exception("AlertMsg控件关闭按钮图片宽度应在0～20之间");
            }
            sAlertMsgColseImgUrl = sImgUrl;
            fCloseImgHeight = fHeight;
            fCloseImgWidth = fWidth;
        }

        /// <summary>
        /// 设置左侧警告图片(Max图片120*70宽)
        /// </summary>
        /// <param name="sImgUrl"></param>
        /// <param name="fHeight"></param>
        /// <param name="fWidth"></param>
        public void sAlertMsgAdmonitionImg(string sImgUrl, float fHeight, float fWidth)
        {
            if (fHeight < 0 || fHeight > 120)
            {
                throw new Exception("AlertMsg控件警告图片高度应在0～26之间");
            }

            if (fWidth < 0 || fWidth > 70)
            {
                throw new Exception("AlertMsg控件警告图片宽度应在0～26之间");
            }
            sAlertMsgAdmonitionImgUrl = sImgUrl;
            fAdmonitionImgHeight = fHeight;
            fAdmonitionImgWidth = fWidth;
        }

        /// <summary>
        /// 检查页面上是否已存在该控件
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static AlertMsg GetCurrent(Page page)
        {
            if (page == null)
            {
                throw new Exception("请验证页面");
            }
            int i = page.Items.Count;
            return page.Items[typeof(AlertMsg)] as AlertMsg;
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="sMsg"></param>
        public void ShowTreeAlertMsg(string _sMsg)
        {
            bShowType = true;
            sMsg = _sMsg;
        }

        /// <summary>
        /// 隐藏对话框
        /// </summary>        
        public void HiddenTreeAlertMsg()
        {
            bShowType = false;
        }
        #endregion
    }
}
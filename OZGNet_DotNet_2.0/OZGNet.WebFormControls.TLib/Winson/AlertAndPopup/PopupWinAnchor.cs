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
    /// 弹出窗口钩子控件
    /// </summary>
    [DefaultProperty("PopupToShow"), ToolboxData("<{0}:PopupWinAnchor runat=server></{0}:PopupWinAnchor>")]
    [Designer(typeof(AnchorDesigner))]
    public class PopupWinAnchor : System.Web.UI.WebControls.WebControl
    {
        #region 变量

        string controlId, controlLink, jsEvent;
        string snMsg, snText, snTitle;
        bool bChangeText = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建控件
        /// </summary>
        public PopupWinAnchor()
        {
            jsEvent = "onclick";
            bChangeText = false;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 是否把弹出窗口里的文本替换为新的文本
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue(false)]
        [Description("是否把弹出窗口里的文本替换为新的文本")]
        public bool ChangeTexts
        {
            get { return bChangeText; }
            set { bChangeText = value; }
        }


        /// <summary>
        /// 新的信息文本
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue("")]
        [Description("新的信息文本")]
        public string NewMessage
        {
            get { return snMsg; }
            set { snMsg = value; }
        }


        /// <summary>
        /// 新的弹出窗口标题
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue("")]
        [Description("新的弹出窗口标题")]
        public string NewTitle
        {
            get { return snTitle; }
            set { snTitle = value; }
        }


        /// <summary>
        /// 在新窗口里显示的新文本
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue("")]
        [Description("在新窗口里显示的新文本")]
        public string NewText
        {
            get { return snText; }
            set { snText = value; }
        }


        /// <summary>
        /// JavaScript事件句柄
        /// </summary>
        [Bindable(true), Category("Anchor"), DefaultValue("onclick")]
        [Editor(typeof(JavaScriptEventEditor), typeof(UITypeEditor))]
        [Description("JavaScript事件句柄")]
        public string HandledEvent
        {
            get { return jsEvent; }
            set { jsEvent = value; }
        }


        /// <summary>
        /// 事件发生时显示弹出窗口控件
        /// </summary>
        [Bindable(true), Category("Anchor"), DefaultValue("")]
        [Editor(typeof(PopupControlsEditor), typeof(UITypeEditor))]
        [Description("事件发生时显示弹出窗口控件")]
        public string PopupToShow
        {
            get { return controlId; }
            set { controlId = value; }
        }


        /// <summary>
        /// 触发弹出窗口显示的事件
        /// </summary>
        [Bindable(true), Category("Anchor"), DefaultValue("")]
        [Editor(typeof(AllControlsEditor), typeof(UITypeEditor))]
        [Description("触发弹出窗口显示的事件")]
        public string LinkedControl
        {
            get { return controlLink; }
            set { controlLink = value; }
        }

        #endregion

        #region 方法

        /// <summary> 
        /// 显示弹出窗口控件时呈现脚本
        /// </summary>
        /// <param name="output">HtmlTextWriter对象</param>
        protected override void Render(HtmlTextWriter output)
        {
            output.Write(@"
        <script type=""text/javascript"">
        //<![CDATA[

        var " + ID + @"oldOnLoad=window.onload;
        window.onload=" + ID + @"espopup_anchorInit;
        function " + ID + @"espopup_anchorInit()
        {
          if (" + ID + @"oldOnLoad!=null) " + ID + @"oldOnLoad();
          document.getElementById('" + controlLink + @"')." + jsEvent + @"=" +
              ID + @"espopup_anchorEvent;
        }

        function " + ID + @"espopup_anchorEvent()
        {
          ");
            if (bChangeText)
            {
                Control ct = Page.FindControl(controlId);
                if (ct != null)
                {
                    output.Write(controlId + "nText=\"" +
                      ((PopupWin)ct).GetWinText(snTitle, snText) + "\";\n");
                }
                output.Write(controlId + "nMsg=\"" + snMsg + "\";\n");
                output.Write(controlId + "nTitle=\"" + snTitle + "\";\n");
                output.Write(controlId + "bChangeTexts=true;\n");
            }
            else
            {
                output.Write(controlId + "bChangeTexts=false;\n");
            }
            output.Write("\n" + controlId + @"espopup_ShowPopup('" + ID + @"');
        }
        //]]>
        </script>");
        }

        #endregion
    }


    /// <summary>
    /// 显示弹出窗口控件类
    /// </summary>
    public class AnchorDesigner : ControlDesigner
    {
        #region Overriden methods

        /// <summary>
        /// 返回HTML代码
        /// </summary>
        public override string GetDesignTimeHtml()
        {
            return "<div style=\"padding:2px; background-color: ButtonFace;color:ButtonText; " +
              "border-style:outset; border-width:1px; font: 75% 'Microsoft Sans Serif';\"><b>" +
              "PopupWinAnchor</b> - " + ((Control)Component).ID + "</div>";
        }

        #endregion
    }


    /// <summary>
    /// 编辑选择的JavaScript事件
    /// </summary>
    public class JavaScriptEventEditor : UITypeEditor
    {
        #region 变量

        private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = null;
        private System.Windows.Forms.ListBox lb;

        #endregion

        #region 方法

        /// <summary>
        /// 改写基类的选择编辑器行为方法
        /// 在自定义控件里显示编辑的值
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        /// <param name="context">编辑控件的上下文</param>
        /// <param name="provider">一个有效的服务提供</param>
        /// <param name="value">当前要编辑的对象的值</param>
        /// <returns>对象的新值</returns>
        public override object EditValue(ITypeDescriptorContext context,
          IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    lb = new System.Windows.Forms.ListBox();
                    lb.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);
                    lb.Items.Add("onclick");
                    lb.Items.Add("ondblclick");
                    lb.Items.Add("onmouseover");
                    lb.Items.Add("onfocus");
                    lb.Items.Add("oncontextmenu");
                    edSvc.DropDownControl(lb);
                    if (lb.SelectedIndex == -1) return value;
                    return lb.SelectedItem;
                }
            }

            return value;
        }


        /// <summary>
        /// 选择编辑器类型
        /// </summary>
        /// <param name="context">编辑器的上下文</param>
        /// <returns>返回 <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }


        /// <summary>
        /// 当用户选择一个值后，关闭此下拉控件
        /// </summary>
        private void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edSvc != null)
            {
                edSvc.CloseDropDown();
            }
        }

        #endregion
    }


    /// <summary>
    /// ASP.NET页面选择的控件的编辑器
    /// Editor for selecting controls from Asp.Net page
    /// </summary>
    public abstract class ControlsEditor : UITypeEditor
    {
        #region 变量

        private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = null;
        private System.Windows.Forms.ListBox lb;
        private Type typeShow;

        #endregion

        #region 构造函数


        /// <summary>
        /// 显示指定的类型
        /// </summary>
        /// <param name="show">类型</param>
        public ControlsEditor(Type show)
        {
            typeShow = show;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 改写基类的选择编辑器行为方法
        /// 在自定义控件里显示编辑的值
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        /// <param name="context">编辑控件的上下文</param>
        /// <param name="provider">一个有效的服务提供</param>
        /// <param name="value">当前要编辑的对象的值</param>
        /// <returns>对象的新值</returns>
        public override object EditValue(ITypeDescriptorContext context,
          IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)
                  provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    lb = new System.Windows.Forms.ListBox();
                    lb.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);
                    foreach (Control ctrl in ((Control)context.Instance).Page.Controls)
                    {
                        if (ctrl.GetType().IsSubclassOf(typeShow) ||
                          ctrl.GetType().FullName == typeShow.FullName) lb.Items.Add(ctrl.ID);
                    }
                    edSvc.DropDownControl(lb);
                    if (lb.SelectedIndex == -1) return value;
                    return lb.SelectedItem;
                }
            }

            return value;
        }


        ///// <summary>
        /// 选择编辑器类型
        /// </summary>
        /// <param name="context">编辑器的上下文</param>
        /// <returns>返回 <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }


        /// <summary>
        /// 当用户选择一个值后，关闭此下拉控件
        /// </summary>
        private void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edSvc != null)
            {
                edSvc.CloseDropDown();
            }
        }

        #endregion
    }


    /// <summary>
    /// 所有ASP.NET的编辑器
    /// </summary>
    public class AllControlsEditor : ControlsEditor
    {
        #region Members

        /// <summary>
        /// 调用基类的构造函数
        /// </summary>
        public AllControlsEditor() : base(typeof(Control)) { }

        #endregion
    }


    /// <summary>
    /// 弹出窗口控件的编辑器
    /// </summary>
    public class PopupControlsEditor : ControlsEditor
    {
        #region Members

        /// <summary>
        /// 调用基类的构造函数
        /// </summary>
        public PopupControlsEditor() : base(typeof(PopupWin)) { }

        #endregion
    }
}

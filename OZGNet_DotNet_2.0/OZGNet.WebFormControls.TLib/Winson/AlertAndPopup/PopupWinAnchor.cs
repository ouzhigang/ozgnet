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
    /// �������ڹ��ӿؼ�
    /// </summary>
    [DefaultProperty("PopupToShow"), ToolboxData("<{0}:PopupWinAnchor runat=server></{0}:PopupWinAnchor>")]
    [Designer(typeof(AnchorDesigner))]
    public class PopupWinAnchor : System.Web.UI.WebControls.WebControl
    {
        #region ����

        string controlId, controlLink, jsEvent;
        string snMsg, snText, snTitle;
        bool bChangeText = false;

        #endregion

        #region ���캯��

        /// <summary>
        /// �����ؼ�
        /// </summary>
        public PopupWinAnchor()
        {
            jsEvent = "onclick";
            bChangeText = false;
        }

        #endregion

        #region ����

        /// <summary>
        /// �Ƿ�ѵ�����������ı��滻Ϊ�µ��ı�
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue(false)]
        [Description("�Ƿ�ѵ�����������ı��滻Ϊ�µ��ı�")]
        public bool ChangeTexts
        {
            get { return bChangeText; }
            set { bChangeText = value; }
        }


        /// <summary>
        /// �µ���Ϣ�ı�
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue("")]
        [Description("�µ���Ϣ�ı�")]
        public string NewMessage
        {
            get { return snMsg; }
            set { snMsg = value; }
        }


        /// <summary>
        /// �µĵ������ڱ���
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue("")]
        [Description("�µĵ������ڱ���")]
        public string NewTitle
        {
            get { return snTitle; }
            set { snTitle = value; }
        }


        /// <summary>
        /// ���´�������ʾ�����ı�
        /// </summary>
        [Bindable(true), Category("PopupWin"), DefaultValue("")]
        [Description("���´�������ʾ�����ı�")]
        public string NewText
        {
            get { return snText; }
            set { snText = value; }
        }


        /// <summary>
        /// JavaScript�¼����
        /// </summary>
        [Bindable(true), Category("Anchor"), DefaultValue("onclick")]
        [Editor(typeof(JavaScriptEventEditor), typeof(UITypeEditor))]
        [Description("JavaScript�¼����")]
        public string HandledEvent
        {
            get { return jsEvent; }
            set { jsEvent = value; }
        }


        /// <summary>
        /// �¼�����ʱ��ʾ�������ڿؼ�
        /// </summary>
        [Bindable(true), Category("Anchor"), DefaultValue("")]
        [Editor(typeof(PopupControlsEditor), typeof(UITypeEditor))]
        [Description("�¼�����ʱ��ʾ�������ڿؼ�")]
        public string PopupToShow
        {
            get { return controlId; }
            set { controlId = value; }
        }


        /// <summary>
        /// ��������������ʾ���¼�
        /// </summary>
        [Bindable(true), Category("Anchor"), DefaultValue("")]
        [Editor(typeof(AllControlsEditor), typeof(UITypeEditor))]
        [Description("��������������ʾ���¼�")]
        public string LinkedControl
        {
            get { return controlLink; }
            set { controlLink = value; }
        }

        #endregion

        #region ����

        /// <summary> 
        /// ��ʾ�������ڿؼ�ʱ���ֽű�
        /// </summary>
        /// <param name="output">HtmlTextWriter����</param>
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
    /// ��ʾ�������ڿؼ���
    /// </summary>
    public class AnchorDesigner : ControlDesigner
    {
        #region Overriden methods

        /// <summary>
        /// ����HTML����
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
    /// �༭ѡ���JavaScript�¼�
    /// </summary>
    public class JavaScriptEventEditor : UITypeEditor
    {
        #region ����

        private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = null;
        private System.Windows.Forms.ListBox lb;

        #endregion

        #region ����

        /// <summary>
        /// ��д�����ѡ��༭����Ϊ����
        /// ���Զ���ؼ�����ʾ�༭��ֵ
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        /// <param name="context">�༭�ؼ���������</param>
        /// <param name="provider">һ����Ч�ķ����ṩ</param>
        /// <param name="value">��ǰҪ�༭�Ķ����ֵ</param>
        /// <returns>�������ֵ</returns>
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
        /// ѡ��༭������
        /// </summary>
        /// <param name="context">�༭����������</param>
        /// <returns>���� <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }


        /// <summary>
        /// ���û�ѡ��һ��ֵ�󣬹رմ������ؼ�
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
    /// ASP.NETҳ��ѡ��Ŀؼ��ı༭��
    /// Editor for selecting controls from Asp.Net page
    /// </summary>
    public abstract class ControlsEditor : UITypeEditor
    {
        #region ����

        private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = null;
        private System.Windows.Forms.ListBox lb;
        private Type typeShow;

        #endregion

        #region ���캯��


        /// <summary>
        /// ��ʾָ��������
        /// </summary>
        /// <param name="show">����</param>
        public ControlsEditor(Type show)
        {
            typeShow = show;
        }

        #endregion

        #region ����

        /// <summary>
        /// ��д�����ѡ��༭����Ϊ����
        /// ���Զ���ؼ�����ʾ�༭��ֵ
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        /// <param name="context">�༭�ؼ���������</param>
        /// <param name="provider">һ����Ч�ķ����ṩ</param>
        /// <param name="value">��ǰҪ�༭�Ķ����ֵ</param>
        /// <returns>�������ֵ</returns>
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
        /// ѡ��༭������
        /// </summary>
        /// <param name="context">�༭����������</param>
        /// <returns>���� <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }


        /// <summary>
        /// ���û�ѡ��һ��ֵ�󣬹رմ������ؼ�
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
    /// ����ASP.NET�ı༭��
    /// </summary>
    public class AllControlsEditor : ControlsEditor
    {
        #region Members

        /// <summary>
        /// ���û���Ĺ��캯��
        /// </summary>
        public AllControlsEditor() : base(typeof(Control)) { }

        #endregion
    }


    /// <summary>
    /// �������ڿؼ��ı༭��
    /// </summary>
    public class PopupControlsEditor : ControlsEditor
    {
        #region Members

        /// <summary>
        /// ���û���Ĺ��캯��
        /// </summary>
        public PopupControlsEditor() : base(typeof(PopupWin)) { }

        #endregion
    }
}

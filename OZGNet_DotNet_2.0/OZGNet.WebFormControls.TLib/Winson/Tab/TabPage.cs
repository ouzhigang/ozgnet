using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OZGNet.WebFormControls.ThirdPart.Winson.Tab
{    
    /// <summary>
    /// ÊôÐÔÒ³¿Ø¼þ
    /// </summary>
    [ToolboxItem(false), PersistChildren(true), Designer(typeof(TabPageDesigner)), ParseChildren(false)]
    public class TabPage : WebControl, INamingContainer
    {
        public TabPage()
        {
            this._Selected = false;
            this._Caption = string.Empty;
            this._ClientID = string.Empty;
            this._ActionLink = string.Empty;
        }

        public object GetOwner()
        {
            return this._Owner;
        }

        protected override void Render(HtmlTextWriter pOutPut)
        {
            if ((this._Owner == null) || (this._Owner.GetType().ToString() != "Winson.Tab.TabControl"))
            {
                throw new ArgumentException("winson.TabPage ´íÎó");
            }
            if (this.Selected)
            {
                pOutPut.Write(string.Concat(new object[] { "<DIV id=\"", this.UniqueID, "\" class=\"tab-page\" style=\"Height:", this._Owner.Height, ";Width:100%;DISPLAY: block\">" }));
            }
            else
            {
                pOutPut.Write(string.Concat(new object[] { "<DIV id=\"", this.UniqueID, "\" class=\"tab-page\" style=\"Height:", this._Owner.Height, ";Width:100%;DISPLAY: none\">" }));
            }
            this.RenderChildren(pOutPut);
            pOutPut.Write("</DIV>");
        }

        protected internal void RenderTitle(HtmlTextWriter pOutPut)
        {
        }

        internal void SetOwner(TabControl pTabControl)
        {
            this._Owner = pTabControl;
        }


        [NotifyParentProperty(true), Browsable(true), Description("")]
        private string ActionLink
        {
            get
            {
                return this._ActionLink;
            }
            set
            {
                this._ActionLink = value;
            }
        }

        [NotifyParentProperty(true), Description(""), Browsable(true)]
        public string Caption
        {
            get
            {
                return this._Caption;
            }
            set
            {
                this._Caption = value;
            }
        }

        internal bool Selected
        {
            get
            {
                return this._Selected;
            }
            set
            {
                this._Selected = value;
            }
        }


        private string _ActionLink;
        private string _Caption;
        private string _ClientID;
        private TabControl _Owner;
        private bool _Selected;
    }
}


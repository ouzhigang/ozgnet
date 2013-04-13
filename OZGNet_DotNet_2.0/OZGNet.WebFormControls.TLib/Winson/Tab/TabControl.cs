using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OZGNet.WebFormControls.ThirdPart.Winson.Tab
{    
    /// <summary>
    /// 属性页控件
    /// </summary>
    [ParseChildren(true, "Items"), Description("WINSON WebControl TabControl"), ToolboxData("<{0}:TabControl runat=server></{0}:TabControl>"), DefaultEvent("SelectedIndexChanged"), DesignTimeVisible(true), PersistChildren(false), Designer(typeof(TabControlDesigner))]
    public class TabControl : WebControl, IPostBackDataHandler, IPostBackEventHandler,INamingContainer
    {
        public event EventHandler SelectedIndexChanged
        {
            add
            {
                base.Events.AddHandler(TabControl.SelectedIndexChangedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(TabControl.SelectedIndexChangedEvent, value);
            }
        }

        static TabControl()
        {
            TabControl.SelectedIndexChangedEvent = new object();
        }

        public TabControl()
        {
            this.SelectedTab = new HtmlInputHidden();
            this._SelectedIndex = -1;
            this.SelectedTab.Value = string.Empty;
            this._Width = Unit.Parse("350");
            this._Height = Unit.Parse("150");
            this._Items = new TabPageCollection(this);
            this._SelectionMode = SelectionModeEnum.Client;
		    this.Height = (Unit) 100;
            this.Width = (Unit) 100;
			this._HeightUnitMode=HeightUnitEnum.percent;
			this._WidthUnitMode=WidthUnitEnum.percent;
        
        }

        protected override void AddParsedSubObject(object pObj)
        {
            if (pObj is TabPage)
            {
                this.Items.Add((TabPage) pObj);
            }
        }

        protected override void CreateChildControls()
        {
            this.CreateControlCollection();
            this.SelectedTab.ID = this.UniqueID;
            for (int num1 = 0; num1 < this.Items.Count; num1++)
            {
                this.Controls.Add(this.Items[num1]);
            }
            base.ChildControlsCreated = true;
			//base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            int num1 = this.SelectedIndex;
            if (num1 != -1)
            {
                this.Items[num1].Selected = true;
                this.SelectedTab.Value = this.Items[num1].UniqueID;
            }
            else
            {
                this.SelectedTab.Value = string.Empty;
            }

			if(!Page.IsClientScriptBlockRegistered("TabWindow"))
			{
				Page.RegisterClientScriptBlock("TabWindow",string.Format("<SCRIPT language='javascript' src='{0}'></SCRIPT>", TabScriptPath));
			}

			base.OnPreRender(args);
        }

        protected void OnSelectedIndexChanged(EventArgs e)
        {
            if (base.Events != null)
            {
                EventHandler handler1 = (EventHandler) base.Events[TabControl.SelectedIndexChangedEvent];
                if (handler1 != null)
                {
                    handler1(this, e);
                }
            }
        }

        protected override void Render(HtmlTextWriter pOutPut)
        {
		    pOutPut.Write("<Div Class=\"TabControl tab-pane\" ID=\"" + this.UniqueID + "_Tab\" Style=\"" + WinsonWebControl.GetControlStyle(this.Width, this.WidthUnitMode, base.Style) + "\">");
            this.SelectedTab.RenderControl(pOutPut);
            pOutPut.Write("<DIV Class=\"tab-row\">");
            this.RenderTabButton(pOutPut);
            pOutPut.Write("</Div><Div>");
            this.RenderTabContent(pOutPut);
			pOutPut.Write("</Div></Div>");
	    }

        private void RenderTabButton(HtmlTextWriter pOutPut)
        {
            if (this.SelectionMode == SelectionModeEnum.Server)
            {
                for (int num1 = 0; num1 < this.Items.Count; num1++)
                {
                    if (this.Items[num1].Selected)
                    {
                        pOutPut.Write("<H2 class=\"tab hover selected\" onMouseOver=\"Tab_OnMouseOver(this)\" onMouseOut=\"Tab_OnMouseOut(this)\">" + this.Items[num1].Caption + "</H2>");
                    }
                    else
                    {
                        pOutPut.Write("<H2 class=\"tab\" onMouseOver=\"Tab_OnMouseOver(this)\" onMouseOut=\"Tab_OnMouseOut(this)\" onClick=\"Tab_OnSelectServerClick(this,'" + this.Items[num1].UniqueID + "');" + this.Page.GetPostBackEventReference(this, "") + "\">" + this.Items[num1].Caption + "</H2>");
                    }
                }
            }
            else
            {
                for (int num2 = 0; num2 < this.Items.Count; num2++)
                {
                    if (this.Items[num2].Selected)
                    {
                        pOutPut.Write("<H2 id=\"" + this.Items[num2].UniqueID + "_H2\" class=\"tab hover selected\" onMouseOver=\"Tab_OnMouseOver(this)\" onMouseOut=\"Tab_OnMouseOut(this)\" onClick=\"Tab_OnSelectClientClick(this,'" + this.Items[num2].UniqueID + "');\">" + this.Items[num2].Caption + "</H2>");
                    }
                    else
                    {
                        pOutPut.Write("<H2 id=\"" + this.Items[num2].UniqueID + "_H2\" class=\"tab\" onMouseOver=\"Tab_OnMouseOver(this)\" onMouseOut=\"Tab_OnMouseOut(this)\" onClick=\"Tab_OnSelectClientClick(this,'" + this.Items[num2].UniqueID + "');\">" + this.Items[num2].Caption + "</H2>");
                    }
                }
            }
        }

        private void RenderTabContent(HtmlTextWriter pOutPut)
        {
            for (int num1 = 0; num1 < this.Items.Count; num1++)
            {
                this.Items[num1].RenderControl(pOutPut);
            }
        }

        bool IPostBackDataHandler.LoadPostData(string ControlDataKey, NameValueCollection PostBackDataCollection)
        {
			
            string text1 = PostBackDataCollection[ControlDataKey];
            if ((text1 != null) && (text1 != this.SelectedTabID))
            {
                this.SelectedTabID = text1;
                return true;
            }
            this.SelectedTabID = text1;
			
            return false;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.OnSelectedIndexChanged(EventArgs.Empty);
        }

        void IPostBackEventHandler.RaisePostBackEvent(string pEventArgument)
        {
        }


        [MergableProperty(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public TabPageCollection Items
        {
            get
            {
                return this._Items;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                if (this.Items.Count <= 0)
                {
                    return (this._SelectedIndex = -1);
                }
                if (this._SelectedIndex == -1)
                {
                    for (int num1 = 0; num1 < this.Items.Count; num1++)
                    {
                        if (this.Items[num1].Visible && (this.Items[num1].UniqueID == this.SelectedTabID))
                        {
                            return (this._SelectedIndex = num1);
                        }
                    }
                    return (this._SelectedIndex = 0);
                }
                if (this._SelectedIndex >= this.Items.Count)
                {
                    return (this._SelectedIndex = 0);
                }
                return this._SelectedIndex;
            }
            set
            {
                if ((value < -1) || (value >= this.Items.Count))
                {
                    throw new ArgumentOutOfRangeException("SelectedIndex must be Less than " + this.Items.Count.ToString());
                }
                this._SelectedIndex = value;
            }
        }

        protected string SelectedTabID
        {
            get
            {
                if (this.ViewState["SelectedTabID"] != null)
                {
                    return (string) this.ViewState["SelectedTabID"];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState["SelectedTabID"] = value;
            }
        }

        public SelectionModeEnum SelectionMode
        {
            get
            {
                return this._SelectionMode;
            }
            set
            {
                this._SelectionMode = value;
            }
        }

	
		public HeightUnitEnum HeightUnitMode
		{
			get
			{
				return this._HeightUnitMode;
			}
			set
			{
				this._HeightUnitMode = value;
			}
		}

		public WidthUnitEnum WidthUnitMode
		{
			get
			{
				return this._WidthUnitMode;
				}
			set
			{
				this._WidthUnitMode = value;
			}
		}


		private HeightUnitEnum _HeightUnitMode;
		private WidthUnitEnum _WidthUnitMode;
        private Unit _Height;
        private TabPageCollection _Items;
        private int _SelectedIndex;
        private SelectionModeEnum _SelectionMode;
        private Unit _Width;
        private static readonly object SelectedIndexChangedEvent;
        private HtmlInputHidden SelectedTab;

		public enum HeightUnitEnum
		{
			percent,
		    px
		}
		
		public enum WidthUnitEnum
		{
			percent,
			px
		}


        public enum SelectionModeEnum
        {
            Client,
            Server
        }


		#region Property ScriptPath

		/// <summary>
		/// Javascript脚本文件所在目录。
		/// </summary>
		[Description("Javascript脚本文件所在目录。"),DefaultValue("./")]
		public string TabScriptPath 
		{
			get
			{
				object obj = ViewState["TabScriptPath"];
				return obj == null ? "../js/tabstrip.js" :(string) obj;
			}
			set
			{
				ViewState["TabScriptPath"] = value;
			}
		}

		#endregion
	
    }
}


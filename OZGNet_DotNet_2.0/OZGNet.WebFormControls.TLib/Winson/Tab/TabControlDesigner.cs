using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;

namespace OZGNet.WebFormControls.ThirdPart.Winson.Tab
{    
    /// <summary>
    /// ÊôÐÔÒ³¿Ø¼þ
    /// </summary>
    public class TabControlDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            StringWriter writer1 = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer1);
            using (HtmlTable table1 = new HtmlTable())
            {
                HtmlTableCell cell1;
                HtmlTableCell cell2;
                table1.Border = 0;
                table1.CellPadding = 5;
                table1.CellSpacing = 2;
                table1.Height = this.DesignTabControl.Height.Value.ToString();
                table1.Width = this.DesignTabControl.Width.Value.ToString();
                if (this.DesignTabControl.Items.Count > 0)
                {
                    using (HtmlTableRow row1 = new HtmlTableRow())
                    {
                        row1.Height = "25";
                        for (int num1 = 0; num1 < this.DesignTabControl.Items.Count; num1++)
                        {
                            using (cell2 = (cell1 = new HtmlTableCell()))
                            {
                                if (this.DesignTabControl.Items[num1].Caption != string.Empty)
                                {
                                    cell1.InnerHtml = "<Font Color=\"#FFFFFF\">" + this.DesignTabControl.Items[num1].Caption + "</Font>";
                                }
                                else
                                {
                                    cell1.InnerHtml = "<Font Color=\"#FFFFFF\">NoName</Font>";
                                }
                                cell1.Align = "Center";
                                cell1.BgColor = "#000000";
                                row1.Cells.Add(cell1);
                            }
                        }
                        table1.Rows.Add(row1);
                    }
                    using (HtmlTableRow row2 = new HtmlTableRow())
                    {
                        using (cell2 = (cell1 = new HtmlTableCell()))
                        {
                            cell1.ColSpan = this.DesignTabControl.Items.Count + 1;
                            cell1.BgColor = "#FFFFFF";
                            cell1.Align = "Center";
                            cell1.InnerHtml = "<Font Color=\"#000000\"><B>WINSON.WebControl.TabControl</B><Font>";
                            row2.Cells.Add(cell1);
                            table1.Rows.Add(row2);
                        }
                        goto Label_025F;
                    }
                }
                using (HtmlTableRow row3 = new HtmlTableRow())
                {
                    row3.Height = "100%";
                    using (cell2 = (cell1 = new HtmlTableCell()))
                    {
                        cell1.Align = "Center";
                        cell1.InnerHtml = "<Font Color=\"#000000\"><B>Not Found TabPage</B><Font>";
                        cell1.BgColor = "#FFFFFF";
                        row3.Cells.Add(cell1);
                    }
                    table1.Rows.Add(row3);
                }
            Label_025F:
                table1.RenderControl(writer2);
            }
            return writer1.ToString();
        }


        public override bool AllowResize
        {
            get
            {
                return true;
            }
        }

        public TabControl DesignTabControl
        {
            get
            {
                return (TabControl) base.Component;
            }
        }

    }
}


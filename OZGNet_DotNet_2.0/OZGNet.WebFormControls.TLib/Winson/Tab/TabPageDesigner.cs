using System.Web.UI.Design;

namespace OZGNet.WebFormControls.ThirdPart.Winson.Tab
{
    /// <summary>
    /// ÊôÐÔÒ³¿Ø¼þ
    /// </summary>
    public class TabPageDesigner : ReadWriteControlDesigner
    {
        public TabPage DesignTabPage
        {
            get
            {
                return (TabPage) base.Component;
            }
        }

    }
}


using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OZGNet.WebFormControls.ThirdPart.Winson.Tab
{
  /// <summary>
  /// ÊôÐÔÒ³¿Ø¼þ
  /// </summary>
  public class WinsonWebControl
  {

    internal static string GetControlStyle(Unit Width, TabControl.WidthUnitEnum wue, CssStyleCollection Style)
    {
	  string str = "";
      if(wue.ToString()=="px")
    	 str = String.Concat("width:"+Width.Value+"px;");
	  else
		 str = String.Concat("width:100%;"); 

      IEnumerator iEnumerator = Style.Keys.GetEnumerator();
      try
      {
        while (iEnumerator.MoveNext())
        {
          object local1 = iEnumerator.Current;
          object local2 = str;
          str = String.Concat(new object[]{local2, local1, ":", Style[(String)local1], ";"});
        }
      }
      finally
      {
        IDisposable iDisposable = iEnumerator as IDisposable;
        if (iDisposable != null)
        {
          iDisposable.Dispose();
        }
      }
      return str;
    }

    internal static string GetControlStyle(CssStyleCollection Style)
    {
      string str = String.Empty;
      IEnumerator iEnumerator = Style.Keys.GetEnumerator();
      try
      {
        while (iEnumerator.MoveNext())
        {
          object local1 = iEnumerator.Current;
          object local2 = str;
          str = String.Concat(new object[]{local2, local1, ":", Style[(String)local1], ";"});
        }
      }
      finally
      {
        IDisposable iDisposable = iEnumerator as IDisposable;
        if (iDisposable != null)
        {
          iDisposable.Dispose();
        }
      }
      return str;
    }

    internal static string ConvertColorToString(Color pColor)
    {
      string str = "#";
      byte b = pColor.R;
      str = String.Concat(str, b.ToString("x2"));
      b = pColor.G;
      str = String.Concat(str, b.ToString("x2"));
      b = pColor.B;
      str = String.Concat(str, b.ToString("x2"));
      return str;
    }
  }

}

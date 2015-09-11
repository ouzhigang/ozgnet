using System;
using Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Graph = Microsoft.Office.Interop.Graph;
using System.Runtime.InteropServices;
using System.Drawing;

namespace OZGNet.Office
{
    /// <summary>
    /// 生成PowerPoint
    /// </summary>
    public class PowerPointHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public PowerPointHelper()
        {

        }

        /// <summary>
        /// 插入文本
        /// </summary>
        /// <param name="slide"></param>
        /// <param name="text">文本</param>
        /// <param name="left">左边距离</param>
        /// <param name="top">上边距离</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void InsertText(PowerPoint._Slide slide, string text, float left, float top, float width, float height)
        {
            PowerPoint.Shape shape = slide.Shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, left, top, width, height);
            PowerPoint.TextRange textRng = shape.TextFrame.TextRange;
            textRng.Text = text;
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="slide"></param>
        /// <param name="fileName">图片路径</param>
        /// <param name="left">左边距离</param>
        /// <param name="top">上边距离</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void InsertPicture(PowerPoint._Slide slide, string fileName, float left, float top, float width, float height)
        {
            slide.Shapes.AddPicture(fileName, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, left, top, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slide"></param>
        /// <param name="left">左边距离</param>
        /// <param name="top">上边距离</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void SetCanvas(PowerPoint._Slide slide, float left, float top, float width, float height)
        {
            slide.Shapes.AddCanvas(left, top, width, height);
        }

        /// <summary>
        /// 插入媒体
        /// </summary>
        /// <param name="slide"></param>
        /// <param name="fileName">媒体路径</param>
        /// <param name="left">左边距离</param>
        /// <param name="top">上边距离</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void InsertMedia(PowerPoint._Slide slide, string fileName, float left, float top, float width, float height)
        {
            PowerPoint.Shape shape = slide.Shapes.AddMediaObject(fileName, left, top, width, height);
            PowerPoint.AnimationSettings mediaSettings = shape.AnimationSettings;
            PowerPoint.PlaySettings oPlaySettings = mediaSettings.PlaySettings;
            oPlaySettings.PlayOnEntry = Microsoft.Office.Core.MsoTriState.msoTrue;
        }

        //public static void InsertFlash(PowerPoint._Slide slide, string fileName, float left, float top, float width, float height)
        //{
        //    ShockwaveFlashObjects.ShockwaveFlashClass flash = (ShockwaveFlashObjects.ShockwaveFlashClass)slide.Shapes.AddOLEObject(left, top, width, height, "ShockwaveFlash.ShockwaveFlash.10", "", MsoTriState.msoFalse, "", 0, "", MsoTriState.msoFalse).OLEFormat.Object;
        //    flash.Movie = fileName;
        //}

        /// <summary>
        /// 测试！
        /// </summary>
        protected static void Test()
        {
            PowerPoint.Application application;
            PowerPoint.Presentations presentations;
            PowerPoint._Presentation presentation;
            PowerPoint.Slides slides;
            PowerPoint._Slide slide;
            PowerPoint.SlideRange objSldRng;
            PowerPoint.SlideShowSettings objSSS;
            PowerPoint.SlideShowTransition objSST;
            PowerPoint.SlideShowWindows objSSWs;

            //create application
            application = new PowerPoint.ApplicationClass();
            application.Visible = MsoTriState.msoTrue;
            presentations = application.Presentations;
            presentation = presentations.Add(Microsoft.Office.Core.MsoTriState.msoTrue);
            //add first silde
            slides = presentation.Slides;

            //第一页
            slide = slides.Add(1, Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutTitleOnly);
            InsertText(slide, "这是第一页", 0, 0, 200, 200);

            //第二页
            slide = slides.Add(2, Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutTitleOnly);
            InsertText(slide, "这是第二页", 0, 0, 200, 200);


            //InsertPicture(slide,@"C:\pic.jpg",0,200,300,200);
            //InsertFlash(slide, @"C:\xiyou.swf", 0, 0, 200, 200);
            //InsertMedia(slide,@"c:\bjhyn.wmv",200,200,200,200);


            int[] SlideIdx = new int[1];
            for (int i = 0; i < 1; i++) SlideIdx[i] = i + 1;
            objSldRng = slides.Range(SlideIdx);
            objSST = objSldRng.SlideShowTransition;
            objSST.AdvanceOnTime = MsoTriState.msoTrue;
            objSST.AdvanceTime = 3;
            objSST.EntryEffect = PowerPoint.PpEntryEffect.ppEffectBoxOut;

            //Prevent Office Assistant from displaying alert messages:
            bool bAssistantOn = application.Assistant.On;
            application.Assistant.On = false;

            //Run the Slide show.
            objSSS = presentation.SlideShowSettings;
            objSSS.StartingSlide = 1;
            objSSS.EndingSlide = 1;
            objSSS.Run();


            //Wait for the slide show to end.
            objSSWs = application.SlideShowWindows;
            while (objSSWs.Count >= 1) System.Threading.Thread.Sleep(1000);

            //Reenable Office Assisant, if it was on:
            if (bAssistantOn)
            {
                application.Assistant.On = true;
                application.Assistant.Visible = false;
            }



            //Close the presentation without saving changes and quit PowerPoint.
            presentation.SaveAs(@"c:\fang.ppt", PowerPoint.PpSaveAsFileType.ppSaveAsDefault, Microsoft.Office.Core.MsoTriState.msoTrue);
            presentation.Close();
            application.Quit();
            GC.Collect();
        }

    }
}

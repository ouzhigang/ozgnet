using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace OZGNet.GDI
{
    /// <summary>
    /// 生成4位验证码图片(jpeg,75*25)
    /// 这个类用在ashx上面要实现System.Web.SessionState.IRequiresSessionState
    /// </summary>
    public class VerificationCode
    {
        /// <summary>
        /// 获取验证码的Session名称
        /// </summary>
        string _CheckCodeName = "CheckCode";

        string _CheckCodeValue = null;

        public string CheckCodeName
        {
            get 
            {
                return this._CheckCodeName;
            }
            set
            {
                this._CheckCodeName = value;
            }
        }

        public string CheckCodeValue
        {
            get
            {
                return this._CheckCodeValue;
            }
            set
            {
                this._CheckCodeValue = value;
            }
        }

        /// <summary>
        /// 生成验证码,直接在Web返回图片，格式为JPG
        /// </summary>
        public void CreateForWeb()
        {
            CreateForWeb(OZGNet.Net.MimeString._JPEG, ImageFormat.Jpeg, OZGNet.Options.RandString.All, HttpContext.Current);
        }        

        /// <summary>
        /// 生成验证码,直接在Web返回图片
        /// </summary>
        /// <param name="mimeString">返回的MIME类型，对应OZGNet.Net.MimeString类的图片MIME</param>
        /// <param name="imgFormat">图片输出格式，最好和MIME类型相对应</param>
        public void CreateForWeb(string mimeString, ImageFormat imgFormat)
        {
            CreateForWeb(mimeString, imgFormat, OZGNet.Options.RandString.All, HttpContext.Current);
        }

        /// <summary>
        /// 生成验证码,直接在Web返回图片
        /// </summary>
        /// <param name="mimeString">返回的MIME类型，对应OZGNet.Net.MimeString类的图片MIME</param>
        /// <param name="imgFormat">图片输出格式，最好和MIME类型相对应</param>
        /// <param name="stringType">输出字符内容的类型，对应OZGNet.Options.RandString</param>
        public void CreateForWeb(string mimeString, ImageFormat imgFormat, OZGNet.Options.RandString stringType)
        {
            CreateForWeb(mimeString, imgFormat, stringType, HttpContext.Current);
        }
        
        /// <summary>
        /// 生成验证码,直接在Web返回图片
        /// </summary>
        /// <param name="mimeString">返回的MIME类型，对应OZGNet.Net.MimeString类的图片MIME</param>        
        /// <param name="imgFormat">图片输出格式，最好和MIME类型相对应</param>
        /// <param name="stringType">输出字符内容的类型，对应OZGNet.Options.RandString</param>
        /// <param name="context">HttpContext实例</param>
        public void CreateForWeb(string mimeString, ImageFormat imgFormat, OZGNet.Options.RandString stringType, HttpContext context)
        {
            context.Response.BufferOutput = true;  //特别注意
            context.Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//特别注意
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意
            context.Response.AppendHeader("Pragma", "No-Cache"); //特别注意

            context.Response.ContentType = mimeString;
            CreateImages(imgFormat, stringType);
        }

        /// <summary>
        /// 生成验证码,返回Bitmap
        /// </summary>
        /// <returns></returns>
        public Bitmap CreateForBitmap()
        {
            return CreateBitmap(OZGNet.Options.RandString.All);
        }

        /// <summary>
        /// 生成验证码,返回Bitmap
        /// </summary>
        /// <param name="stringType">字符类型</param>
        /// <returns></returns>
        public Bitmap CreateForBitmap(OZGNet.Options.RandString stringType)
        {
            return CreateBitmap(stringType);
        }

        /// <summary>
        /// 生成图片(返回Bitmap)
        /// </summary>
        /// <param name="stringType"></param>
        /// <returns></returns>
        protected Bitmap CreateBitmap(OZGNet.Options.RandString stringType)
        {
            string code = this.CheckCodeValue == null ? OZGNet.Utility.GetRandString(stringType, 4) : this.CheckCodeValue;
            HttpContext.Current.Session.Add(CheckCodeName, code);

            Bitmap image = new Bitmap(70, 25);
            Graphics g = Graphics.FromImage(image);

            WebColorConverter ww = new WebColorConverter();
            g.Clear((Color)ww.ConvertFromString("#FAE264"));

            Random random = new Random();
            //画图片的背景噪音线
            for (int i = 0; i < 12; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
            }
            Font font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Italic);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Gray, 1.2f, true);
            g.DrawString(code, font, brush, 0, 0);

            //画图片的前景噪音点
            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.White);
            }

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            MemoryStream ms = new MemoryStream();
            g.Dispose();
            return image;
        }

        /// <summary>
        /// 生成图片
        /// </summary>        
        /// <param name="imgFormat"></param>
        /// <param name="stringType"></param>
        protected void CreateImages(ImageFormat imgFormat, OZGNet.Options.RandString stringType)
        {
            Bitmap image = CreateBitmap(stringType);
            MemoryStream ms = new MemoryStream();
            using (image)
            {
                using (ms)
                {
                    image.Save(ms, imgFormat);
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                }
            }
        } 

    }
}

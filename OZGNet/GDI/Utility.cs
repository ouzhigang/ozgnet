using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace OZGNet.GDI
{
    /// <summary>
    /// GDI工具类
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 打水印图的位置
        /// </summary>
        public enum ShuiYinPicPoint : int
        {
            /// <summary>
            /// 左上
            /// </summary>
            LeftTop = 1,
            /// <summary>
            /// 中上
            /// </summary>
            MiddleTop = 2,
            /// <summary>
            /// 右上
            /// </summary>
            RightTop = 3,
            /// <summary>
            /// 左中
            /// </summary>
            LeftMiddle = 4,
            /// <summary>
            /// 中间
            /// </summary>
            Middle = 5,
            /// <summary>
            /// 右中
            /// </summary>
            RightMiddle = 6,
            /// <summary>
            /// 左下
            /// </summary>
            LeftDown = 7,
            /// <summary>
            /// 中下
            /// </summary>
            MiddleDown = 8,
            /// <summary>
            /// 右下
            /// </summary>
            RightDown = 9
        }

        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="format">生成缩略图的格式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, System.Drawing.Imaging.ImageFormat format)
        {
            Stream s = File.OpenRead(originalImagePath);

            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(s);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, format);
            }
            catch { }

            s.Close();
            s.Dispose();

            g.Dispose();
            bitmap.Dispose();
            originalImage.Dispose();
        } 
        #endregion

        #region 切割图片(GDI版)
        /// <summary>
        /// 切割图片(GDI版)
        /// </summary>
        /// <param name="source_pic">原图路径</param>
        /// <param name="save_pic">输出路径</param>
        /// <param name="layout_left">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_top">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_width">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_height">左上角坐标和剪切的宽和高</param>
        /// <param name="layout_zoom">放大缩小的倍数</param>
        /// <param name="save_width">保存图的宽度</param>
        /// <param name="disp_width">截图前的显示图的宽度</param>
        public static void LayoutForGDI(string source_pic, string save_pic, int layout_left, int layout_top, int layout_width, int layout_height, int layout_zoom, int save_width, int disp_width)
        {
            int x1 = layout_left;
            int y1 = layout_top;
            int x2 = x1 + layout_width;
            int y2 = y1 + layout_height;
            int zoom = layout_zoom;

            Bitmap tmp = (Bitmap)Bitmap.FromFile(source_pic);

            Bitmap source = null;
            if (zoom != 0)
            {
                source = new Bitmap(tmp, tmp.Width * zoom / 100, tmp.Height * zoom / 100);
            }
            else
            {
                source = new Bitmap(tmp, tmp.Width, tmp.Height);
            }

            double szoom = (double)tmp.Width / (double)disp_width;  //计算出截图前显示的尺寸与实际尺寸的比例

            //计算截取原图的4个点
            int Crop_X0 = (int)Math.Round((double)x1 * szoom);
            int Crop_Y0 = (int)Math.Round((double)y1 * szoom);
            int Crop_X1 = (int)Math.Round((double)x2 * szoom);
            int Crop_Y1 = (int)Math.Round((double)y2 * szoom);

            //计算生成图片对象的高度
            int Height = (int)Math.Round((double)((Crop_Y1 - Crop_Y0) * save_width) / (double)(Crop_X1 - Crop_X0));
            Bitmap cropbmp = new Bitmap(save_width, Height);
            using (Graphics g = Graphics.FromImage(cropbmp))
            {
                //第一个Rectangle,是填充整个生成图片
                //第二个Rectangle,是截取原图的位置
                g.DrawImage(source, new Rectangle(0, 0, cropbmp.Width, cropbmp.Height), new Rectangle(Crop_X0, Crop_Y0, Crop_X1 - Crop_X0, Crop_Y1 - Crop_Y0), GraphicsUnit.Pixel);
                cropbmp.Save(save_pic);
            }
            source.Dispose();
            tmp.Dispose();
            cropbmp.Dispose();
        }
        #endregion
        
        #region 输入新的宽度，然后按比例返回图片
        /// <summary>
        /// 输入新的宽度，然后按比例返回图片
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="newWidth">新的宽度</param>
        /// <returns></returns>
        public static Bitmap GetProportionBitmap(string path, int newWidth)
        {
            Bitmap srcBmp = (Bitmap)Bitmap.FromFile(path);
            Bitmap tmp = new Bitmap(newWidth, OZGNet.Utility.GetProportionHeight(srcBmp.Size, newWidth));
            Graphics g = Graphics.FromImage(tmp);
            g.DrawImage(srcBmp, new Rectangle(0, 0, tmp.Width, tmp.Height), new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), GraphicsUnit.Pixel);
            g.Dispose();
            srcBmp.Dispose();
            return tmp;
        } 
        #endregion

        #region 图片旋转
        /// <summary>
        /// 图片旋转
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="angle"></param>
        /// <param name="bkColor"></param>
        /// <returns></returns>
        public static Bitmap BitmapRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            Bitmap tmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            bmp.Dispose();
            tmp.Dispose();

            return dst;
        }
        #endregion

        #region Bitmap重新调整大小
        /// <summary>
        /// Bitmap重新调整大小
        /// </summary>
        /// <param name="source">源图</param>
        /// <param name="width">修改宽度</param>
        /// <param name="height">修改高度</param>
        /// <returns>调整大小后的新图</returns>
        public static Bitmap BitmapResize(Bitmap source, int width, int height)
        {
            Bitmap tmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(tmp);
            using (g)
            {
                g.DrawImage(source, new Rectangle(0, 0, tmp.Width, tmp.Height), new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
            }
            source.Dispose();
            return tmp;
        }
        #endregion

        #region 生成水印(使用文本),然后删除源图
        /// <summary>
        /// 生成水印(使用文本),然后删除源图
        /// </summary>
        /// <param name="Path">源图路径</param>
        /// <param name="Path_sy">生成水印后的图片路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="color">字体颜色</param>
        public static void ShuiYinWord(string Path, string Path_sy, string text, float fontSize, System.Drawing.Color color)
        {
            string addText = text;
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", fontSize);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(color);

            g.DrawString(addText, f, b, 15, 15);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
            File.Delete(Path);
        }
        #endregion

        #region 生成水印(使用文本)
        /// <summary>
        /// 生成水印(使用文本)
        /// </summary>
        /// <param name="Path">源图路径</param>
        /// <param name="Path_sy">生成水印后的图片路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="color">字体颜色</param>
        /// <param name="delete">是否删除源图</param>
        public static void ShuiYinWord(string Path, string Path_sy, string text, float fontSize, System.Drawing.Color color, bool delete)
        {
            string addText = text;
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", fontSize);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(color);

            g.DrawString(addText, f, b, 15, 15);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
            if (delete)
            {
                File.Delete(Path);
            }
        }
        #endregion

        #region 在图片上生成图片水印,然后删除源图
        /// <summary>
        /// 在图片上生成图片水印,然后删除源图
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        /// <param name="point">打水印图的位置</param>
        public static void ShuiYinPic(string Path, string Path_syp, string Path_sypf, ShuiYinPicPoint point)
        {
            ShuiYinPic(Path, Path_syp, Path_sypf, point, true);
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        /// <param name="point">打水印图的位置</param>
        /// <param name="delete">是否删除原图片</param>
        public static void ShuiYinPic(string Path, string Path_syp, string Path_sypf, ShuiYinPicPoint point, bool delete)
        {
            Stream s1 = File.OpenRead(Path);
            Stream s2 = File.OpenRead(Path_sypf);

            System.Drawing.Image image = System.Drawing.Image.FromStream(s1);
            System.Drawing.Image copyImage = System.Drawing.Image.FromStream(s2);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            if (point == ShuiYinPicPoint.LeftTop)
            { 
                //左上
                //g.DrawImage(copyImage, new Point(0, 0));

                g.DrawImage(copyImage, new System.Drawing.Rectangle(0, 0, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.MiddleTop)
            {
                //中上
                //g.DrawImage(copyImage, new Point((image.Width - copyImage.Width) / 2, 0));

                g.DrawImage(copyImage, new System.Drawing.Rectangle((image.Width - copyImage.Width) / 2, 0, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.RightTop)
            {
                //右上
                //g.DrawImage(copyImage, new Point(image.Width - copyImage.Width, 0));

                g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, 0, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.LeftMiddle)
            {
                //左中
                //g.DrawImage(copyImage, new Point(0, (image.Height - copyImage.Height) / 2));

                g.DrawImage(copyImage, new System.Drawing.Rectangle(0, (image.Height - copyImage.Height) / 2, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.Middle)
            {
                //中间
                //g.DrawImage(copyImage, new Point((image.Width - copyImage.Width) / 2, (image.Height - copyImage.Height) / 2));

                g.DrawImage(copyImage, new System.Drawing.Rectangle((image.Width - copyImage.Width) / 2, (image.Height - copyImage.Height) / 2, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.RightMiddle)
            {
                //右中
                //g.DrawImage(copyImage, new Point(image.Width - copyImage.Width, (image.Height - copyImage.Height) / 2));

                g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, (image.Height - copyImage.Height) / 2, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.LeftDown)
            {
                //左下
                //g.DrawImage(copyImage, new Point(0, image.Height - copyImage.Height));

                g.DrawImage(copyImage, new System.Drawing.Rectangle(0, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.MiddleDown)
            {
                //中下
                //g.DrawImage(copyImage, new Point((image.Width - copyImage.Width) / 2, image.Height - copyImage.Height));

                g.DrawImage(copyImage, new System.Drawing.Rectangle((image.Width - copyImage.Width) / 2, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            else if (point == ShuiYinPicPoint.RightDown)
            {
                //右下
                //g.DrawImage(copyImage, new Point(image.Width - copyImage.Width, image.Height - copyImage.Height));

                g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            
            g.Dispose();

            image.Save(Path_syp);

            s1.Close();
            s1.Dispose();
            s2.Close();
            s2.Dispose();
            image.Dispose();
            copyImage.Dispose();
            if (delete)
            {
                File.Delete(Path);
            }
        }
        #endregion


    }
}

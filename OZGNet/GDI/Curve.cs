using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;

namespace OZGNet.GDI
{
    /// <summary>
    /// 生成曲线图
    /// </summary>
    public class Curve
    {
        /// <summary>
        /// 曲线关键点
        /// </summary>
        public Queue KeyDots = new Queue(); //曲线关键点   
        private Graphics objGraphic;//提供绘制的方法   
        /// <summary>
        /// 位图对象 
        /// </summary>
        public Bitmap ObjBitmap;//位图对象   
        private Point CurrentPoint = new Point(0, 0); //当前点   
        private Point LastPoint = new Point(50, 635); //最后的点   
        private int m_Width = 900;// 图像宽度   
        private int m_Height = 650;//图像高度   
        private int m_XGrid = 50;//栅格X长度   
        private int m_YGrid = 50;//栅格Y长度   
        private Color m_BorderColor = Color.Blue; //边框颜色   
        private Color m_BgColor = Color.Black;//背景颜色    
        private Color m_GridColor = Color.Gray;//栅格颜色   

        //每一个栅格的X值个每一个栅格的Y值
        private int m_XGridValue = 1;
        private int m_YGridValue = 1;
        /// <summary>
        /// 获取或设置图像宽度 
        /// </summary>
        public int Width
        {
            set { m_Width = value; }
            get { return m_Width; }
        }
        /// <summary>
        /// 获取或设置图像高度   
        /// </summary>
        public int Height
        {
            set { m_Height = value; }
            get { return m_Height; }
        }
        /// <summary>
        /// 获取或设置边框颜色  
        /// </summary>
        public Color BorderColor
        {
            set { m_BorderColor = value; }
            get { return m_BorderColor; }
        }
        /// <summary>
        /// 获取或设置背景颜色   
        /// </summary>
        public Color BgColor
        {
            set { m_BgColor = value; }
            get { return m_BgColor; }
        }
        /// <summary>
        /// 获取或设置栅格颜色
        /// </summary>
        public Color GridColor
        {
            set { m_GridColor = value; }
            get { return m_GridColor; }
        }
        /// <summary>
        /// 获取或设置栅格X长度   
        /// </summary>
        public int XGrid
        {
            set { m_XGrid = value; }
            get { return m_XGrid; }
        }
        /// <summary>
        /// 获取或设置栅格Y长度   
        /// </summary>
        public int YGrid
        {
            set { m_YGrid = value; }
            get { return m_YGrid; }
        }

        /// <summary>
        /// 每一个栅格的X值
        /// </summary>
        public int XGridValue
        {
            set { m_XGridValue = value; }
            get { return m_XGridValue; }
        }

        /// <summary>
        /// 每一个栅格的Y值
        /// </summary>
        public int YGridValue
        {
            set { m_YGridValue = value; }
            get { return m_YGridValue; }
        }
        /// <summary>
        /// 实例化Curve
        /// </summary>
        public Curve()
        {
            InitializeGraph();
            DrawContent();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitializeGraph()
        {
            //创建指定高度和宽度的位图对象   
            ObjBitmap = new Bitmap(Width, Height);

            //从位图对象中创建Graphics对象   
            objGraphic = Graphics.FromImage(ObjBitmap);

            //创建指定颜色的矩形边框   
            objGraphic.DrawRectangle(new Pen(BorderColor, 1), 0, 0, Width, Height);
            objGraphic.FillRectangle(new SolidBrush(BgColor), 1, 1, Width, Height);

            //画X方向栅格并标注   
            int t = 0;
            for (int i = Height - 15; i >= 0; i -= YGrid)
            {
                objGraphic.DrawLine(new Pen(GridColor, 1), 50, i, Width, i);
                objGraphic.DrawString(System.Convert.ToString(t * m_YGridValue), new Font("宋体", 10), new SolidBrush(Color.White), 0, i);
                t++;
            }

            //画Y方向栅格并标注
            t = 0;
            for (int i = 50; i < Width; i += XGrid)
            {
                objGraphic.DrawLine(new Pen(GridColor, 1), i, 0, i, Height - 15);
                objGraphic.DrawString(System.Convert.ToString(t * m_XGridValue), new Font("宋体", 10), new SolidBrush(Color.White), i, Height - 13);
                t++;
            }

        }
        /// <summary>
        /// 生成内容
        /// </summary>
        public void DrawContent()
        {
            while (KeyDots.Count > 0)
            {
                //设置新点坐标   
                CurrentPoint.X = LastPoint.X + 2;
                CurrentPoint.Y = Height - System.Convert.ToInt16(KeyDots.Dequeue()) - 15;
                objGraphic.DrawLine(new Pen(Color.Red, 2), LastPoint, CurrentPoint);

                //重置最后点坐标   
                LastPoint = CurrentPoint;
                if (LastPoint.X > Width)
                {
                    InitializeGraph();
                    LastPoint.X = 50;
                }
            }
        }




    }

}



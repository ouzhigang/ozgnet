using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OZGNet.GDI
{
    //    float[] month = new float[12];
    //    for (int i = 0; i < 12; i++)
    //    {
    //        month[i] = i + 1;
    //    }

    //    float[] d = new float[12] { 20.5f, 60, 10.8f, 15.6f, 30, 70.9f, 50.3f, 30.7f, 70, 50.4f, 30.8f, 20 };
        
    //    OZGNet.GDI.CurvePaint cp = new OZGNet.GDI.CurvePaint();
                
    //    //������ٸ�
    //    cp.XkeduCount = 12;

    //    //������ٸ�
    //    cp.YkeduCount = 10;

    //    //�������ֵ,Խ��Խ����
    //    cp.XvalueStrMoveleft = 15;
        
    //    //�ݵ�����ֵ,Խ��Խ����
    //    cp.YvalueStrMoveleft = 50;    
      
    //    Bitmap bmp = cp.DrawCurve(month, d, "ĳ����ĳ��Ʒ��������ͼ��", "�·�", "��������");

    /// <summary>
    /// ͼ����
    /// </summary>
    public class CurvePaint
    {
        /// <summary>
        /// ʵ����CurvePaint
        /// </summary>
        public CurvePaint() { }
        //�̶�������
        private int _X_KeduCount=12;
        private int _Y_KeduCount=12;

        //�̶�ֵλ�ö�Ӧ�̶�����������
        private float _X_valueStrMoveleft=5f;
        private float _Y_valueStrMoveleft=45f;

        //��ʽ���̶�ֵ
        private string _X_Format = "#0.0";
        private string _Y_Format = "#0.00";

        //X��̶�ֵ���ַ���
        private bool _X_DirectionVertical = false;



        /// <summary>
        /// 
        /// </summary>
        public int XkeduCount
        {
            get { return _X_KeduCount; }
            set { _X_KeduCount = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int YkeduCount
        {
            get { return _Y_KeduCount; }
            set { _Y_KeduCount = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float XvalueStrMoveleft
        {
            get { return _X_valueStrMoveleft; }
            set { _X_valueStrMoveleft = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float YvalueStrMoveleft
        {
            get { return _Y_valueStrMoveleft; }
            set { _Y_valueStrMoveleft = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool XdirectionVertical
        {
            get { return _X_DirectionVertical; }
            set { _X_DirectionVertical = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Xformat
        {
            get { return _X_Format; }
            set { _X_Format = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Yformat
        {
            get { return _Y_Format; }
            set { _Y_Format = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="smallestValue"></param>
        /// <param name="greatestValue"></param>
        private void setExtremeValues(float[] arr, ref float smallestValue, ref float greatestValue)
        {
            if (arr == null || arr.Length == 0) throw new Exception("���ڻ�����ͼ������Ϊ��");
            smallestValue = arr[0];
            greatestValue = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (smallestValue > arr[i]) smallestValue = arr[i];
                if (greatestValue < arr[i]) greatestValue = arr[i];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keduArr"></param>
        /// <param name="increment"></param>
        private void setKeduStringArray( float[] keduArr, float increment)
        {
            for (int i = 1; i < keduArr.Length; i++)
            {
                keduArr[i] = keduArr[i - 1] + increment;
            }
        }
        /// <summary>
        /// ���ݹ��f(x)=ax+b
        /// </summary>
        /// <param name="x"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        private float Standard(float x, float A, float B,float C,float D)
        {
            return C * ( A * x + B)+ D;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="X_array"></param>
        /// <param name="Y_array"></param>
        /// <param name="chartTitle"></param>
        /// <param name="X_title"></param>
        /// <param name="Y_title"></param>
        /// <returns></returns>
        public Bitmap DrawCurve(float[] X_array, float[] Y_array, string chartTitle, string X_title, string Y_title)
        {
            //��ͼ��ʼ��
            Bitmap bmap = new Bitmap(500, 500);//ͼƬ��С
            Graphics gph = Graphics.FromImage(bmap);
            gph.Clear(Color.White);

            //����ͼ���¡��ҡ��ϵĿ�϶��Ϊ60����
            PointF cpt = new PointF(60f, bmap.Height - 60f);//����ԭ�㣬��������ʼ�㣨60,440��
            PointF X_EndPoint = new PointF(bmap.Width - 60f, cpt.Y);//X���յ�(440,440)
            PointF Y_EndPoint = new PointF(cpt.X, 60f);//Y���յ�(60,60)

            //�����������μ�ͷ
            PointF[] xpt = new PointF[3] { new PointF(X_EndPoint.X + 15, X_EndPoint.Y), new PointF(X_EndPoint.X, X_EndPoint.Y - 4), new PointF(X_EndPoint.X, X_EndPoint.Y + 4) };//x��������
            PointF[] ypt = new PointF[3] { new PointF(Y_EndPoint.X, Y_EndPoint.Y - 15), new PointF(Y_EndPoint.X - 4, Y_EndPoint.Y), new PointF(Y_EndPoint.X + 4, Y_EndPoint.Y) };//y��������           

            //��ͼ�����
            gph.DrawString(chartTitle, new Font("����", 14), Brushes.Black, new PointF(Y_EndPoint.X + 60, Y_EndPoint.Y - 30));//ͼ�����
            //��x�����Ǽ�ͷ������
            gph.DrawLine(Pens.Black, cpt.X, cpt.Y, X_EndPoint.X, X_EndPoint.Y);
            gph.DrawPolygon(Pens.Black, xpt);
            gph.FillPolygon(new SolidBrush(Color.Black), xpt);
            gph.DrawString(X_title, new Font("����", 12), Brushes.Black, new PointF(X_EndPoint.X + 10, X_EndPoint.Y + 10));
            //��y�����Ǽ�ͷ������
            gph.DrawLine(Pens.Black, cpt.X, cpt.Y, Y_EndPoint.X, Y_EndPoint.Y);
            gph.DrawPolygon(Pens.Black, ypt);
            gph.FillPolygon(new SolidBrush(Color.Black), ypt);
            gph.DrawString(Y_title, new Font("����", 12), Brushes.Black, new PointF(0, Y_EndPoint.Y - 30));

            float X_smallestValue = 0f, X_greatestValue = 0f;
            float Y_smallestValue = 0f, Y_greatestValue = 0f;
            //��ȡ�ᡢ������������Сֵ
            setExtremeValues(X_array, ref X_smallestValue, ref X_greatestValue);
            setExtremeValues(Y_array, ref Y_smallestValue, ref Y_greatestValue);

            //�����������ֵ����Сֵ��
            float X_Increment = (X_greatestValue - X_smallestValue);
            float Y_Increment = (Y_greatestValue - Y_smallestValue);

            //ƽ�������������ֵ����Сֵ��/�̶ȼ�������̶ȼ�������̶�������1
            float X_AvgIncrement = X_Increment / (XkeduCount - 1);
            float Y_AvgIncrement = Y_Increment / (YkeduCount - 1);

            float[] X_KeduArr = new float[XkeduCount];//X��̶�ֵ
            float[] Y_KeduArr = new float[YkeduCount];//Y��̶�ֵ
            X_KeduArr[0] = X_smallestValue;
            Y_KeduArr[0] = Y_smallestValue;

            //���̶�ֵ���鸳ֵ
            setKeduStringArray(X_KeduArr, X_AvgIncrement);
            setKeduStringArray(Y_KeduArr, Y_AvgIncrement);

            //�̶�����ʼλ��
            PointF X_KeduStart = new PointF(cpt.X + 30, cpt.Y);             //(90,440) x���һ���̶���
            PointF X_KeduEnd = new PointF(X_EndPoint.X - 10, X_EndPoint.Y); //(430,440) x�����һ���̶���
            PointF Y_KeduStart = new PointF(cpt.X, cpt.Y - 30);             //(60,410) y���һ���̶���
            PointF Y_KeduEnd = new PointF(Y_EndPoint.X, Y_EndPoint.Y + 10); //(60,70) y�����һ���̶���

            //�̶���λ������ƽ������
            float X_KeduIncrement = (X_KeduEnd.X - X_KeduStart.X) / (XkeduCount - 1);
            float Y_KeduIncrement = (Y_KeduStart.Y - Y_KeduEnd.Y) / (YkeduCount - 1);

            //����X��̶�ֵ��ʾ����
            StringFormat X_StringFormat=new StringFormat();
            if (XdirectionVertical)
            {
                X_StringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            }

            //��x��̶��ߡ��̶�ֵ
            for (int i = 1; i <= XkeduCount; i++)
            {
                if (i == 1)
                {
                    gph.DrawString(X_KeduArr[i - 1].ToString(Xformat), new Font("Times New Roman", 11), Brushes.Black, new PointF(X_KeduStart.X - XvalueStrMoveleft, X_KeduStart.Y + 5), X_StringFormat);// new StringFormat(StringFormatFlags.DirectionVertical));//���һ������ʵ���������ţ�Ĭ��Ϊ����
                    gph.DrawLine(Pens.LightGray, X_KeduStart.X, X_KeduStart.Y, X_KeduStart.X, Y_EndPoint.Y);
                }
                else
                {
                    gph.DrawString(X_KeduArr[i - 1].ToString(Xformat), new Font("Times New Roman", 11), Brushes.Black, new PointF(X_KeduStart.X + (i - 1) * X_KeduIncrement - XvalueStrMoveleft, cpt.Y + 5), X_StringFormat);// new StringFormat(StringFormatFlags.DirectionVertical));//���һ������ʵ���������ţ�Ĭ��Ϊ����
                    gph.DrawLine(Pens.LightGray, X_KeduStart.X + (i - 1) * X_KeduIncrement, cpt.Y, X_KeduStart.X + (i - 1) * X_KeduIncrement, Y_EndPoint.Y);
                }
            }
            //��y��̶��ߡ��̶�ֵ
            for (int i = 1; i <= YkeduCount; i++)
            {
                if (i == 1)
                {
                    gph.DrawString(Y_KeduArr[i - 1].ToString(Yformat), new Font("Times New Roman", 11), Brushes.Black, new PointF(Y_KeduStart.X - YvalueStrMoveleft, Y_KeduStart.Y - 6));
                    gph.DrawLine(Pens.LightGray, Y_KeduStart.X, Y_KeduStart.Y, X_EndPoint.X, Y_KeduStart.Y);
                }
                else
                {
                    gph.DrawString(Y_KeduArr[i - 1].ToString(Yformat), new Font("Times New Roman", 11), Brushes.Black, new PointF(Y_KeduStart.X - YvalueStrMoveleft, Y_KeduStart.Y - (i - 1) * Y_KeduIncrement - 6));
                    gph.DrawLine(Pens.LightGray, Y_KeduStart.X, Y_KeduStart.Y - (i - 1) * Y_KeduIncrement, X_EndPoint.X, Y_KeduStart.Y - (i - 1) * Y_KeduIncrement);
                }
            }

            //(90,440) x���һ���̶�����㣬(430,440) x�����һ���̶�����㣬(60,410) y���һ���̶�����㣬 (60,70) y�����һ���̶������
            //Ϊ�˷��㻭ͼ����ԭ��ά���ݹ�񻯵��̶��Ļ�ͼ����(90-430,70��410),��񻯺���F(x) = A * x + B
            //��Ϊ����Y�����귽������������Y�᷽���෴������Ҫ��Y�����ٱ任һ�Σ��任����G(x) = C * x + D
            //������X����任����A,B��Y�����Ӧ�ı任����A,B,C,D
            float X_A = 340.0f / X_Increment;
            float X_B = 90.0f - (340.0f * X_smallestValue) / X_Increment;
            float Y_A = 340.0f / Y_Increment;
            float Y_B = 70.0f - (340.0f * Y_smallestValue) / Y_Increment;
            float Y_C = -1f, Y_D = 480f;

            for (int i = 1; i <= Y_array.Length; i++)
            {
                //����
                gph.DrawEllipse(Pens.Black, Standard(X_array[i - 1], X_A, X_B, 1, 0) - 1.5f, Standard(Y_array[i - 1], Y_A, Y_B, Y_C, Y_D) - 1.5f, 3, 3);
                gph.FillEllipse(new SolidBrush(Color.Black), Standard(X_array[i - 1], X_A, X_B, 1, 0) - 1.5f, Standard(Y_array[i - 1], Y_A, Y_B, Y_C, Y_D) - 1.5f, 3, 3);
                //����ֵ
                gph.DrawString(Y_array[i - 1].ToString(), new Font("Times New Roman", 11), Brushes.Black, new PointF(Standard(X_array[i - 1], X_A, X_B, 1, 0), Standard(Y_array[i - 1], Y_A, Y_B, Y_C, Y_D)));
                //������
                if (i > 1) gph.DrawLine(Pens.Red, Standard(X_array[i - 2], X_A, X_B, 1, 0), Standard(Y_array[i - 2], Y_A, Y_B, Y_C, Y_D), Standard(X_array[i - 1], X_A, X_B, 1, 0), Standard(Y_array[i - 1], Y_A, Y_B, Y_C, Y_D));
            }
            return bmap;
            //�������ͼƬ
        }



    }
}



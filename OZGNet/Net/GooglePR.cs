using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OZGNet.Net
{
    /// <summary>
    /// ��ȡgoogle��Pane Rank��C#�汾
    /// ʵ����
    /// GooglePR GPR = new GooglePR();
    /// GPR.GetGooglePR("http://www.pconline.com.cn/");
    /// Response.Write(GPR.PR);
    /// </summary>
    public class GooglePR
    {
        /// <summary>
        /// ���峣��0xE6359A60=3862272608
        /// </summary>
        private const long GOOGLE_MAGIC = 0xE6359A60;
        /// <summary>
        /// ˽�б���PR
        /// </summary>
        private string _PR = "0";
        /// <summary>
        /// google pr���ԣ�ֻ��
        /// </summary>
        public string PR
        {
            get { return _PR; }
        }
        /// <summary>
        /// �룬��λ���ǣ������
        /// </summary>
        /// <param name="a">Ҫ����������</param>
        /// <param name="b"></param>
        /// <returns></returns>
        private long ZeroFill(long a, int b)
        {
            //ע�����ƶ�ʱҪ��a��bǿ��ת��Ϊint���ͣ�����������
            long z = 0x80000000;/*=2147483648*/
            if ((z & a) != 0)//�Ⱥ�0x80000000����������ж��Ƿ�Ϊ0
            {
                a = a >> 1;
                a &= ~z;
                a |= 0x40000000;
                a = (int)a >> (int)(b - 1);
            }
            else a = (int)a >> (int)b;
            return a;
        }
        /// <summary>
        /// ���ض�a��b��c������ز����������
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private int[] Mix(long a, long b, long c)
        {
            a -= b; a -= c; a ^= ZeroFill(c, 13);
            b -= c; b -= a; b ^= a << 8;
            c -= a; c -= b; c ^= ZeroFill(b, 13);
            a -= b; a -= c; a ^= ZeroFill(c, 12);
            b -= c; b -= a; b ^= a << 16;
            c -= a; c -= b; c ^= ZeroFill(b, 5);
            a -= b; a -= c; a ^= ZeroFill(c, 3);
            b -= c; b -= a; b ^= a << 10;
            c -= a; c -= b; c ^= ZeroFill(b, 15);
            return new int[] { (int)a, (int)b, (int)c };
        }
        private long GoogleCH(int[] url, int length, long init)
        {
            if (length == -1) length = url.Length;
            if (init == -1) init = GOOGLE_MAGIC;
            long a, b, c = init;
            int[] mix;
            a = b = 0x9E3779B9;
            int len = length, k = 0;
            while (len >= 12)
            {
                a += url[k + 0] + (url[k + 1] << 8) + (url[k + 2] << 16) + (url[k + 3] << 24);
                b += url[k + 4] + (url[k + 5] << 8) + (url[k + 6] << 16) + (url[k + 7] << 24);
                c += url[k + 8] + (url[k + 9] << 8) + (url[k + 10] << 16) + (url[k + 11] << 24);
                mix = Mix(a, b, c);
                a = mix[0]; b = mix[1]; c = mix[2];
                k += 12;
                len -= 12;
            }
            c += length;
            if (len == 11) c += url[k + 10] << 24;
            if (len >= 10) c += url[k + 9] << 16;
            if (len >= 9) c += url[k + 8] << 8;
            if (len >= 8) b += url[k + 7] << 24;
            if (len >= 7) b += url[k + 6] << 16;
            if (len >= 6) b += url[k + 5] << 8;
            if (len >= 5) b += url[k + 4];
            if (len >= 4) a += url[k + 3] << 24;
            if (len >= 3) a += url[k + 2] << 16;
            if (len >= 2) a += url[k + 1] << 8;
            if (len >= 1) a += url[k + 0];
            mix = Mix(a, b, c);
            return mix[2];
        }
        /// <summary>
        /// ���ַ���ת��Ϊ��Ӧ��asciiֵ����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        private int[] StrAsc(string str)
        {
            int l = str.Length;
            int[] r = new int[str.Length];
            for (int i = 0; i < l; i++) r[i] = (int)str[i];
            return r;
        }
        /// <summary>
        /// ��ȡgoogle��pr��������������PR
        /// </summary>
        /// <param name="url">Ҫ��ȡpr��url��ַ</param>
        public void GetGooglePR(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            if (url.Split('.').Length <= 2 && !url.StartsWith("www")) url = "www." + url;
            string ch = "6" + GoogleCH(StrAsc("info:" + url), -1, -1).ToString();
            WebClient wc = null;
            try
            {
                wc = new WebClient();
                string str = wc.DownloadString("http://www.google.cn/search?client=navclient-auto&ch=" + ch + "&features=Rank"
                    + "&q=info:" + url);
                string[] rs = str.Split(':');
                _PR = rs[rs.Length - 1];
            }
            catch { }
            finally
            {
                wc.Dispose();
            }
        }
    }
}

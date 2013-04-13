//使用方法：
//MD5Util.GetInstance().md5(MD5Util.GetInstance().md5_3(user.PassWord).ToUpper() + user.VerifyCode.ToUpper()).ToUpper();

using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Net.Tencent.QQ
{
    /// <summary>
    /// QQ 密码加密
    /// </summary>
    public class MD5Util
    {
        protected static MD5Util md5Util;
        protected const int hexcase = 1;
        protected const int chrsz = 8;
        protected const int mode = 32;

        public MD5Util()
        {

        }

        public static MD5Util GetInstance()
        {
            if (md5Util == null)
                return new MD5Util();
            return md5Util;
        }

        public string MD5_3(string B)
        {
            int[] A = Core_MD5(Str2Binl(B), B.Length * chrsz);
            A = Core_MD5(A, 16 * chrsz);
            A = Core_MD5(A, 16 * chrsz);
            return Binl2Hex(A);
        }

        public string MD5(string A)
        {
            return Hex_MD5(A);
        }

        protected int[] Core_MD5(int[] K, int F)
        {
            if ((F >> 5) + 1 > K.Length)
                K = NewArray(K, (F >> 5) + 1);
            K[F >> 5] |= 128 << ((F) % 32);
            //int a = K[F >> 5];
            int n = (int)((UrightMove(F + 64, 9) & (2 ^ 23 - 1)) << 4) + 14;//F+64无符号右移9位.
            if (n + 1 > K.Length)
                K = NewArray(K, n + 1);
            K[n] = F;
            int J = 1732584193;
            int I = -271733879;
            int H = -1732584194;
            int G = 271733878;
            for (int C = 0; C < K.Length; C += 16)
            {
                int E = J;
                int D = I;
                int B = H;
                int A = G;
                #region 变量初始

                int k0, k1, k2, k3, k4, k5, k6, k7, k8, k9, k10, k11, k12, k13, k14, k15;
                if (C + 0 >= K.Length)
                    k0 = 0;
                else
                    k0 = K[C + 0];
                if (C + 1 >= K.Length)
                    k1 = 0;
                else
                    k1 = K[C + 1];
                if (C + 2 >= K.Length)
                    k2 = 0;
                else
                    k2 = K[C + 2];
                if (C + 3 >= K.Length)
                    k3 = 0;
                else
                    k3 = K[C + 3];
                if (C + 4 >= K.Length)
                    k4 = 0;
                else
                    k4 = K[C + 4];
                if (C + 5 >= K.Length)
                    k5 = 0;
                else
                    k5 = K[C + 5];
                if (C + 6 >= K.Length)
                    k6 = 0;
                else
                    k6 = K[C + 6];
                if (C + 7 >= K.Length)
                    k7 = 0;
                else
                    k7 = K[C + 7];
                if (C + 8 >= K.Length)
                    k8 = 0;
                else
                    k8 = K[C + 8];
                if (C + 9 >= K.Length)
                    k9 = 0;
                else
                    k9 = K[C + 9];
                if (C + 10 >= K.Length)
                    k10 = 0;
                else
                    k10 = K[C + 10];
                if (C + 11 >= K.Length)
                    k11 = 0;
                else
                    k11 = K[C + 11];
                if (C + 12 >= K.Length)
                    k12 = 0;
                else
                    k12 = K[C + 12];
                if (C + 13 >= K.Length)
                    k13 = 0;
                else
                    k13 = K[C + 13];
                if (C + 14 >= K.Length)
                    k14 = 0;
                else
                    k14 = K[C + 14];
                if (C + 15 >= K.Length)
                    k15 = 0;
                else
                    k15 = K[C + 15];
                #endregion

                J = MD5_FF(J, I, H, G, k0, 7, -680876936);
                G = MD5_FF(G, J, I, H, k1, 12, -389564586);
                H = MD5_FF(H, G, J, I, k2, 17, 606105819);
                I = MD5_FF(I, H, G, J, k3, 22, -1044525330);
                J = MD5_FF(J, I, H, G, k4, 7, -176418897);
                G = MD5_FF(G, J, I, H, k5, 12, 1200080426);
                H = MD5_FF(H, G, J, I, k6, 17, -1473231341);
                I = MD5_FF(I, H, G, J, k7, 22, -45705983);
                J = MD5_FF(J, I, H, G, k8, 7, 1770035416);
                G = MD5_FF(G, J, I, H, k9, 12, -1958414417);
                H = MD5_FF(H, G, J, I, k10, 17, -42063);
                I = MD5_FF(I, H, G, J, k11, 22, -1990404162);
                J = MD5_FF(J, I, H, G, k12, 7, 1804603682);
                G = MD5_FF(G, J, I, H, k13, 12, -40341101);
                H = MD5_FF(H, G, J, I, k14, 17, -1502002290);
                I = MD5_FF(I, H, G, J, k15, 22, 1236535329);
                J = MD5_GG(J, I, H, G, k1, 5, -165796510);
                G = MD5_GG(G, J, I, H, k6, 9, -1069501632);
                H = MD5_GG(H, G, J, I, k11, 14, 643717713);
                I = MD5_GG(I, H, G, J, k0, 20, -373897302);
                J = MD5_GG(J, I, H, G, k5, 5, -701558691);
                G = MD5_GG(G, J, I, H, k10, 9, 38016083);
                H = MD5_GG(H, G, J, I, k15, 14, -660478335);
                I = MD5_GG(I, H, G, J, k4, 20, -405537848);
                J = MD5_GG(J, I, H, G, k9, 5, 568446438);
                G = MD5_GG(G, J, I, H, k14, 9, -1019803690);
                H = MD5_GG(H, G, J, I, k3, 14, -187363961);
                I = MD5_GG(I, H, G, J, k8, 20, 1163531501);
                J = MD5_GG(J, I, H, G, k13, 5, -1444681467);
                G = MD5_GG(G, J, I, H, k2, 9, -51403784);
                H = MD5_GG(H, G, J, I, k7, 14, 1735328473);
                I = MD5_GG(I, H, G, J, k12, 20, -1926607734);
                J = MD5_HH(J, I, H, G, k5, 4, -378558);
                G = MD5_HH(G, J, I, H, k8, 11, -2022574463);
                H = MD5_HH(H, G, J, I, k11, 16, 1839030562);
                I = MD5_HH(I, H, G, J, k14, 23, -35309556);
                J = MD5_HH(J, I, H, G, k1, 4, -1530992060);
                G = MD5_HH(G, J, I, H, k4, 11, 1272893353);
                H = MD5_HH(H, G, J, I, k7, 16, -155497632);
                I = MD5_HH(I, H, G, J, k10, 23, -1094730640);
                J = MD5_HH(J, I, H, G, k13, 4, 681279174);
                G = MD5_HH(G, J, I, H, k0, 11, -358537222);
                H = MD5_HH(H, G, J, I, k3, 16, -722521979);
                I = MD5_HH(I, H, G, J, k6, 23, 76029189);
                J = MD5_HH(J, I, H, G, k9, 4, -640364487);
                G = MD5_HH(G, J, I, H, k12, 11, -421815835);
                H = MD5_HH(H, G, J, I, k15, 16, 530742520);
                I = MD5_HH(I, H, G, J, k2, 23, -995338651);
                J = MD5_II(J, I, H, G, k0, 6, -198630844);
                G = MD5_II(G, J, I, H, k7, 10, 1126891415);
                H = MD5_II(H, G, J, I, k14, 15, -1416354905);
                I = MD5_II(I, H, G, J, k5, 21, -57434055);
                J = MD5_II(J, I, H, G, k12, 6, 1700485571);
                G = MD5_II(G, J, I, H, k3, 10, -1894986606);
                H = MD5_II(H, G, J, I, k10, 15, -1051523);
                I = MD5_II(I, H, G, J, k1, 21, -2054922799);
                J = MD5_II(J, I, H, G, k8, 6, 1873313359);
                G = MD5_II(G, J, I, H, k15, 10, -30611744);
                H = MD5_II(H, G, J, I, k6, 15, -1560198380);
                I = MD5_II(I, H, G, J, k13, 21, 1309151649);
                J = MD5_II(J, I, H, G, k4, 6, -145523070);
                G = MD5_II(G, J, I, H, k11, 10, -1120210379);
                H = MD5_II(H, G, J, I, k2, 15, 718787259);
                I = MD5_II(I, H, G, J, k9, 21, -343485551);
                J = Safe_Add(J, E);
                I = Safe_Add(I, D);
                H = Safe_Add(H, B);
                G = Safe_Add(G, A);
            }
            if (mode == 16)
            {
                return new int[] { I, H };
            }
            else
            {
                return new int[] { J, I, H, G };
            }
        }

        protected int[] NewArray(int[] old, int len)
        {
            int[] newAr = new int[len];
            for (int i = 0; i < old.Length; i++)
            {
                newAr[i] = old[i];
            }
            return newAr;
        }

        protected int MD5_FF(int C, int B, int G, int F, int A, int E, int D)
        {
            return MD5_Cmn((B & G) | ((~B) & F), C, B, A, E, D);
        }

        protected int MD5_GG(int C, int B, int G, int F, int A, int E, int D)
        {
            return MD5_Cmn((B & F) | (G & (~F)), C, B, A, E, D);
        }
        protected int MD5_HH(int C, int B, int G, int F, int A, int E, int D)
        {
            return MD5_Cmn(B ^ G ^ F, C, B, A, E, D);
        }

        protected int MD5_II(int C, int B, int G, int F, int A, int E, int D)
        {
            return MD5_Cmn(G ^ (B | (~F)), C, B, A, E, D);
        }

        protected string Hex_MD5(string A)
        {
            return Binl2Hex(Core_MD5(Str2Binl(A), A.Length * chrsz));
        }

        protected string Binl2Hex(int[] C)
        {
            string B = Convert.ToBoolean(hexcase) ? "0123456789ABCDEF" : "0123456789abcdef";
            string D = "";
            for (int A = 0; A < C.Length * 4; A++)
            {
                D += B.Substring((int)(C[A >> 2] >> (((int)A % 4) * 8 + 4)) & 15, 1) + B.Substring((int)(C[A >> 2] >> (((int)A % 4) * 8)) & 15, 1);
            }
            return D;
        }

        protected int[] Str2Binl(string D)
        {
            int[] C = new int[D.Length];
            char[] D_char = D.ToCharArray();
            int A = (1 << chrsz) - 1;
            for (int B = 0; B < D_char.Length * chrsz; B += chrsz)
            {
                C[B >> 5] |= (D_char[B / chrsz] & A) << (B % 32);
            }
            C = DelZero(C);
            return C;
        }

        protected int[] DelZero(int[] old)
        {
            int index;
            for (index = old.Length - 1; index >= 0; index--)
            {
                if (old[index] != 0)
                    break;
            }
            index++;
            int[] news = new int[index];
            for (int i = 0; i < index; i++)
            {
                news[i] = old[i];
            }
            return news;
        }

        protected int MD5_Cmn(int F, int C, int B, int A, int E, int D)
        {
            return Safe_Add(Bit_Rol(Safe_Add(Safe_Add(C, F), Safe_Add(A, D)), (int)E), B);
        }

        protected int Safe_Add(int A, int D)
        {
            int C = (A & 65535) + (D & 65535);
            int B = (A >> 16) + (D >> 16) + (C >> 16);
            return (B << 16) | (C & 65535);
        }

        protected int Bit_Rol(int A, int B)
        {
            return (A << B) | UrightMove(A, 32 - B);
        }

        protected int UrightMove(int A, int len)
        {
            if (A > 0)
                return A >> len;
            else
            {
                string tmp = Convert.ToString(A, 2);
                tmp = tmp.Substring(0, tmp.Length - len);
                for (int i = 0; i < len; i++)
                {
                    tmp = "0" + tmp;
                }
                return Convert.ToInt32(tmp, 2);
            }
        }

    }
}

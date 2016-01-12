using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace OZGNet
{
    /// <summary>
    /// QQ编码加密类
    /// </summary>
    public class QQHash
    {
        /// <summary>
        /// 通过MD5加密后返回加密后的BASE64密码
        /// </summary>
        /// <param name="pwd">要加密的内容</param>
        /// <returns>通过MD5加密后返回加密后的BASE64密码</returns>
        public static string PwdHash(string pwd)
        {
            //使用MD5的16位加密后再转换成BASE64
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();//实例化一个md5对像 
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　 
            byte[] s = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd));
            return Convert.ToBase64String(s);
        }
                
        /// <summary>
        /// 返回MD5的32位加密后的密码
        /// </summary>
        /// <param name="inputString">要加密的内容</param>
        /// <returns>加密后的结果</returns>
        public static string StringToMD5Hash(string inputString)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code)
        {
            string encode = "";
            byte[] bytes = System.Text.Encoding.Default.GetBytes(code);

            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="code_type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }


    }
}

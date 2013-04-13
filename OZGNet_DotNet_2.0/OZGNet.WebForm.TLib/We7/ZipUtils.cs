using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;

namespace OZGNet.WebForm.ThirdPart.We7
{
    /// <summary>
    /// ZIPÑ¹Ëõ»ù´¡²Ù×÷Àà
    /// </summary>
    public static class ZipUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream GetFileFromZip(Stream input, string fileName)
        {
            MemoryStream ms = new MemoryStream();

            fileName=fileName.Replace(@"\\", "/");
            fileName = fileName.Replace(@"\","/");
            using (ZipInputStream stream = new ZipInputStream(input))
            {
                ZipEntry ze;
                while ((ze = stream.GetNextEntry()) != null)
                {
                    if (ze.IsFile && ze.Name == fileName)
                    {
                        byte[] temp = new byte[0x1000];
                        int num=0;
                        while ((num=stream.Read(temp, 0, temp.Length))>0)
                        {
                            ms.Write(temp, 0, num);   
                        }
                    }
                }
            }
            ms.Position = 0;
            return ms;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="os"></param>
        public static void CreateZip(string path, Stream os)
        {
            FastZip fz = new FastZip();
            fz.CreateZip(os, path, true, null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="target"></param>
        public static void ExtractZip(Stream input, string target)
        {
            using (ZipInputStream st = new ZipInputStream(input))
            {
                ZipEntry ze;
                byte[] buffer = new byte[0x1000];
                while ((ze = st.GetNextEntry()) != null)
                {
                    string filename = ze.Name;
                    string dir = Path.Combine(target, filename);
                    string path = Path.GetDirectoryName(Path.GetFullPath(dir));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (ze.IsFile)
                    {
                        using (FileStream ts = File.Create(dir))
                        {
                            int num = 0;
                            while ((num = st.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                if (num > 0)
                                {
                                    ts.Write(buffer, 0, num);
                                }
                            }
                            ts.Flush();
                            ts.Close();
                        }
                    }
                    else if (ze.IsDirectory)
                    {   
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                    }
                }
            }
        }

    }
}

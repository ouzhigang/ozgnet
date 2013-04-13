using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Encryption;

namespace OZGNet.Compression
{
    /// <summary>
    /// SharpZipLib帮助类
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件路径</param>
        /// <param name="zipedFile">压缩后的文件路径</param>
        public static void FileToZip(string fileToZip, string zipedFile)
        {
            ZipClass zip = new ZipClass();
            zip.ZipFile(fileToZip, zipedFile);
        }
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="directoryToZip">要压缩的文件夹路径</param>
        /// <param name="zipedDirectory">压缩或的文件夹路径</param>
        public static void DirToZip(string directoryToZip, string zipedDirectory)
        {
            ZipClass zip = new ZipClass();
            zip.ZipDerctory(directoryToZip, zipedDirectory);
        }

        #region 解压缩文件(压缩文件中含有子目录)
        /// <summary>
        /// 解压缩文件(压缩文件中含有子目录)
        /// </summary>
        /// <param name="zipfilepath">待解压缩的文件路径</param>
        /// <param name="unzippath">解压缩到指定目录</param>
        public static void ZipToDir(string zipfilepath, string unzippath)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipfilepath));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(unzippath);
                string fileName = Path.GetFileName(theEntry.Name);

                //生成解压目录
                Directory.CreateDirectory(directoryName);

                if (fileName != String.Empty)
                {
                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入
                    if (theEntry.CompressedSize == 0)
                        break;
                    //解压文件到指定的目录
                    directoryName = Path.GetDirectoryName(unzippath + theEntry.Name);
                    //建立下面的目录和子目录
                    Directory.CreateDirectory(directoryName);

                    FileStream streamWriter = File.Create(unzippath + theEntry.Name);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
            s.Close();
        }
        #endregion

        #region bytes压缩解压
        /// <summary>
        /// bytes压缩
        /// </summary>
        /// <param name="data">bytes数据</param>
        /// <returns></returns>
        public static byte[] BytesZip(byte[] data)
        {
            MemoryStream mstream = new MemoryStream();
            BZip2OutputStream zipOutStream = new BZip2OutputStream(mstream);
            zipOutStream.Write(data, 0, data.Length);
            zipOutStream.Flush();
            zipOutStream.Close();

            byte[] result = mstream.ToArray();
            mstream.Close();

            return result;
        }
        /// <summary>
        /// bytes解压
        /// </summary>
        /// <param name="data">bytes数据</param>
        /// <returns></returns>
        public static byte[] BytesUnZip(byte[] data)
        {
            MemoryStream mstream = new MemoryStream(data);
            BZip2InputStream zipInputStream = new BZip2InputStream(mstream);
            byte[] byteUncompressed = new byte[zipInputStream.Length];
            zipInputStream.Read(byteUncompressed, 0, (int)byteUncompressed.Length);

            zipInputStream.Close();
            mstream.Close();

            return byteUncompressed;
        }
        #endregion

        /// <summary>
        /// 压缩多个文件
        /// </summary>
        /// <param name="strFiles">文件路径</param>
        /// <param name="ZiptoFileName">此参数不用加后缀.zip</param>
        /// <returns></returns>
        public static bool ZipMultiFile(string[] strFiles, string ZiptoFileName)
        {
            string strPhysicalPath = ZiptoFileName;
            string strZipFileName = strPhysicalPath + ".zip";

            if (File.Exists(strZipFileName))
            {
                File.Delete(strZipFileName);
            }

            //需壓縮的文件個數
            string[] strFilePaths = new string[strFiles.Length];

            MemoryStream oMemoryStream = new MemoryStream();

            ZipOutputStream oZipStream = new ZipOutputStream(File.Create(strZipFileName));

            try
            {

                for (int i = 0; i <= strFiles.Length - 1; i++)
                {
                    FileStream oReadFileStream = File.OpenRead(strFiles[i]);
                    byte[] btFile = new byte[oReadFileStream.Length];
                    oReadFileStream.Read(btFile, 0, btFile.Length);

                    string strCurrentFileName = Path.GetFileName(strFiles[i]);
                    strFilePaths[i] = strPhysicalPath + "/" + strCurrentFileName;

                    ZipEntry oZipEntry = new ZipEntry(strCurrentFileName);

                    oZipEntry.DateTime = DateTime.Now;
                    oZipEntry.Size = oReadFileStream.Length;

                    Crc32 oCrc32 = new Crc32();
                    oCrc32.Reset();
                    oCrc32.Update(btFile);


                    oZipEntry.Crc = oCrc32.Value;

                    oZipStream.PutNextEntry(oZipEntry);
                    oZipStream.Write(btFile, 0, btFile.Length);

                    oReadFileStream.Close();
                }

                oZipStream.Finish();
                oZipStream.Close();

                return true;
            }
            catch
            {
                oZipStream.Finish();
                oZipStream.Close();
                return false;
            }
        }


    }
}





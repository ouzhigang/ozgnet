using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

namespace OZGNet.WebForm.ThirdPart.We7
{
    /// <summary>
    /// zip压缩业务处理包括模板组等
    /// </summary>
    public static class ZipClass
    {
        
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="FileToZip"></param>
        /// <param name="ZipedFile"></param>
        /// <param name="CompressionLevel"></param>
        /// <param name="BlockSize"></param>
        public static void ZipFile(string FileToZip, string ZipedFile, int CompressionLevel, int BlockSize)
        {
            //如果檔沒有找到，則報錯
            if (!System.IO.File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("The specified file " + FileToZip + " could not be found. Zipping aborderd");
            }

            System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            ZipStream.PutNextEntry(ZipEntry);
            ZipStream.SetLevel(CompressionLevel);
            byte[] buffer = new byte[BlockSize];
            System.Int32 size = StreamToZip.Read(buffer, 0, buffer.Length);
            ZipStream.Write(buffer, 0, size);
            try
            {
                while (size < StreamToZip.Length)
                {
                    int sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                    ZipStream.Write(buffer, 0, sizeRead);
                    size += sizeRead;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            ZipStream.Finish();
            ZipStream.Close();
            StreamToZip.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="zipName"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string AddZip(string fileName, string zipName, ZipOutputStream s)
        {
            Crc32 crc = new Crc32();
            try
            {
                FileStream fs = File.OpenRead(fileName);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fileName = Path.GetFileName(fileName);
                long fileLength = fs.Length;
                fs.Close();

                ZipEntry entry = new ZipEntry(zipName);
                entry.DateTime = DateTime.Now;
                entry.Size = fileLength;

                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);
                s.Write(buffer, 0, buffer.Length);

                return string.Empty;
            }
            catch (Exception addEx)
            {
                return addEx.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="zipName"></param>
        /// <param name="s"></param>
        public static void AddFolder(string folderName, string zipName, ZipOutputStream s)
        {
            if (Directory.Exists(folderName))
            {
                foreach (string str in Directory.GetFileSystemEntries(folderName))
                {
                    if (File.Exists(str))
                        AddZip(str, zipName + "\\" + str.Substring(str.LastIndexOf("\\") + 1), s);
                    else
                        AddFolder(str, zipName + "\\" + str.Substring(str.LastIndexOf("\\") + 1), s);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="s"></param>
        public static void AddTemplateFolder(string folderName, ZipOutputStream s)
        {
            if (Directory.Exists(folderName))
            {
                foreach (string str in Directory.GetFileSystemEntries(folderName))
                {
                    if (File.Exists(str))
                        AddZip(str, "controls\\" + str.Substring(str.LastIndexOf("\\") + 1), s);
                    else
                        AddFolder(str,str.Substring(str.LastIndexOf("\\") + 1), s);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void CreateTemplateZip(string[] args)
        {
            string dirName = null;
            dirName = Path.GetDirectoryName(Path.GetFullPath(args[1]));
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            ZipOutputStream s = new ZipOutputStream(File.Create(args[1]));

            s.SetLevel(6); // 0 - store only to 9 - means best compression

            AddTemplateFolder(args[0],s);

            s.Finish();
            s.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void ZipFileMain(string[] args)
        {
            string dirName = null;
            dirName = Path.GetDirectoryName(Path.GetFullPath(args[1]));
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            ZipOutputStream s = new ZipOutputStream(File.Create(args[1]));

            s.SetLevel(6); // 0 - store only to 9 - means best compression

            //AddFolder(args[0], args[0].Substring(args[0].Length - 1) == "\\" ? args[0].Substring(0, args[0].Length - 1).Substring(args[0].Substring(0, args[0].Length - 1).LastIndexOf("\\") + 1) : args[0].Substring(args[0].LastIndexOf("\\") + 1), s);
            AddTemplateFolder(args[0], s);

            s.Finish();
            s.Close();
        }
    }
}



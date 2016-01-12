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
    internal class ZipClass
    {
        private int compressionLevel = 9;
        private byte[] buffer = new byte[2048];

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ZipClass()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="compressionLevel">压缩率：0-9</param>
        public ZipClass(int bufferSize, int compressionLevel)
        {
            buffer = new byte[bufferSize];
            this.compressionLevel = compressionLevel;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件路径</param>
        /// <param name="zipedFile">压缩后的文件路径</param>
        public void ZipFile(string fileToZip, string zipedFile)
        {
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("The specified file " + fileToZip + " could not be found.");
            }

            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile)))
            {
                string fileName = Path.GetFileName(fileToZip);
                ZipEntry zipEntry = new ZipEntry(fileName);
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(compressionLevel);

                using (FileStream streamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read))
                {
                    int size = streamToZip.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, size);

                    while (size < streamToZip.Length)
                    {
                        int sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                        zipStream.Write(buffer, 0, sizeRead);
                        size += sizeRead;
                    }
                }
            }
        }

        /// <summary>
        /// 得到文件下的所有文件
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <returns></returns>
        public ArrayList GetFileList(string directory)
        {
            ArrayList fileList = new ArrayList();
            bool isEmpty = true;
            foreach (string file in Directory.GetFiles(directory))
            {
                fileList.Add(file);
                isEmpty = false;
            }
            if (isEmpty)
            {
                if (Directory.GetDirectories(directory).Length == 0)
                {
                    fileList.Add(directory + @"/");
                }
            }
            foreach (string dirs in Directory.GetDirectories(directory))
            {
                foreach (object obj in GetFileList(dirs))
                {
                    fileList.Add(obj);
                }
            }
            return fileList;
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="directoryToZip">要压缩的文件夹路径</param>
        /// <param name="zipedDirectory">压缩或的文件夹路径</param>
        public void ZipDerctory(string directoryToZip, string zipedDirectory)
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedDirectory)))
            {
                ArrayList fileList = GetFileList(directoryToZip);
                int directoryNameLength = (Directory.GetParent(directoryToZip)).ToString().Length;

                zipStream.SetLevel(compressionLevel);
                ZipEntry zipEntry = null;
                FileStream fileStream = null;

                foreach (string fileName in fileList)
                {
                    zipEntry = new ZipEntry(fileName.Remove(0, directoryNameLength));
                    zipStream.PutNextEntry(zipEntry);

                    if (!fileName.EndsWith(@"/"))
                    {
                        fileStream = File.OpenRead(fileName);
                        fileStream.Read(buffer, 0, buffer.Length);
                        zipStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}

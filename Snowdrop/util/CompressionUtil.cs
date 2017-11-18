// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;

namespace Snowdrop.util
{
    class CompressionUtil
    {
        // Method for compressing files.
        public static void Compress(FileInfo fileToCompress, string rootFolder)
        {
            try
            {
                //just for testing, needs to be rewritten
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if (File.GetAttributes(fileToCompress.FullName).HasFlag(FileAttributes.Hidden) & fileToCompress.Extension != ".gz")
                    {
                        if (!Directory.Exists(rootFolder + "\\temporary"))
                        {
                            Directory.CreateDirectory(rootFolder + "\\temporary");
                        }
                        using (StreamWriter sw = File.AppendText(rootFolder + "\\temporary\\checksum"))
                        {
                            sw.WriteLine(fileToCompress.FullName.Replace(rootFolder, "") + ";" + CryptographyUtil.Md5(File.ReadAllBytes(fileToCompress.FullName)));
                        }

                        Directory.CreateDirectory(rootFolder + "\\temporary\\" + fileToCompress.FullName.Replace(rootFolder, "").Replace(fileToCompress.Name, ""));
                        using (FileStream compressedFileStream = File.Create(rootFolder + "\\temporary\\" + fileToCompress.FullName.Replace(rootFolder, "") + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                            {
                                // copy the file of the 
                                // stream to the specified location
                                originalFileStream.CopyTo(compressionStream);
                                FileInfo fileCompressed = new FileInfo(rootFolder + "\\temporary\\" + fileToCompress.FullName.Replace(rootFolder, "") + ".gz");
                                Console.WriteLine("Compressed {0} from {1} to {2} bytes.", fileToCompress.Name, fileToCompress.Length.ToString(), fileCompressed.Length.ToString());
                                LoggingUtil.Info(String.Format("Compressed: {0}", fileToCompress.FullName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log if the file could not be 
                // compressed due to an exception.
                LoggingUtil.Exception(String.Format("Compress Exception: {0}", ex.Message));
            }
        }

        // Method for decompressing files.
        public static void Decompress(FileInfo fileToDecompress)
        {
            try
            {
                // open the file as a file stream
                using (FileStream decompressFileStream = fileToDecompress.OpenRead())
                {
                    // full name of the file including the 
                    // compressed file extension e.g. "C:\Data.exe.gz"
                    string fullFileName = fileToDecompress.FullName;

                    // full name of the file without the 
                    // compressed file extension e.g. "C:\Data.exe"
                    string fileNameWithoutExtension = fullFileName.Remove(fullFileName.Length - fileToDecompress.Extension.Length);

                    // create the file
                    using (FileStream decompressedFileStream = File.Create(fileNameWithoutExtension))
                    {
                        // decompress the file
                        using (GZipStream decompressionStream = new GZipStream(decompressFileStream, CompressionMode.Decompress))
                        {
                            // copy the file of the 
                            // stream to the specified location
                            decompressionStream.CopyTo(decompressedFileStream);
                            LoggingUtil.Info(String.Format("Decompressed: {0}", fileToDecompress.FullName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log if the file could not be 
                // decompressed due to an exception.
                LoggingUtil.Exception(String.Format("Decompress Exception: {0}", ex.Message));
            }
        }
    }
}

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
        public static void Compress(FileInfo fileToCompress, string baseFolder)
        {
            try
            {
                // open the file as a file stream
                // that need to be compressed
                using (FileStream fileStream = fileToCompress.OpenRead())
                {
                    // if the file is not hidden and has not the 
                    // compression file extension (e.g. ".gz")
                    // go on and compress the file
                    if (!File.GetAttributes(fileToCompress.FullName).HasFlag(FileAttributes.Hidden) & fileToCompress.Extension != Configuration.COMPRESSION_FORMAT)
                    {
                        // create a temporary directory if it does not exist
                        DirectoryUtil.CreateDirectory(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME));

                        //if (File.Exists(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME)) && Regex.IsMatch(File.ReadAllText(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME)), fileToCompress.FullName.Replace(baseFolder, "") + ";" + ".*"))
                        //{
                        //    File.WriteAllText(baseFolder + @"\" + Configuration.TEMP_FOLDER_NAME + @"\" + Configuration.CHECKSUM_NAME, Regex.Replace(File.ReadAllText(baseFolder + @"\" + Configuration.TEMP_FOLDER_NAME + @"\" + Configuration.CHECKSUM_NAME), fileToCompress.FullName.Replace(baseFolder, "") + ";" + ".*", fileToCompress.FullName.Replace(baseFolder, "") + ";" + CryptographyUtil.Md5(File.ReadAllBytes(fileToCompress.FullName))));
                        //}
                        //else
                        //{
                        // generate a md5 checksum and append it to 
                        // the filename and the path seperated by a ";"
                        // e.g. "Snowdrop.exe;aeb23baef889c3cb0b759c46aa2d428c"
                        using (StreamWriter streamWriter = File.AppendText(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME)))
                        {
                            // replace the baseFolder from the path
                            // we do not want an absolute path
                            // TODO: transfer into checksum util and add 
                            // function to replace a already set md5 with a
                            // new one generated 
                            streamWriter.WriteLine(fileToCompress.FullName.Replace(baseFolder, "") + ";" + CryptographyUtil.Md5(File.ReadAllBytes(fileToCompress.FullName)));
                        }
                        //}

                        // create the sub-directory in the temp folder 
                        // if it does not exist
                        DirectoryUtil.CreateDirectory(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME + fileToCompress.FullName.Replace(baseFolder, "").Replace(fileToCompress.Name, "")));

                        // create the file in the location of the base 
                        // folder + temp folder + the file name, again we
                        // need to replace the base path to append it after the
                        // temp folder in order to prevent an absolute path at the end
                        // append the compressed file extension
                        using (FileStream compressedFileStream = File.Create(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME + fileToCompress.FullName.Replace(baseFolder, "") + Configuration.COMPRESSION_FORMAT)))
                        {
                            // compress the file
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                            {
                                // copy the file of the 
                                // stream to the specified location
                                fileStream.CopyTo(compressionStream);
                                LoggingUtil.Info(String.Format("Compressed: {0}", fileToCompress.FullName));
                            }

                            // just some not explicit needed information
                            // reduced size info after the compression ...
                            FileInfo fileInfo = new FileInfo(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME + fileToCompress.FullName.Replace(baseFolder, "") + Configuration.COMPRESSION_FORMAT));
                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.", fileToCompress.Name, fileToCompress.Length.ToString(), fileInfo.Length.ToString());
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
                            // copy the decompressed file of the 
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

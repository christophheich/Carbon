// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Snowdrop.lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Snowdrop.util
{
    class CompressionUtil
    {
        private static Dictionary<string, string> checksumElements = new Dictionary<string, string>();
        private static AtomicBoolean checksumCreated = new AtomicBoolean() { Value = false };

        public static void WriteFile(string baseFolder)
        {
            // Delete the file as we read it already and now write a new one
            DirectoryUtil.DeleteFile(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME));

            using (StreamWriter streamWriter = File.AppendText(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME)))
            {
                // replace the baseFolder from the path
                // we do not want an absolute path
                foreach (var element in checksumElements)
                {
                    //write every element e.g. Snowdrop.exe;4243342342341sdf
                    streamWriter.WriteLine(element.Key + ";" + element.Value); 
                }
            }
        }

        // Method for compressing files.
        public static void Compress(FileInfo fileToCompress, string baseFolder)
        {
            // atomic boolean if the value of 
            // checksumcreated is false set it to true and continue
            if (checksumCreated.CompareAndExchange(false, true))
            {
                // if the file does exist read the content
                if (File.Exists(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME)))
                {
                    using (var streamReader = new StreamReader(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME, Configuration.CHECKSUM_NAME)))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            // spereate the line by ; to get the path and md5
                            // add it afterwards to the dictionary
                            string[] separation = line.Split(';');
                            checksumElements.Add(separation[0], separation[1]);
                        }
                    }
                }
            }

            try
            {
                // open the file as a file stream
                // that need to be compressed
                using (FileStream fileStream = fileToCompress.OpenRead())
                {
                    // if the file is not hidden and has not the 
                    // compression file extension (e.g. ".gz")
                    // go on and compress the file
                    if (!File.GetAttributes(fileToCompress.FullName).HasFlag(FileAttributes.Hidden) & !fileToCompress.DirectoryName.Contains(Configuration.TEMP_FOLDER_NAME))
                    {
                        // create a temporary directory if it does not exist
                        DirectoryUtil.CreateDirectory(Path.Combine(baseFolder, Configuration.TEMP_FOLDER_NAME));

                        // path for the dictionary key
                        string key = fileToCompress.FullName.Replace(baseFolder, "");

                        // generate the md5 of the file
                        string hashValue = CryptographyUtil.Md5(File.ReadAllBytes(fileToCompress.FullName));

                        // try to get the value of path (key) and set it to the variable valueKey
                        // check if the value of the key is not equal to the md5 if so
                        // delete the value from the dictionary and set the new one
                        if (checksumElements.TryGetValue(key, out string valueKey)) {
                            if (valueKey != hashValue)
                            {
                                checksumElements.Remove(key);
                                checksumElements.Add(key, hashValue);
                            }
                        }
                        else 
                        {
                            // if there is no key just add the key and value
                            checksumElements.Add(key, hashValue);
                        }

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

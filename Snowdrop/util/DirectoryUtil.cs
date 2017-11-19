// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Snowdrop.util
{
    class DirectoryUtil
    {
        // method to create and check if a directory exists.
        public static void CreateDirectory(string path)
        {
            // check if directory does not exist
            if (!Directory.Exists(path))
            {
                // create the directory
                Directory.CreateDirectory(path);
            }
        }


        // method to create and check if a directory exists.
        public static void DeleteDirectory(string path)
        {
            // check if directory exist
            if (Directory.Exists(path))
            {
                // delete the directory
                Directory.Delete(path);
            }
        }

        // method to delete and check if a file exists.
        public static void DeleteFile(string path)
        {
            // check if the file exist
            if (File.Exists(path))
            {
                // delete the file
                File.Delete(path);
            }
        }
    }
}

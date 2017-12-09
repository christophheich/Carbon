// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Snowdrop.util;
using System;
using System.IO;
using System.Windows;

namespace Snowdrop
{
    public static class ConsoleWindow
    {
        public static void Program(StartupEventArgs e)
        {
            Console.WriteLine(@"==================================================");
            Console.WriteLine(@"  ____                         _                  ");
            Console.WriteLine(@" / ___| _ __   _____      ____| |_ __ ___  _ __   ");
            Console.WriteLine(@" \___ \| '_ \ / _ \ \ /\ / / _` | '__/ _ \| '_ \  ");
            Console.WriteLine(@"  ___) | | | | (_) \ V  V / (_| | | | (_) | |_) | ");
            Console.WriteLine(@" |____/|_| |_|\___/ \_/\_/ \__,_|_|  \___/| .__/  ");
            Console.WriteLine(@"                                          |_|     ");
            Console.WriteLine(@"==================================================");
            Console.WriteLine(@"           Download, Install and Update           ");
            Console.WriteLine(@"==================================================");
            Console.Write("\n");

            try
            {
                // Create new directoryInfo for the file or 
                // folder using the path passed through the arguments.
                DirectoryInfo directoryInfo = new DirectoryInfo(e.Args[0]);

                // Check if the fileAttributes has the 
                // flag directory and go into the if case.
                // If it is not a directory handle it as a file.
                if (File.GetAttributes(directoryInfo.FullName).HasFlag(FileAttributes.Directory))
                {
                    Console.WriteLine("=================== Directory ====================");

                    // Loop through every directory within the root directory
                    // check if it is a file and compress the file.
                    foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                    {
                        CompressionUtil.Compress(new FileInfo(fileInfo.FullName), e.Args[0]);
                    }
                }
                else
                {
                    Console.WriteLine("===================== File ======================");
                    // If the flag is a file compress only the 
                    // file and not the full directory.
                    CompressionUtil.Compress(new FileInfo(directoryInfo.FullName), directoryInfo.FullName.Remove(directoryInfo.FullName.Length - directoryInfo.Name.Length));
                }
            }
            catch (FileNotFoundException ex)
            {
                // Show that the file or folder is not 
                // available anymore or never existed.
                Console.WriteLine(String.Format("The file or folder \"{0}\" does either not exist or the path is incorrect.", ex.FileName));
                LoggingUtil.Exception(String.Format("Argument Exception: {0}", ex));
            }

            Console.Write("\n");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

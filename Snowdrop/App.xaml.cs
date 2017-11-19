// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Snowdrop.util;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Snowdrop
{
    /// <summary>
    /// Interaction logic for "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        /**
         * Handles the application startup, if there are command line arguments 
         * specified the console application will start in administration mode 
         * to generate hashes for files. If there are no command line arugments
         * specified the application will start in user mode to download and 
         * update files.
         */
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Check if the log file exist if so delete it
            DirectoryUtil.DeleteFile(Configuration.APPDATA_PATH + @"\" + Configuration.LOG_NAME);

            // if there are arguments specified
            if (e.Args.Length >= 1)
            {
                // Allocates a new console for the calling process.
                AllocConsole();

                // Call the method "Program" of the Static class ConsoleWindow.
                ConsoleWindow.Program(e);

                // Detaches the calling process from its console.
                FreeConsole();

                // Exit the console application with exit code 0.
                Environment.Exit(0);
            }
            else
            {
                // Create a new class of MainWindow
                MainWindow wnd = new MainWindow();
                
                // show the MainWindow
                wnd.Show();
            }
        }
    }
}

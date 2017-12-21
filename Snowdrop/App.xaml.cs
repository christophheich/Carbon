// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Snowdrop.util;
using Squirrel;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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

        public static Mutex mutex;

        /**
         * Handles the application startup, if there are command line arguments 
         * specified the console application will start in administration mode 
         * to generate hashes for files. If there are no command line arugments
         * specified the application will start in user mode to download and 
         * update files.
         */
        private void Application_Startup(object sender, StartupEventArgs startupEventArgs)
        {
            // using mutex to prevent the application startup a second time?
            if (Configuration.USING_MUTEX)
            {
                try
                {
                    // Try to open existing mutex.
                    Mutex.OpenExisting(Configuration.MUTEX_NAME);
                    Environment.Exit(0);
                }
                catch
                {
                    // If exception occurred, there is no such mutex and we create one.
                    mutex = new Mutex(true, Configuration.MUTEX_NAME);
                }
            }

            // run the squirrel update process async in the background
            if (Configuration.USING_SQUIRREL_UPDATER == true)
            {
                Task.Run(async () =>
                {
                    // Initialize Squirrel Update
                    if (Configuration.GITHUB_UPDATE_MANAGER == true)
                    {
                        using (var mgr = UpdateManager.GitHubUpdateManager(Configuration.GITHUB_UPDATE_MANAGER_URL))
                        {
                            await mgr.Result.UpdateApp();
                        }
                    }
                    else
                    {
                        using (var mgr = new UpdateManager(Configuration.UPDATE_MANAGER_URL))
                        {
                            await mgr.UpdateApp();
                        }
                    }
                }).GetAwaiter().GetResult();
            }


            // if the log file should be deleted on application startup
            if (Configuration.DELETE_LOG_FILE == true)
            {
                // Check if the log file exist if so delete it
                DirectoryUtil.DeleteFile(Path.Combine(Configuration.APPDATA_PATH, Configuration.LOG_NAME));
            }
            

            // if there are arguments specified and it does not containt --squirrel
            if (startupEventArgs.Args.Length >= 1 && !startupEventArgs.Args[0].Contains("--squirrel"))
            {
                // Allocates a new console for the calling process.
                AllocConsole();

                // Call the method "Program" of the Static class ConsoleWindow.
                ConsoleWindow.Program(startupEventArgs);

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

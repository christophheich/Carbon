// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Snowdrop.util
{
    class LoggingUtil
    {
        // Append each log event into the log file specified
        // NOTE: we delete the log file each time the program starts
        // the code for deletion is within the App.xaml code
        private static void Writer(string type, string message)
        {
            // if the appdata dir does not exist create it
            DirectoryUtil.CreateDirectory(Configuration.APPDATA_PATH);

            using (StreamWriter streamWriter = File.AppendText(Configuration.APPDATA_PATH + "/" + Configuration.LOG_NAME))
            {
                streamWriter.WriteLine(DateTime.Now.ToString("d/M/yyyy HH:mm:ss ") + type + message);
            }
        }

        public static void Info(string info)
        {
            Writer("[Info]: ", info);
        }

        public static void Debug(string debug)
        {
            Writer("[Debug]: ", debug);
        }

        public static void Exception(string exception)
        {
            Writer("[Exception]: ", exception);
        }

        public static void Error(string error)
        {
            Writer("[Error]: ", error);
        }

        public static void Warning(string warning)
        {
            Writer("[Warning]: ", warning);
        }
    }
}

using System;
using System.IO;

namespace Snowdrop.util
{
    class LoggingUtil
    {
        // Append each log event into the log file specified.
        private static void Writer(string type, string message)
        {
            using (StreamWriter streamWriter = File.AppendText(Configuration.SETTINGS_SAVE_PATH + "/update.log"))
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

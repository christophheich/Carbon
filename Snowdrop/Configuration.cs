// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Snowdrop
{
    class Configuration
    {
        // ----------------------------
        //       LOCALIZATION
        // ----------------------------
        public static readonly string[] LANGUAGES = { "enu", "deu", "fra", "plk", "rus" };



        // ----------------------------
        //             LOG
        // ----------------------------
        public const string LOG_NAME = "update.log";



        // ----------------------------
        //       UPDATE BASE URL
        // ----------------------------
        public const string UPDATE_BASE = @"http://download.localhost/";
        public const string UPDATE_FILE_BASE = @"http://download.localhost/client/";



        // ----------------------------
        //    TODO: USING SQUIRREL?
        // ----------------------------
        public const string UPGRADE_BASE = @"http://download.localhost/Snowdrop.exe";



        // ----------------------------
        //       APPDATA PATH
        // ----------------------------
        public static string APPDATA_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowdrop");



        // ----------------------------
        //       COMPRESSION
        // ----------------------------
        public const string COMPRESSION_FORMAT = ".gz";



        // ----------------------------
        //       TEMP
        // ----------------------------
        public const string TEMP_FOLDER_NAME = "temp";
        public const string CHECKSUM_NAME = "checksum";
        public const string JSON_NAME = "setting.json";
        
    }
}

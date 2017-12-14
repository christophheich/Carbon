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
        //       SQUIRREL UPDATER
        // ----------------------------

        // Using GitHubUpdateManager?
        public const bool GITHUB_UPDATE_MANAGER = false;

        // Make sure your url doesn't end in a forward slash ("/").
        public const string GITHUB_UPDATE_MANAGER_URL = @"https://github.com/myuser/myapp";

        // Only if GITHUB_UPDATE_MANAGER is 'false' we are then using local or own HTTP/S.
        public const string UPDATE_MANAGER_URL = @"I:\Snowdrop\Snowdrop\Releases";


        // ----------------------------
        //       LOCALIZATION
        // ----------------------------
        public static readonly string[] LANGUAGES = {
            "en-US",
            "de-DE",
            "fr-FR",
            "pl-PL",
            "ru-RU"
        };


        // ----------------------------
        //             LOG
        // ----------------------------

        // Enable logging?
        public const bool LOG_ENABLED = true;

        // Delete log file on application start?
        public const bool DELETE_LOG_FILE = true;

        // Name of the log file.
        public const string LOG_NAME = "application.log";



        // ----------------------------
        //       UPDATE BASE URL
        // ----------------------------
        public const string UPDATE_BASE = @"http://download.localhost/";
        public const string UPDATE_FILE_BASE = @"http://download.localhost/client/";



        // ----------------------------
        //       APPDATA PATH
        // ----------------------------
        public static string APPDATA_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Snowdrop");



        // ----------------------------
        //       COMPRESSION
        // ----------------------------

        // this will not change the compression itself
        // buz you can use it to customize your files for example .snowdrop
        public const string COMPRESSION_FORMAT = ".gz";



        // ----------------------------
        //       TEMP
        // ----------------------------
        public const string TEMP_FOLDER_NAME = "APPLICATION_TEMP_FOLDER";
        public const string CHECKSUM_NAME = "checksum.txt";
        public const string JSON_NAME = "setting.json";
        
    }
}

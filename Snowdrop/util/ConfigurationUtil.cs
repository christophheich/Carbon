// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Snowdrop.json;
using System;
using System.IO;

namespace Snowdrop.util
{
    class ConfigurationUtil
    {
        // create a new class of the getter/setter ConfigurationJsonModel
        public static ConfigurationJsonModel ConfigurationJsonModel { get; set; }

        // Method used to load the content of the json file.
        public static void Load()
        {
            try
            {
                // read the json file and deserialize it
                // save the content of each json value into
                // the ConfigurationJsonModel
                ConfigurationJsonModel = JsonConvert.DeserializeObject<ConfigurationJsonModel>(File.ReadAllText(Configuration.APPDATA_PATH + @"\" + Configuration.JSON_NAME));
            }
            catch (Exception ex)
            {
                // set the ConfigurationJsonModel to null
                // we will create a new one as the old
                // does not exist or is corrupted
                ConfigurationJsonModel = null;
                LoggingUtil.Exception(String.Format("Configuration Load Exception {0}", ex));
            }
        }

        // Method used to save the content to the json file.
        public static void Save()
        {
            // if the appdata dir does not exist create it
            DirectoryUtil.CreateDirectory(Configuration.APPDATA_PATH);

            // serialize the content to the json format
            // write it as a file into the appdata path
            File.WriteAllText(Configuration.APPDATA_PATH + @"\" + Configuration.JSON_NAME, JsonConvert.SerializeObject(ConfigurationJsonModel));
        }
    }
}

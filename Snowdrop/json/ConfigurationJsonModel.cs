// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Snowdrop.json
{
    public class ConfigurationJsonModel
    {
        [JsonProperty("BASE_DIRECTORY_PATH")]
        public string BaseDirectoryPath { get; set; }

        [JsonProperty("APPLICATION_VERSION")]
        public string ApplicationVersion { get; set; }

        [JsonProperty("UPDATE_VERSION")]
        public long UpdateVersion { get; set; }

        [JsonProperty("LANGUAGE")]
        public string Language { get; set; }
    }
}

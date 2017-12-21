// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Snowdrop.json
{
    public class WebspaceJsonModel
    {
        [JsonProperty("UPDATE_VERSION")]
        public string UpdateVersion { get; set; }

        [JsonProperty("ANNOUNCEMENT")]
        public string Announcement { get; set; }

        [JsonProperty("CHANGELOG")]
        public string[] Changelog { get; set; }

        [JsonProperty("EVENT")]
        public string[] Event { get; set; }

        [JsonProperty("IMAGE")]
        public string[] Image { get; set; }
    }
}

// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Snowdrop.json
{
    public class FileJsonModel
    {
        [JsonProperty("NAME")]
        public string Name { get; set; }

        [JsonProperty("PATH")]
        public string Path { get; set; }

        [JsonProperty("HASH")]
        public string Hash { get; set; }

        [JsonProperty("DATE")]
        public string Date { get; set; }

        [JsonProperty("SIZE")]
        public long Size { get; set; }

        [JsonProperty("LANGUAGE")]
        public string[] Language { get; set; }
    }
}

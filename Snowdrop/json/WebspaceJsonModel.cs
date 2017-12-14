// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Snowdrop.json
{
    public class WebspaceJsonModel
    {
        public string UPDATE_VERSION { get; set; }
        public string ANNOUNCEMENT { get; set; }
        public string CHANGELOG { get; set; }
        public string EVENT { get; set; }
        public string IMAGE { get; set; }
    }
}

// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using Snowdrop.json;
using Snowdrop.util;
using System.Reflection;
using System.Resources;

namespace Snowdrop
{
    // this class is used to not declare the 
    // resource manager over and over again
    public class ApplicationManager
    {
        // Translation
        private static readonly ResourceManager resourceManager = new ResourceManager("Snowdrop.Resources.Translation", Assembly.GetExecutingAssembly());

        public static ResourceManager Localization
        {
            get => resourceManager;
        }


        // Webspace
        private static WebspaceJsonModel webspaceJsonModel = new WebspaceJsonModel();

        public static WebspaceJsonModel Webspace
        {
            get => webspaceJsonModel;
            set => webspaceJsonModel = value;
        }
    }
}

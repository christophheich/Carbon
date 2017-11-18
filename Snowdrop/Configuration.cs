using System;
using System.IO;

namespace Snowdrop
{
    class Configuration
    {
        public static string SETTINGS_SAVE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Kataria");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimpleMinecraftLauncher
{
    internal class Constants
    {
        /// <summary>
        /// absolute path of minecraft clients (extracted)
        /// </summary>
        public static string MINECRAFT_CLIENTS_PATH = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\.minecraft_fae\\");
        /// <summary>
        /// relative path of application json version file
        /// </summary>
        public static string JSON_CONFIG_PATH = "\\version_data.json";
        /// <summary>
        /// relative path of minecraft zipped client builds
        /// </summary>
        public static string MINECRAFT_PACKED_CLIENTS_CONTAINER_PATH = "\\zips\\";
        /// <summary>
        /// runs string command as Windows CMD command
        /// </summary>
        /// <param name="cmd"></param>
        public static void InitSystemCommand(string cmd)
        {
            Process.Start("CMD.exe", cmd);
        }

    }
}

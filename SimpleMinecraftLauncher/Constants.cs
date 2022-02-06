using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SimpleMinecraftLauncher
{
    internal static class Constants
    {

        /// <summary>
        /// runs string command as Windows CMD command
        /// </summary>
        /// <param name="cmd"></param>
        public static void InitSystemCommand(string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
            //p.StartInfo.WorkingDirectory = @"C:\myproject";
            p.StartInfo.Arguments = cmd;
            p.Start();

        }

        public static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                        queue.Enqueue(subDir);
                }
                catch (Exception ex){}
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex){}
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                        yield return files[i];
                }
            }
        }

        public static dynamic Clamp(dynamic v, dynamic min, dynamic max)
        {
            return v > max ? max : (v < min ? min : v);
        }

        internal static class Web
        {
            private const string HOST_IP = "5.9.31.179:32127";
            /// <summary>
            /// Get current launcher build version
            /// </summary>
            public static string HOST_GET_LAUNCHER_BUILD = "http://" + HOST_IP + "/getLauncherVersion";
            /// <summary>
            /// URL to download client archive from host by given archive name
            /// </summary>
            public static string HOST_DOWNLOAD_CLIENT_ARCHIVE = "http://" + HOST_IP + "/versions/";
            /// <summary>
            /// URL to get exact archive CRC by provided 'archive_name' query parameter
            /// </summary>
            public static string HOST_GET_ARCHIVE_CRC_URL = "http://" + HOST_IP + "/getMinecraftClientCRC?archive_name=";
            /// <summary>
            /// URL to get json config CRC
            /// </summary>
            public static string HOST_GET_CONFIG_CRC_URL = "http://" + HOST_IP + "/getConfigCRC";
            /// <summary>
            /// URL to download json config from host directly
            /// </summary>
            public static string HOST_GET_CONFIG_DOWNLOAD_URL = "http://" + HOST_IP + "/versions/version_data.json";
            /// <summary>
            /// URL to launcher build
            /// </summary>
            public static string HOST_GET_LAUNCHER_DOWNLOAD_URL = "http://" + HOST_IP + "/launcher.zip";
        }

        internal static class Util
        {
            /// <summary>
            /// launcher update installation archive name
            /// </summary>
            public static string LAUNCHER_UPDATE_FILENAME = "launcher.zip";
            /// <summary>
            /// a special RegEx string for nickname format
            /// </summary>
            public static string MINECRAFT_NICKNAME_REGEX = "^[a-zA-Z-0-9_]*$";
            /// <summary>
            /// launcher assembly version RegEx
            /// </summary>
            public static string LAUNCHER_VERSION_REGEX = @"\d.\d.\d.\d";
        }

        internal static class Path
        {
            /// <summary>
            /// absolute path of minecraft clients (installed)
            /// </summary>
            public static string MINECRAFT_INSTALLED_CLIENTS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.fae_minecraft";
            /// <summary>
            /// relative path of minecraft clients (zips)
            /// </summary>
            public static string MINECRAFT_CLIENTS_PATH = "\\versions\\";
            /// <summary>
            /// relative path of application json version file
            /// </summary>
            public static string JSON_CONFIG_PATH = "\\versions\\version_data.json";
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMinecraftLauncher
{
    internal class MinecraftLauncher
    {

        private string ClientPath;
        private LaunchOptions Options;
        internal bool CanLaunch = false;

        public MinecraftLauncher(string client_path, string username, string version_name, string min_ram, string max_ram, string serverHost=null, string serverPort=null)
        {

            ClientPath = client_path;
            Options = new LaunchOptions(client_path, username, version_name, min_ram, max_ram);
            if (Options.IsClassPathExist)
                CanLaunch = true;

        }

        internal void LaunchClient()
        {

            if (!FormManager.GetMainForm().ValidateNickname())
            {
                UIManager.ShowNotification("Никнейм должен соответствовать условиям. Минимум символов - 3. Максимум - 16. Только английские символы, цифры от 0 до 9 и нижнее подчеркивание", true);
                UIManager.MainForm().textBox1.Focus();
                return;
            }
            else
                try
                {
                    ProcessStartInfo minecraft = new ProcessStartInfo
                    {
                        FileName = "javaw",
                        CreateNoWindow = false,
                        Arguments = Options.BuildArguments()
                    };
                    Process minecraftProcess = Process.Start(minecraft);
                    UIManager.MainForm().Hide();

                    while (!minecraftProcess.HasExited & minecraftProcess.Responding)
                    {
                        Thread.Sleep(1000);
                    }

                    UIManager.MainForm().Show();
                }
                catch (Exception ex)
                {
                    Log("Не удалось запустить клиент - " + ex.Message);
                    if (ex is Win32Exception)
                        UIManager.ShowNotification("Отсутствует Java! Не удалось запустить клиент Minecraft. Установите Java и попробуйте снова.", true);
                }
        }

        private void Log(string s)
        {
            UIManager.MainForm().Log(s);
        }

    }

    internal class LaunchOptions
    {
        private protected string[] RequiredParams { get; private set; }
        private protected string[] ExtraParams { get; private set; }
        private protected string[] ClassPathLibraries { get; private set; }

        private protected string ClientPath = string.Empty;
        private protected string ClientNickname = string.Empty;
        private protected string ClientVersion = string.Empty;
        private protected string ClientAssetsIndex = string.Empty;
        private protected string ClientUUID = "1e0ab942-8024-11ec-b05c-2346f1e65647";
        private protected string ClientAccessToken = "1e0ab940-8024-11ec-b05c-2346f1e65647";
        internal bool IsClassPathExist = false;
        private void GetClassPathLibraries()
        {

            List<string> temp = new List<string>() { };

            foreach (string file in Constants.GetFiles(ClientPath + @"\libraries"))
            {
                temp.Add(file);
            }

            foreach (string file in Constants.GetFiles(ClientPath + @"\versions"))
            {
                temp.Add(file);
            }


            ClassPathLibraries = temp.ToArray();
            if (temp.Count > 0)
                IsClassPathExist = true;

        }

        private string BuildClassPath()
        {
            string ret = string.Empty;
            foreach (string cp in ClassPathLibraries)
                ret += cp + ";";

            return ret;
        }

        // TODO: complete making pieces of launcher
        // add "assetIndex to Version class in launcher and config editor "
        // fix hash comparing problems

        private void SetAssetIndex(string versionName)
        {
            ClientAssetsIndex = versionName.Split('.')[0] + "." + versionName.Split('.')[1];
        }

        private string GetJavaLibraryPath()
        {
            return ClientPath + @"\natives\" + ClientVersion;
        }
        /// <summary>
        /// Builds extra long argument string for launch JVM
        /// </summary>
        /// <returns></returns>
        internal string BuildArguments() {
            string ret = string.Empty;
            foreach (string arg in RequiredParams)
                ret += arg + " ";
            return ret;
        }

        /// <summary>
        /// Options for launch client
        /// </summary>
        /// <param name="clientPath">path to minecraft client main folder</param>
        /// <param name="username">your username</param>
        /// <param name="versionName">name of version - for example 1.15.2</param>
        /// <param name="MinMemory">minimum amount of RAM for JVM. For example - 2G, 2048M, 4G</param>
        /// <param name="MaxMemory">maximum amount of RAM (see MinMemory for details)</param>
        public LaunchOptions(string clientPath, string username, string versionName, string MinMemory, string MaxMemory)
        {
            ClientPath = clientPath;
            ClientVersion = versionName;

            SetAssetIndex(versionName);
            GetClassPathLibraries();

            RequiredParams = new string[] {
                "-XX:-UseAdaptiveSizePolicy",
                "-XX:-OmitStackTraceInFastThrow",
                "-Dfml.ignorePatchDiscrepancies=true",
                "-Dfml.ignoreInvalidMinecraftCertificates=true",
                "-Djava.library.path="+GetJavaLibraryPath(),
                "-Xms"+MinMemory,
                "-Xmx"+MaxMemory,
                "-XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump",
                //"-Dlog4j.configurationFile=log4j2_112-116.xml",
                "-cp "+BuildClassPath(),
                "net.minecraft.client.main.Main",
                "--username "+username,
                "--version "+versionName,
                "--gameDir "+clientPath,
                "--assetsDir "+clientPath+@"\assets",
                "--assetIndex "+ClientAssetsIndex,
                "--uuid "+ClientUUID,
                "--accessToken "+ClientAccessToken,
                "--userType mojang",
                "--versionType release"
            };
        }
    }
}

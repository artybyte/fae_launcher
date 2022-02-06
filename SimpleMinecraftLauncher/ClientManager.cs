using SimpleMinecraftLauncher.Properties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Drawing;
using System.Diagnostics;

namespace SimpleMinecraftLauncher
{
    internal class ClientManager
    {

        public delegate void OnClientDataReady(object sender, ClientDataReadyEventArgs e);

        private string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private VersionControl CurrentVersions;
        public event OnClientDataReady onClientDataReady;
        /// <summary>
        /// Loads and prepares actual versions data 
        /// </summary>
        internal void LoadUpVersions()
        {
            string VersionDataJSON = File.ReadAllText(CurrentDirectory + Constants.Path.JSON_CONFIG_PATH);
            CurrentVersions = VersionControlDeserializer.AssembleVersionControl(VersionDataJSON);

            onClientDataReady?.Invoke(this, null);

            UIManager.ShowLoadingPanel();
            UIManager.SetProgress(33);

            UpdateLocalConfigCRC();

            // this method should be used (IMPORTANT)******************************************************************************************************

            //UpdateVersionControlCRCs(CurrentVersions); 

            // now we get deserialized VersionControl with updated CRCs and ready to compare it with host version of file
            Log("Проверка контрольных сумм клиентов..");
            ValidateClientCRCs();
            UIManager.SetProgress(66);
            Log("Проверка контрольной суммы конфига..");
            ValidateConfigCRC();
            UIManager.SetProgress(0);
            UIManager.HideLoadingPanel();

        }
        /// <summary>
        /// Saves actual version data to config file
        /// </summary>
        internal void LoadOutVersions()
        {
            string VersionDataJSON = VersionControlDeserializer.DisassembleVersionControl(CurrentVersions);
            File.WriteAllText(CurrentDirectory + Constants.Path.JSON_CONFIG_PATH, VersionDataJSON);
        }

        /// <summary>
        /// checks existance of clients folder and JSON config file
        /// </summary>
        internal async void ValidateDependencies(Action onJSONCreated = null)
        {
            if (!Directory.Exists(CurrentDirectory + Constants.Path.MINECRAFT_CLIENTS_PATH))
                Directory.CreateDirectory(CurrentDirectory + Constants.Path.MINECRAFT_CLIENTS_PATH);

            if (!Directory.Exists(Constants.Path.MINECRAFT_INSTALLED_CLIENTS_PATH))
                Directory.CreateDirectory(Constants.Path.MINECRAFT_INSTALLED_CLIENTS_PATH);

            if (!File.Exists(CurrentDirectory + Constants.Path.JSON_CONFIG_PATH))
                File.Create(CurrentDirectory + Constants.Path.JSON_CONFIG_PATH).Close();
        }

        internal async void UpdateLauncher()
        {
            string tempPath = CurrentDirectory + @"\temp";
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);

            Directory.CreateDirectory(tempPath);

            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (a, e) =>
                {
                    double bytesIn = double.Parse(e.BytesReceived.ToString());
                    double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                    double percentage = bytesIn / totalBytes * 100;

                    UIManager.SetProgress((int)percentage);

                    Log("Загрузка обновления лаунчера - " + Math.Floor(percentage) + "%", true, Color.Blue);

                };

                client.DownloadFileCompleted += (a, b) =>
                {
                    FileInfo fs = new FileInfo(tempPath + @"\launcher.zip");
                    if (fs.Length == 0)
                        OnDownloadError();
                    else
                        InstallLauncher(tempPath);
                };

                UIManager.ShowLoadingPanel();
                client.DownloadFileAsync(new Uri(Constants.Web.HOST_GET_LAUNCHER_DOWNLOAD_URL, UriKind.Absolute), CurrentDirectory + @"\temp\launcher.zip");
            }
        }

        internal async void InstallClient(Version version, Action callback)
        {
            AsyncWorker.PollAsyncMethod(async () =>
            {
                UIManager.SetProgress(0);
                UIManager.ShowLoadingPanel();

                Log("Устанавливаем клиент...");

                UIManager.DisableControls();

                string archivePath = GetClientZIPFile(version.mVersionArchiveName);
                string InstallTo = GetClientPath(version.mVersionArchiveName);

                await Task.Run(() =>
                {
                    DeleteClient(version, async () =>
                    {
                        UIManager.SetProgress(33);
                        if (!File.Exists(archivePath))
                            await DownloadClientArchive(version, () =>
                            {
                                UIManager.SetProgress(66);
                                ExtractArchive(archivePath, InstallTo);
                                UIManager.SetProgress(100);
                                UIManager.EnableControls();
                                UIManager.SetProgress(0);
                                UIManager.HideLoadingPanel();
                            });
                        else
                        {
                            UIManager.SetProgress(66);
                            ExtractArchive(archivePath, InstallTo);
                            UIManager.EnableControls();
                            UIManager.SetProgress(0);
                            UIManager.HideLoadingPanel();
                        }
                        Log("Распаковка " + version.mVersionArchiveName + " прошла успешно", false, Color.Green);

                        callback();
                    });
                });

            });
        }
        internal string GetClientPath(string archiveName)
        {
            return Constants.Path.MINECRAFT_INSTALLED_CLIENTS_PATH + "\\" + ArchiveToFolder(archiveName);
        }

        internal string ArchiveToFolder(string s)
        {
            return s.Replace(".zip", "");
        }
        /// <summary>
        /// Doing same like ArchiveToFolder
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal string StripExtension(string s)
        {
            return ArchiveToFolder(s);
        }

        internal void DeleteArchive(Version version, Action callback = null)
        {

            UIManager.DisableControls();

            AsyncWorker.PollAsyncMethod(() => {

                if (File.Exists(GetClientZIPFile(version.mVersionArchiveName)))
                    File.Delete(GetClientZIPFile(version.mVersionArchiveName));

                if (callback != null)
                    callback();

                UIManager.EnableControls();
            });
        }

        internal void DeleteClient(Version version, Action callback)
        {
            AsyncWorker.PollAsyncMethod(() =>
            {
                Log("Удаляем клиент " + StripExtension(version.mVersionArchiveName));

                DirectoryInfo di = new DirectoryInfo(GetClientPath(version.mVersionArchiveName));
                foreach (FileInfo file in di.GetFiles())
                    file.Delete();

                foreach (DirectoryInfo dir in di.GetDirectories())
                    dir.Delete(true);

                Log("Удаление " + StripExtension(version.mVersionArchiveName) + " прошло успешно", false, Color.Green);

                callback();
            });
        }

        internal void ValidateGameClient(Version version, bool notify_client_ready = false)
        {
            if (version == null)
            {
                Log("Ошибка - версия не обнаружена", false, Color.Red);
                return;
            }

            ValidateClientInstallation(version, false, notify_client_ready);
            CurrentVersions.versions[CurrentVersions.versions.IndexOf(version)].SetValidated(true);
        }

        internal VersionControl GetVersionData()
        {
            return CurrentVersions;
        }

        internal class ClientDataReadyEventArgs
        {
            public string Response { get; }
            public ClientDataReadyEventArgs(string response)
            {
                Response = response;
            }

        }

        private void RequestConfigChecksum()
        {
            // send request to server and get back hashsum of json config file.
        }

        private void Log(string log, bool clearBefore=false, Color? highlight=null)
        {
            UIManager.LogMessage(log, clearBefore, highlight);
        }

        private async void ValidateClientCRCs()
        {
            try
            {
                foreach (Version version in CurrentVersions.versions)
                {
                    if (!version.mVersionEnabled)
                        continue;
                    string actualCRC = await AsyncWorker.MakeHTTPRequest(Constants.Web.HOST_GET_ARCHIVE_CRC_URL + version.mVersionArchiveName);
                    if (version.mArchiveChecksum != actualCRC)
                        version.mValidatedCRC = false;
                    else
                        version.mValidatedCRC = true;
                }
                Log("Проверка контрольных сумм - ОК", false, Color.Green);
                if (UIManager.CanEnableMainButton())
                    UIManager.EnableMainButton();

                UIManager.EnableByCheckComplete();
            }
            catch (Exception ex)
            {
                Log("Проверка контрольных сумм - Ошибка (" + ex.Message + ")", false, Color.Red);
            }
        }

        private string GetClientZIPFile(string archiveName)
        {
            return CurrentDirectory + Constants.Path.MINECRAFT_CLIENTS_PATH + "\\" + archiveName;
        }
        
        private void OnDownloadError()
        {
            Log("Загрузка закончилась с ошибкой", false, Color.IndianRed);
            UIManager.ShowNotification("Не удалось загрузить данные. Соединение недоступно");
            UIManager.HideLoadingPanel();
            UIManager.EnableControls();
        }

        private async void ValidateClientArchive(Version version, Action callbackOnDownload)
        {
            if (version == null)
            {
                Log("Ошибка - версия не обнаружена", false, Color.Red);
                return;
            }

            Log("Начинаем проверку оригинала клиента...");

            if (File.Exists(GetClientZIPFile(version.mVersionArchiveName)))
            {
                string archiveMD5 = GetMD5(GetClientZIPFile(version.mVersionArchiveName));


                //  *******************************************************************************************************************
                // validation of md5 or file existance (bug here)

                if (archiveMD5 == version.mArchiveChecksum)
                {
                    Log("Архив " + version.mVersionArchiveName + " в порядке", false, Color.Green);
                    callbackOnDownload();
                }
                else
                {
                    Log("Не верная контрольная сумма " + version.mVersionArchiveName + " - скачиваем", false, Color.Yellow);
                    await DownloadClientArchive(version, () => { UIManager.EnableControls(); });
                }
            }
            else
            {
                Log("Не найден " + version.mVersionArchiveName + " - скачиваем", false, Color.Yellow);
                await DownloadClientArchive(version, () => { UIManager.EnableControls(); });
            }
        }

        private async void UpdateConfig()
        {
            try
            {
                Log("Скачиваем новый конфиг...");
                string json = await AsyncWorker.MakeHTTPRequest(Constants.Web.HOST_GET_CONFIG_DOWNLOAD_URL);
                File.WriteAllText(CurrentDirectory + Constants.Path.JSON_CONFIG_PATH, json);

                using (FileStream FS = new FileStream(CurrentDirectory + Constants.Path.JSON_CONFIG_PATH, FileMode.Create))
                {
                    using (StreamWriter SW = new StreamWriter(FS))
                    {
                        SW.Write(json);
                    }
                }

                Log("Повторное чтение конфига после обновления...", true, Color.Yellow);
                LoadUpVersions();

                Log("Конфиг успешно скачан и обновлён (!)", false, Color.Green);

            }
            catch (Exception ex)
            {
                Log("Не удалось обновить конфиг (" + ex.Message + ")", false, Color.Red);
            }

        }

        private async void ValidateConfigCRC()
        {
            UIManager.SelectFirstAvailableVersion();
            try
            {
                string actualConfigCRC = await AsyncWorker.MakeHTTPRequest(Constants.Web.HOST_GET_CONFIG_CRC_URL);
                int crc = 0;
                int.TryParse(actualConfigCRC, out crc);

                if (crc == 0)
                {
                    Log("Проверка контрольной суммы конфига невозможна (соединение недоступно)", false, Color.Red);
                    return;
                }
                else if (crc == CurrentVersions.checksum)
                {
                    Log("Проверка контрольной суммы конфига - ОК", false, Color.Green);
                }
                else
                {
                    Log("Проверка контрольной суммы конфига - найдены отличия, обновляем", false, Color.Yellow);
                    UpdateConfig();
                }

            }
            catch (Exception ex)
            {
                Log("Проверка контрольных сумм - Ошибка (" + ex.Message + ")", false, Color.Red);
            }
        }

        private async Task DownloadClientArchive(Version version, Action callback)
        {

            if (version == null)
            {
                Log("Ошибка - версия не обнаружена", false, Color.Red);
                return;
            }

            await Task.Run(() =>
            {

                string ret = string.Empty;

                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadProgressChanged += (a, e) =>
                        {
                            double bytesIn = double.Parse(e.BytesReceived.ToString());
                            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                            double percentage = bytesIn / totalBytes * 100;

                            UIManager.SetProgress((int)percentage);

                            Log("Загрузка " + version.mVersionArchiveName + " - " + Math.Floor(percentage) + "%", true, Color.Blue);

                        };

                        client.DownloadFileCompleted += (a, b) =>
                        {

                            FileInfo fs = new FileInfo(GetClientZIPFile(version.mVersionArchiveName));

                            if (fs.Length == 0)
                            {
                                OnDownloadError();
                            }
                            else
                            {
                                Log("Проверка установки...");
                                ValidateClientInstallation(version);
                                UIManager.SetProgress(0);
                                UIManager.HideLoadingPanel();

                                ValidateClientInstallation(version, true, true);

                                callback();
                            }
                        };

                        UIManager.DisableControls();
                        UIManager.ShowLoadingPanel();
                        client.DownloadFileAsync(new Uri(Constants.Web.HOST_DOWNLOAD_CLIENT_ARCHIVE + version.mVersionArchiveName, UriKind.Absolute), GetClientZIPFile(version.mVersionArchiveName));
                    }
                    Log("Архив " + version.mVersionArchiveName + " скачан успешно", false, Color.Green);
                }
                catch (Exception ex)
                {
                    Log("Не удалось скачать архив " + version.mVersionArchiveName + "(" + ex.Message + ")", false, Color.Red);
                }
                return ret;
            });

        }
        // set ip of host ssh
        private void InstallLauncher(string path)
        {
            string executableName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            // first line - close current instance of app
            string BatFile = "taskkill /f /im " + executableName + "\n";
            // then delete all files of current app
            foreach (string f in Directory.EnumerateFiles(CurrentDirectory))
                if (Path.GetFileName(f) != Constants.Util.LAUNCHER_UPDATE_FILENAME)
                    BatFile += "del " + f + "\n";
            // then move downloaded .zip to main app folder
            BatFile += "move " + path + @"\launcher.zip " + CurrentDirectory + "\n";
            // extract new app via PowerShell
            BatFile += "powershell -Command \"Expand-Archive " + Constants.Util.LAUNCHER_UPDATE_FILENAME + " -Force -DestinationPath " + CurrentDirectory + "\"\n";
            // delete .zip
            BatFile += "del " + CurrentDirectory + @"\launcher.zip" + "\n";
            // start new app instance
            BatFile += "start " + CurrentDirectory + @"\" + executableName + "\n";
            File.WriteAllText(path + @"\install.bat", BatFile);

            ProcessStartInfo batInstaller = new ProcessStartInfo()
            {
                FileName = path + @"\install.bat",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(batInstaller);

            UIManager.DisableUpdateButton();
        }

        private void ExtractArchive(string archivePath, string installPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(archivePath, installPath);
            }
            catch (Exception ex)
            {
                Log("Не удалось распаковать архив " + archivePath + "(" + ex.Message + ")", false, Color.Red);
                UIManager.ShowNotification("Не удалось распаковать архив (" + ex.Message + ")");
            }
        }

        private bool SimpleClientExistanceValidation(Version version)
        {

            string clientDir = GetClientPath(version.mVersionArchiveName);

            if (!Directory.Exists(clientDir))
                Directory.CreateDirectory(clientDir);

            int filesCount = Directory.GetFiles(clientDir).Length;

            return filesCount >= 3;
        }

        private void ValidateClientInstallation(Version version, bool force_reinstall = false, bool show_client_ready_notification = false)
        {
            if (version == null)
            {
                Log("Ошибка - версия не обнаружена", false, Color.Red);
                return;
            }

            UIManager.DisableControls();

            ValidateClientArchive(version, () => {
                if (!SimpleClientExistanceValidation(version) | force_reinstall)
                    InstallClient(version, () => {
                        UIManager.EnableControls();
                        UIManager.SwitctToLaunch();
                        if (show_client_ready_notification)
                            UIManager.ShowNotification("Клиент в порядке. Можно запускать!");
                    });
                else
                {
                    Log("Клиент " + version.mVersionArchiveName + " в порядке", false, Color.Green);
                    UIManager.EnableControls();
                    UIManager.SwitctToLaunch();
                    if (show_client_ready_notification)
                    {
                        UIManager.ShowNotification("Клиент в порядке. Можно запускать!");
                        FlashWindow.FlashWindowEx(UIManager.MainForm());
                    }
                }
            });
        }
    
        private string GetMD5(string filename)
        {
            string ret = string.Empty;
            using (var md5 = MD5.Create())
            {
                try
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        stream.Dispose();
                        ret = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
                catch (Exception ex)
                {
                    Log("Не удалось вызвать метод GetMD5", false, Color.DarkRed);
                }
            }
            return ret;
        }

        private Dictionary<string, string> CalculateVersionCRCs()
        {
            Dictionary<string, string> VerCRC = new Dictionary<string, string>();
            string[] files = Directory.GetFiles(CurrentDirectory + "/versions");
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".zip")
                {
                    VerCRC[Path.GetFileName(file)] = GetMD5(file);
                }
            }
            return VerCRC;
        }

        private void UpdateLocalConfigCRC()
        {
            Settings.Default.config_checksum = CurrentVersions.checksum;
            Settings.Default.Save();
        }

        private void UpdateVersionControlCRCs(VersionControl VC)
        {
            if (VC == null)
                return;
            Dictionary<string, string> D = CalculateVersionCRCs();
            foreach (Version version in VC.versions)
            {
                if (D[version.mVersionArchiveName] != null)
                    version.mArchiveChecksum = D[version.mVersionArchiveName];
            }
        }
    }
}

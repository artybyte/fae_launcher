using SimpleMinecraftLauncher.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace SimpleMinecraftLauncher
{
    internal class ClientManager
    {

        public delegate void OnClientDataReady(object sender, ClientDataReadyEventArgs e);

        private string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private VersionControl CurrentVersions;
        public event OnClientDataReady onClientDataReady;

        private void RequestConfigChecksum()
        {
            // send request to server and get back hashsum of json config file.
        }

        private void Log(string log, bool clearBefore=false)
        {
            if (FormManager.GetMainForm() == null)
                return;
            FormManager.GetMainForm().Log(log, clearBefore);
        }

        /// <summary>
        /// Loads and prepares actual versions data 
        /// </summary>
        internal void LoadUpVersions()
        {
            string VersionDataJSON = File.ReadAllText(CurrentDirectory+Constants.JSON_CONFIG_PATH);
            CurrentVersions = VersionControlDeserializer.AssembleVersionControl(VersionDataJSON);

            onClientDataReady?.Invoke(this, null);
            
            UpdateLocalConfigCRC();

            // this method should be used (IMPORTANT)******************************************************************************************************

            //UpdateVersionControlCRCs(CurrentVersions); 

            // now we get deserialized VersionControl with updated CRCs and ready to compare it with host version of file
            Log("Проверка контрольных сумм клиентов..");
            ValidateClientCRCs();
            Log("Проверка контрольной суммы конфига..");
            ValidateConfigCRC();

        }
        /// <summary>
        /// Saves actual version data to config file
        /// </summary>
        internal void LoadOutVersions()
        {
            string VersionDataJSON = VersionControlDeserializer.DisassembleVersionControl(CurrentVersions);
            File.WriteAllText(CurrentDirectory + Constants.JSON_CONFIG_PATH, VersionDataJSON);
        }

        /// <summary>
        /// checks existance of clients folder and JSON config file
        /// </summary>
        internal async void ValidateDependencies(Action onJSONCreated=null)
        {
            if (!Directory.Exists(CurrentDirectory + Constants.MINECRAFT_CLIENTS_PATH))
                Directory.CreateDirectory(CurrentDirectory + Constants.MINECRAFT_CLIENTS_PATH);

            if (!Directory.Exists(Constants.MINECRAFT_INSTALLED_CLIENTS_PATH))
                Directory.CreateDirectory(Constants.MINECRAFT_INSTALLED_CLIENTS_PATH);

            if (!File.Exists(CurrentDirectory + Constants.JSON_CONFIG_PATH))
                File.Create(CurrentDirectory + Constants.JSON_CONFIG_PATH).Close();
        }
        // перевести все запросы на async

        private async void ValidateClientCRCs()
        {
            try
            {
                foreach (Version V in CurrentVersions.versions)
                {
                    string actualCRC = await AsyncWorker.MakeHTTPRequest(Constants.HOST_GET_ARCHIVE_CRC_URL + V.mVersionArchiveName);
                    if (V.mArchiveChecksum != actualCRC)
                        V.mValidatedCRC = false;
                    else
                        V.mValidatedCRC = true;
                }
                Log("Проверка контрольных сумм - ОК");
                if (FormManager.GetMainForm().CanEnableMainButton)
                    FormManager.GetMainForm().button1.Enabled = true;

                FormManager.GetMainForm().EnabledByCheckComplete = true;
            }
            catch (Exception ex)
            {
                Log("Проверка контрольных сумм - Ошибка (" + ex.Message + ")");
            }
        }

        private Version GetVersion(int verId)
        {
            Version ret = null;

            try
            {
                ret = CurrentVersions.versions[verId];
            }
            catch (Exception ex) {
                Log("Ошибка - версия #" + verId + " не обнаружена");
            }
            return ret;
        }

        private string GetClientZIPFile(string archiveName)
        {
            return CurrentDirectory + Constants.MINECRAFT_CLIENTS_PATH + "\\" + archiveName;
        }

        private async Task DownloadClientArchive(Version V)
        {
            
            if (V == null)
            {
                Log("Ошибка - версия не обнаружена (код ошибки - 1)");
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

                            FormManager.GetMainForm().SetLoadingProgress((int)percentage);

                            Log("Загрузка " + V.mVersionArchiveName + " - " + percentage + "%", true);

                        };

                        client.DownloadFileCompleted += (a, b) =>
                        {

                            Log("Проверка установки...");
                            ValidateClientInstallation(V);
                            FormManager.GetMainForm().SetLoadingProgress(0);
                            FormManager.GetMainForm().ShowLoadingPanel(false);

                            ValidateClientInstallation(V, true);

                        };

                        FormManager.GetMainForm().DisableVersionControls();
                        FormManager.GetMainForm().ShowLoadingPanel(true);
                        client.DownloadFileAsync(new Uri(Constants.HOST_DOWNLOAD_CLIENT_ARCHIVE + V.mVersionArchiveName, UriKind.Absolute), GetClientZIPFile(V.mVersionArchiveName));
                    }
                    Log("Архив " + V.mVersionArchiveName + " скачан успешно");
                }
                catch (Exception ex)
                {
                    Log("Не удалось скачать архив " + V.mVersionArchiveName + "(" + ex.Message + ")");
                }
                return ret;
            });

        }
        internal string GetClientPath(string archiveName)
        {
            return Constants.MINECRAFT_INSTALLED_CLIENTS_PATH + "\\" + ArchiveToFolder(archiveName);
        }
        internal string ArchiveToFolder(string s)
        {
            return s.Replace(".zip", "");
        }

        internal void DeleteClient(Version V)
        {
            AsyncWorker.PollAsyncMethod(() =>
            {
                Log("Удаляем клиент " + V.mVersionArchiveName);

                FormManager.GetMainForm().DisableVersionControls();

                DirectoryInfo di = new DirectoryInfo(GetClientPath(V.mVersionArchiveName));
                foreach (FileInfo file in di.GetFiles())
                    file.Delete();

                foreach (DirectoryInfo dir in di.GetDirectories())
                    dir.Delete(true);

                FormManager.GetMainForm().EnableVersionControls();

                Log("Удаление " + V.mVersionArchiveName + " прошло успешно");
            });
        }
        internal void InstallClient(Version V)
        {
            AsyncWorker.PollAsyncMethod(async () =>
                {
                Log("Устанавливаем клиент...");
                FormManager.GetMainForm().DisableVersionControls();

                string ArchivePath = CurrentDirectory + Constants.MINECRAFT_CLIENTS_PATH + "\\" + V.mVersionArchiveName;
                string InstallTo = GetClientPath(V.mVersionArchiveName);

                DeleteClient(V);

                await Task.Run(()=> { ZipFile.ExtractToDirectory(ArchivePath, InstallTo); });
                Log("Распаковка " + V.mVersionArchiveName + " прошла успешно");

                FormManager.GetMainForm().EnableVersionControls();
                });
        }

        private async void ValidateClientInstallation(Version V, bool force_reinstall=false)
        {
            if (V == null)
            {
                Log("Ошибка - версия не обнаружена (код ошибки - 1)");
                return;
            }

            string clientDir = GetClientPath(V.mVersionArchiveName);

            if (!Directory.Exists(clientDir))
                Directory.CreateDirectory(clientDir);

            int filesCount = Directory.GetFiles(clientDir).Length;

            if (filesCount >= 3 & !force_reinstall)
            {
                // ok chill - client is ok (not perfect method to validate, but ok)
                Log("Клиент "+V.mVersionArchiveName+" в порядке");
            }
            else
            {
                ValidateClientArchive(V);

                // unpack client folder - install client
                InstallClient(V);
            }

        }
        private async void ValidateClientArchive(Version V)
        {

            if (V == null)
            {
                Log("Ошибка - версия не обнаружена (код ошибки - 1)");
                return;
            }

            Log("Начинаем проверку оригиналов клиентов...");
            // we need to store archives at client pc to compare their's crc with original client crc stored on host server

            if (File.Exists(GetClientZIPFile(V.mVersionArchiveName)))
            {
                // ok, file exists
                // validate archive crc
                if (GetMD5(GetClientZIPFile(V.mVersionArchiveName)) == V.mArchiveChecksum)
                {
                    // ok, client validated
                    Log("Архив " + V.mVersionArchiveName + " в порядке");
                }
                else
                {
                    // no, client is not actual, download
                    Log("Не верная контрольная сумма " + V.mVersionArchiveName + " - скачиваем");
                    await DownloadClientArchive(V);
                }
            }
            else
            {
                Log("Не найден "+ V.mVersionArchiveName + " - скачиваем");
                await DownloadClientArchive(V);
            }
        }

        internal void ValidateGameClient(Version V)
        {
            if (V == null)
            {
                Log("Ошибка - версия не обнаружена (код ошибки - 1)");
                return;
            }

            //AsyncWorker.PollAsyncMethod(() => {
                ValidateClientInstallation(V); // test
                CurrentVersions.versions[CurrentVersions.versions.IndexOf(V)].SetValidated(true);
                FormManager.GetMainForm().SwitchToLaunch();
            //}
            //);
        }

        private async void UpdateConfig()
        {
            try
            {
                Log("Скачиваем новый конфиг...");
                string json = await AsyncWorker.MakeHTTPRequest(Constants.HOST_GET_CONFIG_DOWNLOAD_URL);
                File.WriteAllText(CurrentDirectory + Constants.JSON_CONFIG_PATH, json);

                using (FileStream FS = new FileStream(CurrentDirectory + Constants.JSON_CONFIG_PATH, FileMode.Create)) {
                    using (StreamWriter SW = new StreamWriter(FS))
                    {
                        SW.Write(json);
                    }
                }

                Log("Повторное чтение конфига после обновления...", true);
                LoadUpVersions();

                Log("Конфиг успешно скачан и обновлён (!)");

            } catch(Exception ex)
            {
                Log("Не удалось обновить конфиг ("+ex.Message+")");
            }

        }

        private async void ValidateConfigCRC()
        {
            try
            {
                string actualConfigCRC = await AsyncWorker.MakeHTTPRequest(Constants.HOST_GET_CONFIG_CRC_URL);
                int crc = 0;
                int.TryParse(actualConfigCRC, out crc);

                if (crc == 0)
                {
                    Log("Проверка контрольной суммы конфига невозможна (соединение недоступно)");
                    return;
                }
                else if (crc == CurrentVersions.checksum)
                {
                    Log("Проверка контрольной суммы конфига - ОК");
                }
                else
                {
                    Log("Проверка контрольной суммы конфига - найдены отличия, обновляем");
                    UpdateConfig();
                }

                FormManager.GetMainForm().SelectFirstAvailableVersion();
            }
            catch (Exception ex)
            {
                Log("Проверка контрольных сумм - Ошибка (" + ex.Message + ")");
            }
        }

        private string GetMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        internal VersionControl GetVersionData() {
            return CurrentVersions;
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
            foreach (Version V in VC.versions)
            {
                if (D[V.mVersionArchiveName] != null)
                    V.mArchiveChecksum = D[V.mVersionArchiveName];
            }
        }
    }

    internal class ClientDataReadyEventArgs
    {
        public string Response { get; }
        public ClientDataReadyEventArgs(string response) { 
            Response = response;
        }

    }

}

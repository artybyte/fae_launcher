using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SimpleMinecraftLauncher.Properties;
using Newtonsoft.Json;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Drawing;

namespace SimpleMinecraftLauncher
{
    public partial class Form1 : Form
    {
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        internal bool CanEnableMainButton = false;
        internal bool EnabledByCheckComplete = false;

        private bool Launching = false;
        private MinecraftLauncher mClientLauncher;
        private HashSet<string> ProhibitedClients = new HashSet<string>() { "1.15.2_dev_mods.zip" };

        ClientManager mClientManager = new ClientManager();

        public void Log(string l, bool clear=false)
        {
            try
            {
                logBox.BeginInvoke(new MethodInvoker(() =>
                {
                    if (clear)
                        logBox.Clear();
                    logBox.AppendText(l + '\n');
                }));
            }
            catch (Exception ex) { }
        }

        public Form1()
        {
            InitializeComponent();

            button1.OnClick += button1_Click;
            button1.Text = "Проверить клиент";
        }


        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void windowTopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        internal void SwitchToCheck()
        {
            Launching = false;
            button1.Text = "Проверить клиент";
        }

        internal void SelectFirstAvailableVersion()
        {
            if (mClientManager.GetVersionData().versions.Count > 0)
                versionList.SelectedIndex = 0;
        }

        internal void SwitchToLaunch()
        {
            Launching = true;
            button1.Text = "Запуск";
        }

        private void bMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        internal void EnableVersionControls()
        {
            Invoke(new MethodInvoker(() => {
                button1.Enabled = true;
                versionList.Enabled = true;
            }));
        }

        internal void DisableVersionControls()
        {
            Invoke(new MethodInvoker(() => {
                button1.Enabled = false;
                versionList.Enabled = false;
            }));
        }

        private async void ValidateLauncherBuild()
        {
            string ver = await AsyncWorker.MakeHTTPRequest(Constants.HOST_GET_LAUNCHER_BUILD);
            // version string usually have N.N.N.N format and have length ~7 chars
            if (ver.Length > 8 | ver.Length == 0)
            {
                Log("Не удалось получить актуальную версию");
                return;
            }

            if (GetAssemblyVersion() != ver)
                Log("Ваш лаунчер устарел");
            // add download & replace function
            else
                Log("Лаунчер последней версии");
        }
        internal string GetAssemblyVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Settings.Default.nickname;

            mClientManager.onClientDataReady += (a, b) => {
                versionList.Items.Clear();
                foreach (Version v in mClientManager.GetVersionData().versions)
                {
                    versionList.Items.Add(v.mVersionName + " - " + v.mVersionDescription);
                }
            };

            mClientManager.ValidateDependencies();
            mClientManager.LoadUpVersions();

            configVersion.Text = "build " + GetAssemblyVersion();

            ValidateLauncherBuild();

            Log("Инициализация параметров - OK");
        }

        private string GetJavaInstallationPath()
        {
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return environmentPath;
            }

            string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
            {
                string currentVersion = rk.GetValue("CurrentVersion").ToString();
                using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                {
                    return key.GetValue("JavaHome").ToString();
                }
            }
        }

        private void InitClientLaunch(int cliId)
        {

            Log("Запускаем клиент");

            if (mClientManager.GetVersionData().versions.Count == 0)
                return;

            Version currentVersion = mClientManager.GetVersionData().versions[cliId];

            string clientPath = mClientManager.GetClientPath(currentVersion.mVersionArchiveName);
            string username = Settings.Default.nickname;
            string versionName = currentVersion.mVersionName;

            mClientLauncher = new MinecraftLauncher(clientPath, username, versionName, "2G", "5G");
            mClientLauncher.LaunchClient();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Match M = Regex.Match(e.KeyChar.ToString(), "^[a-zA-Z-0-9]*$");
            if (!M.Success & e.KeyChar != (char)Keys.Back & e.KeyChar != '_')
            {
                e.Handled = true;
            }
        }

        internal void ShowLoadingPanel(bool show)
        {
            loadingPanel.Invoke(new MethodInvoker(() => {
                loadingPanel.Visible = show;
            }));
        }

        internal void SetLoadingProgress(int progress)
        {
            progressBar.Invoke(new MethodInvoker(() => {
                progressBar.Value = progress;
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            //mClientManager.LoadOutVersions();
            AsyncWorker.Dispose();
            Thread.Sleep(1000);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.nickname = textBox1.Text;
            Settings.Default.Save();
        }

        private void versionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (versionList.SelectedIndex > -1)
            {
                if (mClientManager.GetVersionData() != null)
                {

                    Version selectedVersion = mClientManager.GetVersionData().versions[versionList.SelectedIndex];

                    // dev client can't be installed right now 

                    if (selectedVersion.GetValidated())
                        SwitchToLaunch();
                    else
                        SwitchToCheck();

                    if (EnabledByCheckComplete)
                        button1.Enabled = true;

                    if (ProhibitedClients.Contains(selectedVersion.mVersionArchiveName))
                    {
                        button1.Enabled = false;
                        button1.Text = "Недоступно";
                        CanEnableMainButton = false;
                    }
                    else
                        CanEnableMainButton = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!Launching)
            {
                if (versionList.SelectedIndex != -1)
                {
                    Log("Начата проверка клиента - пожалуйста, подождите. Окно может зависнуть - это нормально.", true);
                    mClientManager.ValidateGameClient(mClientManager.GetVersionData().versions[versionList.SelectedIndex]);
                }
            }
            else
                InitClientLaunch(versionList.SelectedIndex);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // show context menu
            ContextMenuStrip CMS = new ContextMenuStrip();

            string reinstallClient = "Переустановить клиент";
            string deleteClient = "Удалить клиент";

            CMS.Items.Add("Переустановить клиент");
            CMS.Items.Add("Удалить клиент");
            CMS.ItemClicked += (a, b) => {
                if (versionList.SelectedIndex != -1 & versionList.SelectedIndex <= mClientManager.GetVersionData().versions.Count) {
                    Version V = mClientManager.GetVersionData().versions[versionList.SelectedIndex];
                    if (b.ClickedItem.Text == reinstallClient)
                    {
                        mClientManager.InstallClient(V);
                        SwitchToCheck();
                    }
                    
                    if (b.ClickedItem.Text == deleteClient)
                    {
                        mClientManager.DeleteClient(V);
                        SwitchToCheck();
                    }
                }

            };
            CMS.Show(PointToScreen(ctxButton.Location));
        }
    }
}

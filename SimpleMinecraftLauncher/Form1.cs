using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SimpleMinecraftLauncher.Properties;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.ComponentModel;

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
        private bool NotificationLock = false;
        private Color loggerDefaultTextColor = Color.FromArgb(255, 153, 180, 209);
        private Color notifyDefaultBackgroundColor = Color.FromArgb(255, 40, 40, 50);
        private MinecraftLauncher mClientLauncher;
        private ClientManager mClientManager = new ClientManager();

        public Form1()
        {
            InitializeComponent();
            button1.Text = "Проверить клиент";

            TriggerLogger();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Settings.Default.nickname;

            mClientManager.onClientDataReady += (a, b) => {
                versionList.Items.Clear();
                foreach (Version version in mClientManager.GetVersionData().versions)
                {
                    versionList.Items.Add(version.mVersionName + " - " + version.mVersionDescription);
                }
            };

            string tempPath = CurrentDirectory + @"\temp";
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);

            mClientManager.ValidateDependencies();
            mClientManager.LoadUpVersions();

            configVersion.Text = "build " + GetAssemblyVersion();

            ValidateLauncherBuild();

            Log("Базовая инициализация лаунчера - ОК", false, Color.DarkOliveGreen);

            TryJava();
        }

        public void Log(string l, bool clear=false, Color? highlight=null)
        {
            try
            {
                logBox.BeginInvoke(new MethodInvoker(() =>
                {
                    if (clear)
                        logBox.Clear();
                    if (highlight != null)
                        logBox.SelectionColor = (Color)highlight;
                    logBox.AppendText(l + '\n');
                    logBox.SelectionColor = loggerDefaultTextColor;
                }));
            }
            catch (Exception ex) { }
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

        internal bool ValidateNickname()
        {
            return textBox1.Text.Length >= 3 & Regex.IsMatch(textBox1.Text, Constants.Util.MINECRAFT_NICKNAME_REGEX);
        }

        internal void SwitchToCheck()
        {
            Launching = false;
            Invoke(new MethodInvoker(() => {
                button1.Text = "Проверить клиент";
            }));
        }

        internal void SelectFirstAvailableVersion()
        {
            if (mClientManager.GetVersionData().versions.Count > 0)
                versionList.SelectedIndex = 0;
        }

        internal void SwitchToLaunch()
        {
            Launching = true;
            Invoke(new MethodInvoker(() => {
                button1.Text = "Запуск";
            }));
        }

        private void bMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        internal void ShowLogger()
        {
            logBox.Height = 243;
            loggerLabel.Location = new Point(497, 90);
        }

        internal void HideLogger()
        {
            logBox.Height = 0;
            loggerLabel.Location = new Point(497, 336);
        }

        internal void EnableVersionControls()
        {
            Invoke(new MethodInvoker(() => {
                button1.Enabled = true;
                ctxButton.Enabled = true;
                versionList.Enabled = true;
            }));
        }

        internal void DisableVersionControls()
        {
            Invoke(new MethodInvoker(() => {
                button1.Enabled = false;
                ctxButton.Enabled = false;
                versionList.Enabled = false;
            }));
        }

        internal void ShowNotification(string text, bool important=false)
        {
            if (NotificationLock)
                return;
            Invoke(new MethodInvoker(() => {
                notifyPanel.Visible = true;
                notifyInfoText.Text = text;

                    NotificationLock = important;

                if (important)
                {
                    notifyPanel.BackColor = Color.IndianRed;
                    SystemSounds.Beep.Play();
                }
            }));
        }
        private int NumberFromVersion(string ver)
        {
            return int.Parse(ver.Replace(".", string.Empty), 0);
        }
        private async void ValidateLauncherBuild()
        {
            string ver = await AsyncWorker.MakeHTTPRequest(Constants.Web.HOST_GET_LAUNCHER_BUILD);
            bool Match = Regex.IsMatch(ver, Constants.Util.LAUNCHER_VERSION_REGEX);

            if (!Match)
            {
                Log("Не удалось получить актуальную версию", false, Color.Red);
                ShowNotification("Не удаётся установить соединение с сервером лаунчера");
                return;
            }
            else
            {
                if (NumberFromVersion(ver) > NumberFromVersion(GetAssemblyVersion()))
                {
                    Log("Ваш лаунчер устарел", true, Color.DarkRed);
                    ShowNotification("Доступно обновление лаунчера " + ver + "! Для обновления нажмите кнопку Обновить лаунчер.");
                    updateBtn.Visible = true;
                }
                else
                    Log("Лаунчер последней версии", true, Color.LightGreen);
            }
        }
        internal string GetAssemblyVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }

        private void TryJava()
        {
            try
            {
                ProcessStartInfo minecraft = new ProcessStartInfo
                {
                    FileName = "javaw",
                    CreateNoWindow = true
                };
                Process minecraftProcess = Process.Start(minecraft);
                Log("Проверка Java - успешно (" + minecraftProcess.Id + ")", false, Color.MediumPurple);
                minecraftProcess.Close();

            }
            catch (Exception ex)
            {
                Log("Не удалось запустить клиент - " + ex.Message);

                if (ex is Win32Exception)
                {
                    UIManager.ShowNotification("Отсутствует Java! Проверьте установку Java и попробуйте еще раз.", true);
                    MessageBox.Show("Не удалось найти Java, проверьте установку. Это может стать причиной ошибки запуска Minecraft", "Установите Java!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (mClientLauncher.CanLaunch)
                mClientLauncher.LaunchClient();
            else
                MessageBox.Show("Клиент повреждён. Отсутствуют библиотеки. Запуск невозможен", "Ошибка запуска Minecraft", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Match M = Regex.Match(e.KeyChar.ToString(), Constants.Util.MINECRAFT_NICKNAME_REGEX);
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
            AsyncWorker.Dispose();
            Application.Exit();
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

                    if (!selectedVersion.mVersionEnabled)
                    {
                        ctxButton.Enabled = false;
                        CanEnableMainButton = false;
                        button1.Enabled = false;
                        button1.Text = "Недоступно";
                    }
                    else
                    {
                        ctxButton.Enabled = true;
                        CanEnableMainButton = true;
                    }
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
                    mClientManager.ValidateGameClient(mClientManager.GetVersionData().versions[versionList.SelectedIndex], true);
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
            string deleteClientArchive = "Удалить архив клиента";

            CMS.Items.Add(reinstallClient);
            CMS.Items.Add(deleteClient);
            CMS.Items.Add(deleteClientArchive);
            CMS.ItemClicked += (a, b) => {
                // if selected index of menu is valid
                if (versionList.SelectedIndex != -1 & versionList.SelectedIndex <= mClientManager.GetVersionData().versions.Count) {
                    Version version = mClientManager.GetVersionData().versions[versionList.SelectedIndex];

                    if (b.ClickedItem.Text == reinstallClient)
                    {
                        mClientManager.InstallClient(version, () => {
                            SwitchToLaunch();
                        });
                    }

                    if (b.ClickedItem.Text == deleteClient)
                    {
                        mClientManager.DeleteClient(version, () =>
                        {
                            SwitchToCheck();
                        });

                    }

                    if (b.ClickedItem.Text == deleteClientArchive)
                    {
                        mClientManager.DeleteArchive(version, () =>
                        {
                            SwitchToCheck();
                        });

                    }
                }

            };
            CMS.Show(PointToScreen(ctxButton.Location));
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Settings.Default.logger_show = !Settings.Default.logger_show;
            Settings.Default.Save();
            TriggerLogger();
        }

        private void TriggerLogger()
        {
            if (Settings.Default.logger_show)
                ShowLogger();
            else
                HideLogger();
        }

        private void infoBtnClose_Click(object sender, EventArgs e)
        {
            notifyPanel.Visible = false;
            notifyInfoText.Text = string.Empty;
            NotificationLock = false;
            notifyPanel.BackColor = notifyDefaultBackgroundColor;
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            mClientManager.UpdateLauncher();
            updateBtn.Enabled = false;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(button2, "Никнейм может состоять только из символов английского алфавита, цифр от 0 до 9 и нижнего подчёркивания. Минимум 3 символа!");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            button2_MouseEnter(sender, e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleMinecraftLauncher.Properties;

namespace SimpleMinecraftLauncher
{
    public partial class Form1 : Form
    {
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        ClientManager mClientManager = new ClientManager();

        public Form1()
        {
            InitializeComponent();
        }


        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void SetInitialParams()
        {
            buildVersion.Text = "Launcher build version " + Settings.Default.build_version;
        }

        private void windowTopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            mClientManager.ValidateDependencies(() => {
                mClientManager.InitVersions();
            });

            SetInitialParams();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Match M = Regex.Match(e.KeyChar.ToString(), "^[a-zA-Z-0-9]*$");
            if (!M.Success & e.KeyChar != (char)Keys.Back & e.KeyChar != '_')
            {
                e.Handled = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMinecraftLauncher
{
    public static class UIManager
    {

        internal static Form1 MainForm()
        {
            return FormManager.GetMainForm();
        }
        internal static void DisableUpdateButton()
        {
            MainForm().updateBtn.Visible = false;
        }
        internal static void LogMessage(string message, bool deleteAllprevious=false, Color? highlight=null)
        {
            if (MainForm() == null)
                return;
            MainForm().Log(message, deleteAllprevious, highlight);
        }

        internal static void EnableControls()
        {
            MainForm().EnableVersionControls();
        }

        internal static void ShowNotification(string text, bool important=false)
        {
            MainForm().ShowNotification(text, important);
        }

        internal static void DisableControls()
        {
            MainForm().DisableVersionControls();
        }

        internal static bool CanEnableMainButton()
        {
            return MainForm().CanEnableMainButton;
        }

        internal static void EnableMainButton()
        {
            MainForm().button1.Enabled = true;
        }

        internal static void DisableMainButton()
        {
            MainForm().button1.Enabled = false;
        }

        internal static void EnableByCheckComplete()
        {
            MainForm().EnabledByCheckComplete = true;
        }

        internal static void DisableByCheckComplete()
        {
            MainForm().EnabledByCheckComplete = false;
        }

        internal static void SetProgress(int progress)
        {
            MainForm().SetLoadingProgress(progress);
        }

        internal static void ShowLoading()
        {
            MainForm().ShowLoadingPanel(true);
        }

        internal static void HideLoading()
        {
            MainForm().ShowLoadingPanel(false);
        }

        internal static void InvokeFormShow()
        {
            MainForm().Invoke(new MethodInvoker(() => {
                MainForm().Show();
                MainForm().BringToFront();
                FlashWindow.FlashWindowEx(MainForm());
                MainForm().EnableVersionControls();
            }));
        }

        internal static void InvokeFormHide()
        {
            MainForm().Invoke(new MethodInvoker(() => {
                MainForm().Hide();
            }));
        }

        internal static void SelectFirstAvailableVersion()
        {
            MainForm().SelectFirstAvailableVersion();
        }

    }
}

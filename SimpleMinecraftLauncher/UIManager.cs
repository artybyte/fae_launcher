using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinecraftLauncher
{
    public static class UIManager
    {

        internal static void LogMessage(string message, bool deleteAllprevious = false)
        {
            if (FormManager.GetMainForm() == null)
                return;
            FormManager.GetMainForm().Log(message, deleteAllprevious);
        }

        internal static void EnableControls()
        {
            FormManager.GetMainForm().EnableVersionControls();
            LogMessage("ENABLED!!!!!!!!!!");
        }

        internal static void DisableControls()
        {
            FormManager.GetMainForm().DisableVersionControls();
            LogMessage("DISABLED!!!!!!!!!!");
        }

        internal static bool CanEnableMainButton()
        {
            return FormManager.GetMainForm().CanEnableMainButton;
        }

        internal static void EnableMainButton()
        {
            FormManager.GetMainForm().button1.Enabled = true;
        }

        internal static void DisableMainButton()
        {
            FormManager.GetMainForm().button1.Enabled = false;
        }

        internal static void EnableByCheckComplete()
        {
            FormManager.GetMainForm().EnabledByCheckComplete = true;
        }

        internal static void DisableByCheckComplete()
        {
            FormManager.GetMainForm().EnabledByCheckComplete = false;
        }

        internal static void SetProgress(int progress)
        {
            FormManager.GetMainForm().SetLoadingProgress(progress);
        }

        internal static void ShowLoadingPanel()
        {
            FormManager.GetMainForm().ShowLoadingPanel(true);
        }

        internal static void HideLoadingPanel()
        {
            FormManager.GetMainForm().ShowLoadingPanel(false);
        }

        internal static void SwitctToLaunch()
        {
            FormManager.GetMainForm().SwitchToLaunch();
        }
        internal static void SwitchToCheck()
        {
            FormManager.GetMainForm().SwitchToCheck();
        }
        internal static void SelectFirstAvailableVersion()
        {
            FormManager.GetMainForm().SelectFirstAvailableVersion();
        }

    }
}

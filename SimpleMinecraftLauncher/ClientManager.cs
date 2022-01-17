using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace SimpleMinecraftLauncher
{
    internal class ClientManager
    {

        public string CurrentDirectory = "";

        public ClientManager(){
            CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        VersionControl mVersionController = new VersionControl();

        public void LoadVersionData() { }

        public void InitVersions()
        {

            string VersionDataJSON = File.ReadAllText(CurrentDirectory+Constants.JSON_CONFIG_PATH);
            MessageBox.Show("data: " + VersionDataJSON);

        }
        /// <summary>
        /// checks existance of clients folder and JSON config file
        /// </summary>
        public void ValidateDependencies(Action onJSONCreated=null)
        {
            if (!Directory.Exists(Constants.MINECRAFT_CLIENTS_PATH))
                Directory.CreateDirectory(Constants.MINECRAFT_CLIENTS_PATH);

            if (!File.Exists(CurrentDirectory + Constants.JSON_CONFIG_PATH))
                Task.Run(action: async () =>
                {
                    await Task.Run(() => {
                        FileStream FS = File.Create(CurrentDirectory + Constants.JSON_CONFIG_PATH);
                        if (onJSONCreated != null)
                            onJSONCreated();
                    }).ContinueWith(async (a) => { onJSONCreated(); });
                });
        }

    }
}

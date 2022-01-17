using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace SimpleMinecraftLauncher
{
    internal static class VersionControlDeserializer
    {
        public static string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Builds abstract VersionControl class from JSON string
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public static VersionControl AssembleVersionControl(string JSON)
        {

            VersionControl vc = new VersionControl();
            vc.SetVersions(new List<Version>() { });

            try
            {
                VersionControl VC = JsonConvert.DeserializeObject<VersionControl>(JSON);
                if (VC != null)
                {
                    vc = VC;
                    FormManager.GetMainForm().Log("Чтение файла конфигурации - OK");
                }
            }
            catch(Exception ex)
            {
                if (ex is JsonReaderException || ex is NullReferenceException)
                    FormManager.GetMainForm().Log("Чтение файла конфигурации - ошибка");
            }
            return vc;
        }
        /// <summary>
        /// Decompiles abstract VersionControl to JSON string
        /// </summary>
        /// <param name="VC"></param>
        /// <returns></returns>
        public static string DisassembleVersionControl(VersionControl VC)
        {
            VC.UpdateChecksum();
            return JsonConvert.SerializeObject(VC, Formatting.Indented);
        }
    }
}

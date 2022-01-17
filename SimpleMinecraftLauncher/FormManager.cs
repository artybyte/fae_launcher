using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMinecraftLauncher
{
    internal class FormManager
    {

        public static Form1 GetMainForm() {
            if (Application.OpenForms.Count == 0)
                return new Form1();
            else
                return Application.OpenForms.Cast<Form1>().Last();
        }
    }
}

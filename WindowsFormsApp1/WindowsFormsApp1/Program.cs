using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AddIconToRemovePrograms("ts");
            Application.Run(new Form1());
        }
        static string IconPath => Path.Combine(Application.StartupPath, "imgcompany.ico");

        internal static void AddIconToRemovePrograms(string productName)
    {
        try
        {   //first run after first install or after update
            if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                if (File.Exists(IconPath))
                {
                    var uninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                    var subKeyNames = uninstallKey.GetSubKeyNames();
                    for (int i = 0; i < subKeyNames.Length; i++)
                    {
                        using (var key = uninstallKey.OpenSubKey(subKeyNames[i], true))
                        {
                            var displayName = key?.GetValue("DisplayName");
                            if (displayName == null || displayName.ToString() != productName)
                                continue;
                            //Set this to the display name of the application.
                            key.SetValue("DisplayIcon", IconPath);
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"We could not properly setup the application icons. Please notify your software vendor of this error.{Environment.NewLine}{ex.ToString()}", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    }
}

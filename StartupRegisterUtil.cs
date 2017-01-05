using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class StartupRegisterUtil
    {
        public static void RegisterToSystem()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (!IsStartupRegistered())
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("WebScrapping", Application.ExecutablePath.ToString());
            }
        }

        public static void RemoveRegistry()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (IsStartupRegistered())
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("WebScrapping", false);
        }

        public static bool IsStartupRegistered()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue("WebScrapping") == null)
                // The value doesn't exist, the application is not set to run at startup
                return false;
            else
                // The value exists, the application is set to run at startup
                return true;
        }

    }
}

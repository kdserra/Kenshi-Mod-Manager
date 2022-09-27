using System;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModManagerForm1());
        }
    }
}

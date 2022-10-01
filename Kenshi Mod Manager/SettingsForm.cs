using System.IO;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.Select();
            this.Focus();
            kenshiDirTextBox.Text = Settings.Default.KENSHI_DIRECTORY;
            kenshiModDirTextBox.Text = Settings.Default.KENSHI_MOD_DIRECTORY;
        }

        private void kenshiDirOpenButton_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    kenshiDirTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void kenshiModDirOpenButton_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    kenshiModDirTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            Settings.Default.KENSHI_DIRECTORY = kenshiDirTextBox.Text;
            Settings.Default.KENSHI_MOD_DIRECTORY = kenshiModDirTextBox.Text;
            Settings.Default.Save();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Kenshi_Mod_Manager
{
    public partial class ModManagerForm : Form
    {
        private readonly Color FocusColor = Color.FromArgb(242, 242, 242);
        private readonly Color UnfocusedColor = Color.FromArgb(151, 151, 151);
        private readonly Color PrimaryGrey = Color.FromArgb(51, 51, 51);
        private readonly Color NonprimaryGrey = Color.FromArgb(25, 25, 25);
        private List<ModEntry> m_ActiveModEntryList = new List<ModEntry>() { };
        private List<ModEntry> m_InactiveModEntryList = new List<ModEntry>() { };
        private ActiveTab m_CurrentActiveTab = ActiveTab.ActiveMods;

        public ActiveTab CurrentActiveTab
        {
            get { return m_CurrentActiveTab; }
            set
            {
                if (m_CurrentActiveTab == value) { return; }
                m_CurrentActiveTab = value;
                SyncModTab();
                SyncModEntryTable();
            }
        }

        public List<ModEntry> CurrentModEntryList
        {
            get
            {
                switch (CurrentActiveTab)
                {
                    case ActiveTab.ActiveMods: return m_ActiveModEntryList;
                    case ActiveTab.InactiveMods: return m_InactiveModEntryList;
                    default: return new List<ModEntry>() { };
                }
            }
        }

        public ModManagerForm()
        {
            InitializeComponent();
            this.Select();
            this.Focus();
            Init();
            SyncModTab();
            SyncModEntryTable();
        }

        private async void Init()
        {
            m_InactiveModEntryList = await FetchModsFromDisk();
        }

        private void AddToModEntryTable(ModEntry modEntry)
        {
            CurrentModEntryList.Add(modEntry);
            SyncModEntryTable();
        }

        private bool RemoveFromModEntryTable(ModEntry modEntry)
        {
            bool removed = CurrentModEntryList.Remove(modEntry);
            if (removed) { SyncModEntryTable(); }
            return removed;
        }

        private static async Task<List<ModEntry>> FetchModsFromDisk()
        {
            string modDirectoryPath = Settings.Default.KENSHI_MOD_DIRECTORY;

            /*
            // Uri.IsWellFormedUriString doesn't work as expected (i think it's an issue with back slashes)
            if (!Uri.IsWellFormedUriString(modDirectoryPath, UriKind.Absolute))
            {
                MessageBox.Show(
                    "Failed to fetch mods from disk. Check that the Kenshi Mod Directory Path is setup correctly.",
                    "Kenshi Mod Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                return new List<ModEntry>(){ };
            }
            */

            List<ModEntry> modEntries = new List<ModEntry>() { };
            string steamCommunityFileIDURL = "https://steamcommunity.com/sharedfiles/filedetails/?id=";
            string[] subDirectories = Directory.GetDirectories(modDirectoryPath);
            foreach (string subDirectory in subDirectories)
            {
                string[] fileNames = Directory.GetFiles(subDirectory, "*.info", SearchOption.TopDirectoryOnly);
                if (fileNames == null || fileNames.Length == 0) { continue; }
                string fileName = fileNames[0];
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(fileName);
                    XmlElement xmlE = xmlDoc.DocumentElement;

                    //string id = "";
                    string modTitle = "";
                    List<string> modCategories = new List<string>() { };
                    foreach (XmlNode v in xmlE.ChildNodes)
                    {
                        string s = v.Name;
                        //if (s == "id") { id = v.InnerText; }
                        if (s == "title") { modTitle = v.InnerText; }
                        if (s == "tags") 
                        {
                            foreach (XmlNode xmlNode in v.ChildNodes)
                            {
                                modCategories.Add(xmlNode.InnerText);
                            }
                        }
                    }
                    //Image image = await Utilities.GetSteamWorkshopThumbnail(id);
                    //if (image == null) { continue; }
                    string modFileName = Path.GetFileNameWithoutExtension(fileName)[1..] + ".mod";

                    ModEntry modEntry = new ModEntry(
                        name: modTitle,
                        fileName: modFileName,
                        image: null,
                        categories: modCategories.ToArray(),
                        source: ModSource.Steam
                        );

                    modEntries.Add(modEntry);
                }
                catch { continue; }
            }

            return modEntries;
        }

        private void SyncModEntryTable()
        {
            if (CurrentModEntryList != null)
            {
                SuspendLayout();
                modEntryTableLayoutPanel.SuspendLayout();
                modEntryTableLayoutPanel.ClearRows();
                foreach (ModEntry modEntry in CurrentModEntryList)
                {
                    AddRowToModEntryTable(modEntry);
                }
                modEntryTableLayoutPanel.ResumeLayout(false);
                ResumeLayout(false);
                modEntryTableLayoutPanel.PerformLayout();
                PerformLayout();
            }
        }

        private void SyncModTab()
        {
            switch (CurrentActiveTab)
            {
                case ActiveTab.ActiveMods:
                    activeModsButton.ForeColor = FocusColor;
                    inactiveModsButton.ForeColor = UnfocusedColor;
                    break;
                case ActiveTab.InactiveMods:
                    activeModsButton.ForeColor = UnfocusedColor;
                    inactiveModsButton.ForeColor = FocusColor;
                    break;
            }
        }

        private void AddRowToModEntryTable(ModEntry modEntry)
        {
            Panel modPanel = new Panel();
            Panel modPictureBoxPanel = new Panel();
            TableLayoutPanel modTableLayoutPanel = new TableLayoutPanel();
            PictureBox modPictureBox = new PictureBox();
            TableLayoutPanel modInfoTableLayoutPanel = new TableLayoutPanel();
            Label modFileNameLabel = new Label();
            Label modNameLabel = new Label();
            Label modCategoryLabel = new Label();
            Label modSourceLabel = new Label();
            Button modStateButton = new Button();

            RowStyle rowStyle = new RowStyle(SizeType.Absolute, 50.0f);
            RowStyle fillRowStyle = new RowStyle(SizeType.AutoSize);

            int rowIndex = modEntryTableLayoutPanel.AddRow(rowStyle);
            modEntryTableLayoutPanel.AddRow(fillRowStyle);

            modNameLabel.Text = modEntry.Name;
            modFileNameLabel.Text = modEntry.FileName;
            modCategoryLabel.Text = modEntry.GetCategoriesString();
            modSourceLabel.Text = modEntry.Source.ToString();
            modStateButton.Text = "Toggle";
            modPictureBox.Image = modEntry.Image;

            ((System.ComponentModel.ISupportInitialize)(modPictureBox)).BeginInit();

            #region spam
            // 
            // modEntryTableLayoutPanel
            // 
            modEntryTableLayoutPanel.Controls.Add(modPanel, 0, rowIndex);
            modEntryTableLayoutPanel.Controls.Add(modCategoryLabel, 1, rowIndex);
            modEntryTableLayoutPanel.Controls.Add(modSourceLabel, 2, rowIndex);
            modEntryTableLayoutPanel.Controls.Add(modStateButton, 3, rowIndex);
            // modPanel
            modPanel.Controls.Add(modTableLayoutPanel);
            modPanel.Dock = DockStyle.Fill;
            modPanel.Location = new Point(0, 60);
            modPanel.Margin = new Padding(0);
            modPanel.Name = "modPanel";
            modPanel.Size = new Size(245, 50);
            modPanel.TabIndex = 5;
            // 
            // modPictureBoxPanel
            // 
            modPictureBoxPanel.BackColor = Color.FromArgb(51, 51, 51);
            modPictureBoxPanel.Controls.Add(modPictureBox);
            modPictureBoxPanel.Dock = DockStyle.Fill;
            modPictureBoxPanel.Location = new Point(0, 0);
            modPictureBoxPanel.Margin = new Padding(0);
            modPictureBoxPanel.Size = new Size(50, 50);
            modPictureBoxPanel.TabIndex = 0;
            modPictureBoxPanel.Controls.Add(modPictureBox);
            // modTableLayoutPanel
            modTableLayoutPanel.ColumnCount = 2;
            modTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            modTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            modTableLayoutPanel.Controls.Add(modPictureBoxPanel, 0, 0);
            modTableLayoutPanel.Controls.Add(modInfoTableLayoutPanel, 1, 0);
            modTableLayoutPanel.Dock = DockStyle.Fill;
            modTableLayoutPanel.Location = new Point(0, 0);
            modTableLayoutPanel.RowCount = 1;
            modTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            modTableLayoutPanel.Size = new Size(245, 50);
            modTableLayoutPanel.TabIndex = 0;
            // modPictureBox
            modPictureBox.Dock = DockStyle.Fill;
            modPictureBox.Location = new Point(0, 0);
            modPictureBox.Margin = new Padding(0);
            modPictureBox.Size = new Size(50, 50);
            modPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            modPictureBox.TabIndex = 1;
            modPictureBox.TabStop = false;
            // modInfoTableLayoutPanel
            modInfoTableLayoutPanel.ColumnCount = 1;
            modInfoTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            modInfoTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            modInfoTableLayoutPanel.Controls.Add(modFileNameLabel, 0, 1);
            modInfoTableLayoutPanel.Controls.Add(modNameLabel, 0, 0);
            modInfoTableLayoutPanel.Dock = DockStyle.Fill;
            modInfoTableLayoutPanel.Location = new Point(50, 0);
            modInfoTableLayoutPanel.Margin = new Padding(0);
            modInfoTableLayoutPanel.RowCount = 2;
            modInfoTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            modInfoTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            modInfoTableLayoutPanel.Size = new Size(195, 50);
            modInfoTableLayoutPanel.TabIndex = 1;
            // modFileNameLabel
            modFileNameLabel.BackColor = Color.FromArgb(51, 51, 51);
            modFileNameLabel.Dock = DockStyle.Fill;
            modFileNameLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            modFileNameLabel.ForeColor = Color.Silver;
            modFileNameLabel.Location = new Point(0, 25);
            modFileNameLabel.Margin = new Padding(0);
            modFileNameLabel.Size = new Size(195, 25);
            modFileNameLabel.TabIndex = 3;
            modFileNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // modNameLabel
            modNameLabel.BackColor = Color.FromArgb(51, 51, 51);
            modNameLabel.Dock = DockStyle.Fill;
            modNameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            modNameLabel.ForeColor = Color.FromArgb(242, 242, 242);
            modNameLabel.Location = new Point(0, 0);
            modNameLabel.Margin = new Padding(0);
            modNameLabel.Size = new Size(195, 25);
            modNameLabel.TabIndex = 2;
            modNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            // modCategoryLabel
            modCategoryLabel.BackColor = Color.FromArgb(51, 51, 51);
            modCategoryLabel.Dock = DockStyle.Fill;
            modCategoryLabel.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            modCategoryLabel.ForeColor = Color.FromArgb(242, 242, 242);
            modCategoryLabel.Location = new Point(245, 60);
            modCategoryLabel.Margin = new Padding(0);
            modCategoryLabel.Size = new Size(250, 50);
            modCategoryLabel.TabIndex = 6;
            modCategoryLabel.TextAlign = ContentAlignment.MiddleLeft;
            // modSourceLabel
            modSourceLabel.BackColor = Color.FromArgb(51, 51, 51);
            modSourceLabel.Dock = DockStyle.Fill;
            modSourceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            modSourceLabel.ForeColor = Color.FromArgb(242, 242, 242);
            modSourceLabel.Location = new Point(495, 60);
            modSourceLabel.Margin = new Padding(0);
            modSourceLabel.Size = new Size(100, 50);
            modSourceLabel.TabIndex = 7;
            modSourceLabel.TextAlign = ContentAlignment.MiddleLeft;
            // modStateButton
            modStateButton.Dock = DockStyle.Fill;
            modStateButton.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            modStateButton.FlatStyle = FlatStyle.Flat;
            modStateButton.Font = new Font("Corbel", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            modStateButton.ForeColor = Color.FromArgb(242, 242, 242);
            modStateButton.Location = new Point(595, 60);
            modStateButton.Margin = new Padding(0);
            modStateButton.Size = new Size(100, 50);
            modStateButton.TabIndex = 8;
            modStateButton.UseVisualStyleBackColor = true;
            #endregion

            ((System.ComponentModel.ISupportInitialize)(modPictureBox)).EndInit();
        }

        private void activeModsButton_Click(object sender, EventArgs e)
        {
            CurrentActiveTab = ActiveTab.ActiveMods;
        }

        private void inactiveModsButton_Click(object sender, EventArgs e)
        {
            CurrentActiveTab = ActiveTab.InactiveMods;
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            Form form = new SettingsForm();
            form.ShowDialog();
        }

        private void newProfileButton_Click(object sender, EventArgs e)
        {

        }

        private void saveProfileButton_Click(object sender, EventArgs e)
        {

        }

        private void loadProfileButton_Click(object sender, EventArgs e)
        {

        }

        private void importProfileButton_Click(object sender, EventArgs e)
        {

        }

        private void exportButton_Click(object sender, EventArgs e)
        {

        }

        private void saveToKenshiButton_Click(object sender, EventArgs e)
        {
        }

        private void orderModsButton_Click(object sender, EventArgs e)
        {
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {

        }
    }
}

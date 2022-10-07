﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static string GetKenshiModConfigFilePath()
        {
            return Path.Combine(Settings.Default.KENSHI_DIRECTORY, "data", "mods.cfg");
        }

        public static string GetProfilePath()
        {
            return Path.Combine(Application.StartupPath, "Profiles");
        }

        public List<ModEntry> AllModEntriesList
        {
            get
            {
                List<ModEntry> allModEntriesList = new List<ModEntry>() { };
                allModEntriesList.AddRange(m_ActiveModEntryList);
                allModEntriesList.AddRange(m_InactiveModEntryList);
                return allModEntriesList;
            }
        }

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
            SyncModTab();
            Init();
        }

        private async void Init()
        {
            m_InactiveModEntryList = await FetchModsFromDisk();
            SyncModEntryTable();
            CacheModIcons();
        }

        private ModEntry GetModEntryFromID(string id)
        {
            foreach (ModEntry modEntry in AllModEntriesList)
            {
                if (modEntry.Id != id) { continue; }
                return modEntry;
            }
            return null;
        }

        private string GetModIconCacheDirectory()
        {
            return Path.Combine(Application.StartupPath, "ModIconCache");
        }

        private void CacheModIcons()
        {
            foreach (ModEntry modEntry in AllModEntriesList)
            {
                if (modEntry.Image == null) { continue; }
                if (IsModIconCached(modEntry.Id)) { continue; }
                string dir = GetModIconCacheDirectory();
                if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                string imageFilePath = Path.Combine(dir, modEntry.Id + ".png");
                modEntry.Image.Save(imageFilePath, ImageFormat.Png);
            }
        }

        private bool IsModIconCached(string id)
        {
            string filePath = Path.Combine(GetModIconCacheDirectory(), id + ".png");
            bool isModIconCached = File.Exists(filePath);
            return isModIconCached;
        }

        private Image GetCachedModIcon(string id)
        {
            string filePath = Path.Combine(GetModIconCacheDirectory(), id + ".png");
            if (!File.Exists(filePath)) { return null; }
            Image image = Image.FromFile(filePath).Resize(new Size(100, 100));
            return image;
        }

        private async Task<List<ModEntry>> FetchModsFromDisk()
        {
            string modDirectoryPath = Settings.Default.KENSHI_MOD_DIRECTORY;

            if (!Directory.Exists(modDirectoryPath))
            {
                MessageBox.Show(
                    "Failed to fetch mods from disk. Check that the Kenshi Mod Directory Path is setup correctly.",
                    "Kenshi Mod Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                return new List<ModEntry>() { };
            }

            List<ModEntry> modEntries = new List<ModEntry>() { };
            string[] subDirectories = Directory.GetDirectories(modDirectoryPath);
            foreach (string subDirectory in subDirectories)
            {
                string directoryName = new DirectoryInfo(subDirectory).Name;
                string[] fileNames = Utilities.GetFiles(subDirectory, new Regex(@"_.*\.info"), SearchOption.TopDirectoryOnly);
                Array.Sort(fileNames);

                if (fileNames == null || fileNames.Length == 0) { continue; }
                XmlDocument xmlDoc = new XmlDocument();
                foreach (string fileName in fileNames)
                {
                    try
                    {
                        xmlDoc.Load(fileName);
                        XmlElement xmlE = xmlDoc.DocumentElement;

                        string id = "";
                        string modTitle = "";
                        string modInfoName = "";
                        string kenshiFormatModFileName = "";
                        List<string> modCategories = new List<string>() { };
                        foreach (XmlNode v in xmlE.ChildNodes)
                        {
                            string s = v.Name;
                            if (s == "id")
                            {
                                if (v.InnerText != directoryName)
                                {
                                    goto CONTINUE_NEXT_FILE_NAME;
                                }
                                id = v.InnerText;
                            }
                            if (s == "mod")
                            {
                                modInfoName = v.InnerText;
                            }
                            if (s == "title") { modTitle = v.InnerText; }
                            if (s == "tags")
                            {
                                foreach (XmlNode xmlNode in v.ChildNodes)
                                {
                                    modCategories.Add(xmlNode.InnerText);
                                }
                            }
                        }

                        Image image = GetCachedModIcon(id);
                        if (image == null) { image = await Utilities.GetSteamWorkshopThumbnail(id); }
                        if (image == null) { continue; }

                        kenshiFormatModFileName = Path.GetFileNameWithoutExtension(fileNames[0])[1..] + ".mod";
                        string modFileName = Path.GetFileNameWithoutExtension(fileName)[1..] + ".mod";

                        ModEntry modEntry = new ModEntry(
                            id: id,
                            displayName: modTitle,
                            fileName: modFileName,
                            modInfoName: modInfoName,
                            kenshiFormatModFileName: kenshiFormatModFileName,
                            image: image,
                            categories: modCategories.ToArray()
                        );

                        modEntries.Add(modEntry);
                    CONTINUE_NEXT_FILE_NAME:;
                    }
                    catch { continue; }
                }
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
                modEntryTableLayoutPanel.Size = new Size(695, 246);
                foreach (ModEntry modEntry in CurrentModEntryList)
                {
                    #region Add Row To Mod Entry Table
                    Panel modPanel = new Panel();
                    Panel modPictureBoxPanel = new Panel();
                    TableLayoutPanel modTableLayoutPanel = new TableLayoutPanel();
                    PictureBox modPictureBox = new PictureBox();
                    TableLayoutPanel modInfoTableLayoutPanel = new TableLayoutPanel();
                    Label modFileNameLabel = new Label();
                    Label modNameLabel = new Label();
                    Label modCategoryLabel = new Label();
                    Label modIDLabel = new Label();
                    Button modStateButton = new Button();

                    RowStyle rowStyle = new RowStyle(SizeType.Absolute, 50.0f);
                    RowStyle fillRowStyle = new RowStyle(SizeType.AutoSize);

                    int rowIndex = modEntryTableLayoutPanel.AddRow(rowStyle);
                    modEntryTableLayoutPanel.AddRow(fillRowStyle);

                    modNameLabel.Text = modEntry.DisplayName;
                    modFileNameLabel.Text = modEntry.FileName;
                    modCategoryLabel.Text = modEntry.GetCategoriesString();
                    modIDLabel.Text = modEntry.Id;
                    modStateButton.Text = "Toggle";
                    modPictureBox.Image = modEntry.Image;

                    // 
                    // modEntryTableLayoutPanel
                    // 
                    modEntryTableLayoutPanel.Controls.Add(modPanel, 0, rowIndex);
                    modEntryTableLayoutPanel.Controls.Add(modCategoryLabel, 1, rowIndex);
                    modEntryTableLayoutPanel.Controls.Add(modIDLabel, 2, rowIndex);
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
                    // modIDLabel
                    modIDLabel.BackColor = Color.FromArgb(51, 51, 51);
                    modIDLabel.Dock = DockStyle.Fill;
                    modIDLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
                    modIDLabel.ForeColor = Color.FromArgb(242, 242, 242);
                    modIDLabel.Location = new Point(495, 60);
                    modIDLabel.Margin = new Padding(0);
                    modIDLabel.Size = new Size(100, 50);
                    modIDLabel.TabIndex = 7;
                    modIDLabel.TextAlign = ContentAlignment.MiddleLeft;
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
                    modStateButton.Click += new EventHandler(modStateButton_Click);
                    #endregion
                }
                modEntryTableLayoutPanel.ResumeLayout(false);
                ResumeLayout(false);
                modEntryTableLayoutPanel.PerformLayout();
                PerformLayout();
            }
            GC.Collect();
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

        private void modStateButton_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            TableLayoutPanelCellPosition controlCellPosition = modEntryTableLayoutPanel.GetPositionFromControl(control);
            Control idControl = modEntryTableLayoutPanel.GetControlFromPosition(controlCellPosition.Column - 1, controlCellPosition.Row);
            if (idControl == null) { return; }
            string id = idControl.Text;
            ModEntry modEntry = GetModEntryFromID(id);
            if (modEntry == null) { return; }
            switch (CurrentActiveTab)
            {
                case ActiveTab.ActiveMods:
                    m_ActiveModEntryList.Remove(modEntry);
                    m_InactiveModEntryList.Add(modEntry);
                    break;
                case ActiveTab.InactiveMods:
                    m_InactiveModEntryList.Remove(modEntry);
                    m_ActiveModEntryList.Add(modEntry);
                    break;
            }
            modEntryTableLayoutPanel.RowStyles[controlCellPosition.Row].Height = 0;
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

        private void saveProfileButton_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(GetProfilePath());
            string fileContent = "";
            foreach (ModEntry modEntry in m_ActiveModEntryList)
            {
                fileContent += modEntry.KenshiFormatModFileName + "\n";
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = GetProfilePath();
            saveFileDialog.Filter = "kmmprofile files (*.kmmprofile)|*.kmmprofile";
            saveFileDialog.Title = "Save Kenshi Mod Manager Profile";

            Stream stream;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    stream.Close();
                    File.WriteAllText(saveFileDialog.FileName, fileContent);
                }
            }
        }

        private async void loadProfileButton_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(GetProfilePath());
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = GetProfilePath();
            openFileDialog.Filter = "kmmprofile files (*.kmmprofile)|*.kmmprofile";
            openFileDialog.Title = "Open Kenshi Mod Manager Profile";

            Stream stream;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    stream.Close();
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    List<ModEntry> allModEntries = await FetchModsFromDisk();
                    List<ModEntry> bufferActiveModEntryList = new List<ModEntry>() { };
                    foreach (string line in fileContent.Split('\n'))
                    {
                        foreach (ModEntry mod in allModEntries)
                        {
                            if (mod.KenshiFormatModFileName != line) { continue; }
                            bufferActiveModEntryList.Add(mod);
                        }
                    }
                    m_ActiveModEntryList = bufferActiveModEntryList;
                    m_InactiveModEntryList = allModEntries.Except(bufferActiveModEntryList).ToList();
                    SyncModEntryTable();
                }
            }
        }

        private void importProfileButton_Click(object sender, EventArgs e)
        {

        }

        private void exportButton_Click(object sender, EventArgs e)
        {

        }

        private void saveToKenshiButton_Click(object sender, EventArgs e)
        {
            string kenshiModConfigFilePath = GetKenshiModConfigFilePath();
            string fileContent = "";
            foreach (ModEntry modEntry in m_ActiveModEntryList)
            {
                fileContent += modEntry.KenshiFormatModFileName + "\n";
            }
            File.WriteAllText(kenshiModConfigFilePath, fileContent);
        }

        public List<ModEntry> GetModsWithTags(List<ModEntry> modEntryList, params Tag[] tags)
        {
            List<ModEntry> output = new List<ModEntry>() { };
            foreach (Tag tag in tags)
            {
                foreach (ModEntry modEntry in modEntryList)
                {
                    if (output.Contains(modEntry)) { continue; }
                    Tag[] modEntryTags = modEntry.GetModTags();
                    if (!modEntryTags.Contains(tag)) { continue; }
                    output.Add(modEntry);
                }
            }
            output.AddRange(modEntryList.Except(output).ToList());
            return output;
        }

        private void orderModsButton_Click(object sender, EventArgs e)
        {
            if (CurrentActiveTab != ActiveTab.ActiveMods) { return; }
            List<ModEntry> orderedMods = GetModsWithTags(m_ActiveModEntryList,
                Kenshi_Mod_Manager.Tag.GUI,
                Kenshi_Mod_Manager.Tag.Graphical,
                Kenshi_Mod_Manager.Tag.Gameplay,
                Kenshi_Mod_Manager.Tag.Research,
                Kenshi_Mod_Manager.Tag.Cheats,
                Kenshi_Mod_Manager.Tag.Races,
                Kenshi_Mod_Manager.Tag.Characters,
                Kenshi_Mod_Manager.Tag.Factions,
                Kenshi_Mod_Manager.Tag.Buildings,
                Kenshi_Mod_Manager.Tag.Clothing_Or_Armour,
                Kenshi_Mod_Manager.Tag.Items_Or_Weapons,
                Kenshi_Mod_Manager.Tag.Total_Overhaul,
                Kenshi_Mod_Manager.Tag.Translation,
                Kenshi_Mod_Manager.Tag.None
            );
            m_ActiveModEntryList = orderedMods;
            SyncModEntryTable();
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            List<ModEntry> activeModEntryBuffer = new List<ModEntry>() { };
            List<ModEntry> inactiveModEntryBuffer = new List<ModEntry>() { };
            List<ModEntry> allModEntries = await FetchModsFromDisk();

            foreach (ModEntry mod in allModEntries)
            {
                foreach (ModEntry activeMod in m_ActiveModEntryList)
                {
                    if (activeMod.Id == mod.Id)
                    {
                        activeModEntryBuffer.Add(mod);
                        goto CONTINUE_NEXT_MOD;
                    }
                }
                inactiveModEntryBuffer.Add(mod);
            CONTINUE_NEXT_MOD:;
            }

            m_ActiveModEntryList = activeModEntryBuffer;
            m_InactiveModEntryList = inactiveModEntryBuffer;
            SyncModEntryTable();
        }

        private void newProfileButton_Click(object sender, EventArgs e)
        {
            m_InactiveModEntryList.AddRange(m_ActiveModEntryList);
            m_ActiveModEntryList.Clear();
            SyncModEntryTable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    public partial class ModManagerForm : Form
    {
        private readonly Color FocusColor = Color.FromArgb(242, 242, 242);
        private readonly Color UnfocusedColor = Color.FromArgb(151, 151, 151);
        private readonly Color PrimaryGrey = Color.FromArgb(51, 51, 51);
        private readonly Color NonprimaryGrey = Color.FromArgb(25, 25, 25);
        private List<ModEntry> m_ModEntryList = new List<ModEntry>() { };
        private ActiveTab m_CurrentActiveTab = ActiveTab.ActiveMods;

        public ActiveTab CurrentActiveTab
        {
            get { return m_CurrentActiveTab; }
            set
            {
                if (m_CurrentActiveTab == value) { return; }
                m_CurrentActiveTab = value;
                SyncModTab();
            }
        }

        public ModManagerForm()
        {
            InitializeComponent();
            this.Select();
            this.Focus();
            SyncModTab();
            SyncModEntryTable();
        }

        private void AddToModEntryTable(ModEntry modEntry)
        {
            m_ModEntryList.Add(modEntry);
            SyncModEntryTable();
        }

        private bool RemoveFromModEntryTable(ModEntry modEntry)
        {
            bool removed = m_ModEntryList.Remove(modEntry);
            if (removed) { SyncModEntryTable(); }
            return removed;
        }

        private void SyncModEntryTable()
        {
            if (m_ModEntryList == null || m_ModEntryList.Count == 0) { return; }
            //modEntryTableLayoutPanel.RemoveRowFrom(REPLICA_MOD_ROW_INDEX);
            foreach (ModEntry modEntry in m_ModEntryList)
            {
                AddRowToModEntryTable(modEntry);
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
        }

        private void activeModsButton_Click(object sender, EventArgs e)
        {
            CurrentActiveTab = ActiveTab.ActiveMods;
        }

        private void inactiveModsButton_Click(object sender, EventArgs e)
        {
            CurrentActiveTab = ActiveTab.InactiveMods;
        }
    }
}

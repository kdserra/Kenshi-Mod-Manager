using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    public partial class ModManagerForm : Form
    {
        private List<ModEntry> m_ModEntryList = new List<ModEntry>() { };

        public ModManagerForm()
        {
            InitializeComponent();
            this.Select();
            this.Focus();
            HideReplicaModTableEntry();
        }

        private void ClearModTable(ModEntry modEntry)
        {
            m_ModEntryList.Clear();
            SyncModTable();
        }

        private void AddToModTable(ModEntry modEntry)
        {
            m_ModEntryList.Add(modEntry);
            SyncModTable();
        }

        private bool RemoveFromModTable(ModEntry modEntry)
        {
            bool removed = m_ModEntryList.Remove(modEntry);
            if (removed) { SyncModTable(); }
            return removed;
        }

        private void SyncModTable()
        {
            throw new NotImplementedException();
        }

        private void HideReplicaModTableEntry()
        {
            replicaModTableLayoutPanel.Visible = false;
            replicaModCategoryLabel.Visible = false;
            replicaModSourceLabel.Visible = false;
            replicaModStateButton.Visible = false;
        }
    }
}

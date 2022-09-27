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
            throw new NotImplementedException();
        }

        private bool AddToModTable(ModEntry modEntry)
        {
            throw new NotImplementedException();
        }

        private bool RemoveFromModTable(ModEntry modEntry)
        {
            throw new NotImplementedException();
        }

        private void SyncModTable(ModEntry modEntry)
        {
            throw new NotImplementedException();
        }

        private void HideReplicaModTableEntry()
        {
            replicaModInfoTableLayoutPanel.Visible = false;
            replicaModCategoryLabel.Visible = false;
            replicaModSourceLabel.Visible = false;
            replicaModStateButton.Visible = false;
        }
    }
}

using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;

namespace Kenshi_Mod_Manager
{
    public static class TableLayoutPanelHelpers
    {
        public static int AddRow(this TableLayoutPanel table, RowStyle rowStyle)
        {
            table.RowCount++;
            table.RowStyles.Add(rowStyle);
            return table.RowCount - 1;
        }

        public static int AddColumn(this TableLayoutPanel table, ColumnStyle columnStyle)
        {
            table.ColumnCount++;
            table.ColumnStyles.Add(columnStyle);
            return table.ColumnCount - 1;
        }

        public static void RemoveLastRow(this TableLayoutPanel table)
        {
            table.RemoveRow(table.RowCount - 1);
        }

        public static void RemoveRow(this TableLayoutPanel table, int index)
        {
            if (index >= table.RowCount)
            {
                return;
            }
            for (int i = 0; i < table.ColumnCount; i++)
            {
                var control = table.GetControlFromPosition(i, index);
                table.Controls.Remove(control);
            }
            for (int i = index + 1; i < table.RowCount; i++)
            {
                for (int j = 0; j < table.ColumnCount; j++)
                {
                    var control = table.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        table.SetRow(control, i - 1);
                    }
                }
            }
            var removeStyle = table.RowCount - 1;
            if (table.RowStyles.Count > removeStyle)
                table.RowStyles.RemoveAt(removeStyle);

            table.RowCount--;
        }

        public static void ClearRows(this TableLayoutPanel table)
        {
            table.Controls.Clear();
            table.RowStyles.Clear();
            table.RowCount = 0;
        }
    }
}

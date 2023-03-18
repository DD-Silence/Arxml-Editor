/*  
 *  This file is a part of Arxml Editor.
 *  
 *  Copyright (C) 2021-2023 DJS Studio E-mail: ddsilence@sina.cn
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using ArxmlEditor.Model;

namespace ArxmlEditor.UI
{
    internal class ContentListView : ListView
    {
        private readonly ContentComboBox? cbCandidate;

        public ContentListView(ArCommon common)
        {
            Tag = common;
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;
            LabelEdit = true;
            Width = 250;
            Height = 250;
            Enabled = !common.IsNull();
            MouseClick += ContentListView_MouseClick;

            if (common.Role != null)
            {
                if (common.Type == ArCommonType.Others)
                {
                    Columns.Add(common.Role.Name, 200, HorizontalAlignment.Left);
                    var commons = common.GetCommonObjs();
                    foreach (var co in commons)
                    {
                        Items.Add(co.ToString());
                    }
                }
                else if(common.Type == ArCommonType.Enums)
                {
                    cbCandidate = new ContentComboBox(common)
                    {
                        Visible = false
                    };
                    Controls.Add(cbCandidate);
                    cbCandidate.SelectedIndexChanged += CbCandidate_SelectedIndexChanged;
                    Columns.Add(common.Role.Name, 200, HorizontalAlignment.Left);
                    var commons = common.GetCommonEnums();
                    foreach (var co in commons)
                    {
                        cbCandidate.Items.Add(co.ToString());
                        Items.Add(co.ToString());
                    }
                }
            }
        }

        private void CbCandidate_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if ((cbCandidate != null) && (SelectedItems.Count > 0))
            {
                SelectedItems[0].Text = cbCandidate.Text;
            }
        }

        private void ContentListView_MouseClick(object? sender, MouseEventArgs e)
        {
            var lvItem = GetItemAt(e.X, e.Y);
            lvItem.Selected = true;

            if (lvItem != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (cbCandidate != null)
                    {
                        cbCandidate.IndexSet(lvItem.Index);
                        cbCandidate.Bounds = lvItem.Bounds;
                        cbCandidate.Visible = true;
                        cbCandidate.BringToFront();
                        cbCandidate.Focus();
                        cbCandidate.Text = lvItem.Text;
                    }
                }
            }
        }
    }
}

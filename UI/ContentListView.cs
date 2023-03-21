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
            AutoArrange = true;
            MouseClick += ContentListView_MouseClick;

            ContextMenuStrip = new ContextMenuStrip();
            var itemAdd = ContextMenuStrip.Items.Add("Add");
            itemAdd.Click += ItemAdd_Click;
            itemAdd.Tag = Tag;

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
            if (cbCandidate != null)
            {
                cbCandidate.Visible = false;
                cbCandidate.BringToFront();
            }
        }

        private void ContentListView_MouseClick(object? sender, MouseEventArgs e)
        {
            var lvItem = GetItemAt(e.X, e.Y);
            if (lvItem != null)
            {
                lvItem.Selected = true;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (lvItem != null)
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
            else if (e.Button == MouseButtons.Right)
            {
                if (cbCandidate != null)
                {
                    cbCandidate.Visible = false;
                    cbCandidate.Hide();
                }

                ContextMenuStrip.Items.Clear();
                var itemAdd = ContextMenuStrip.Items.Add("Add");
                itemAdd.Click += ItemAdd_Click;
                itemAdd.Tag = Tag;

                if (lvItem != null)
                {
                    var itemDel = ContextMenuStrip.Items.Add("Del");
                    itemDel.Click += ItemDel_Click;
                    itemDel.Tag = (Tag, lvItem.Index);
                }
            }
        }

        private void ItemAdd_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem item)
            {
                if (item.Tag is ArCommon c)
                {
                    if (c.Role != null)
                    {
                        c.Parent.Add(c.Role);
                        ContentUpdate();
                    }
                }
            }
        }

        private void ItemDel_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem item)
            {
                if (item.Tag is (ArCommon c, int index))
                {
                    if (c.Role != null)
                    {
                        c.Parent.RemoveObject(c.Role, index);
                        ContentUpdate();
                    }
                }
            }
        }

        public void ContentUpdate()
        {
            if (Tag is ArCommon c)
            {
                Items.Clear();

                if (c.Type == ArCommonType.Others)
                {
                    var commons = c.GetCommonObjs();
                    foreach (var co in commons)
                    {
                        Items.Add(co.ToString());
                    }
                }
                else if (c.Type == ArCommonType.Enums)
                {
                    var commons = c.GetCommonEnums();
                    foreach (var co in commons)
                    {
                        Items.Add(co.ToString());
                    }
                }
            }
        }
    }
}

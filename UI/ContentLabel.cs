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
    internal class ContentLabel : Label
    {
        public ContentLabel(ArCommon common, TextBox tb)
        {
            Tag = (common, tb);
            Width = 250;
            if (common.Role != null)
            {
                Text = common.Role.Name;
            }
            MouseClick += ContentLabel_MouseClick;
            ContextMenuStrip = new ContextMenuStrip();
        }

        private void ContentLabel_MouseClick(object? sender, MouseEventArgs e)
        {
            if (Tag is (ArCommon c, TextBox tb))
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip.Items.Clear();
                    if (((c.Type == ArCommonType.Enum) || (c.Type == ArCommonType.Bool) || (c.Type == ArCommonType.Other)) && (c.Role != null))
                    {
                        if (c.Role.Option())
                        {
                            if (c.IsNull())
                            {
                                var itemAdd = ContextMenuStrip.Items.Add("Add");
                                itemAdd.Click += ItemAdd_Click;
                                itemAdd.Tag = c;
                            }
                            else
                            {
                                var itemDel = ContextMenuStrip.Items.Add("Delete");
                                itemDel.Click += ItemDel_Click;
                                itemDel.Tag = c;
                            }
                        }
                    }
                    ContextMenuStrip.Show();
                }
                tb.Text = c.GetDesc();
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
                    }
                }
            }
        }

        private void ItemDel_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem item)
            {
                if (item.Tag is ArCommon c)
                {
                    if (c.Role != null)
                    {
                        c.Parent.SetSpecified(c.Role, false);
                    }
                }
            }
        }
    }
}

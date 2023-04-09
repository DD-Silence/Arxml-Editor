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
using ArxmlEditor.UI;
using GenTool_CsDataServerAsrBase;
using GenTool_CsDataServerDomAsr4.Iface;
using System.Text.RegularExpressions;

namespace ArxmlEditor
{
    public partial class Main : Form
    {
        private readonly ArFile arFile = new();
        public Main()
        {
            InitializeComponent();
        }

        private void RefreshUi()
        {
            tvContent.Nodes.Clear();
            var rootNode = tvContent.Nodes.Add("Autosar");
            var rootCommon = new ArCommon(arFile.root, null, null);
            rootNode.Tag = (rootCommon, true);
            tvContent.ShowNodeToolTips = true;
            tvContent.AfterLabelEdit += AfterLabelEdit_tvContent;

            ConstructTreeView(rootCommon, rootNode, true);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            foreach (var f in new DirectoryInfo("data/bswmd").GetFiles())
            {
                arFile.AddFile(f.FullName);
            }

            RefreshUi();
        }

        private void ConstructTreeView(ArCommon arObj, TreeNode node, bool first)
        {
            foreach (var m in arObj.GetExistingMember())
            {
                if (m.Role != null)
                {
                    if (m.Type == ArCommonType.References)
                    {
                        var references = m.GetCommonReferences();
                        var nodeCurrent = node.Nodes.Add($"{m}");

                        nodeCurrent.Tag = (m, true);
                        foreach (var r in references)
                        {
                            var nodeCurrent2 = nodeCurrent.Nodes.Add($"{r}");
                            nodeCurrent2.Tag = (r, false);
                            if (first)
                            {
                                ConstructTreeView(r, nodeCurrent2, false);
                            }
                        }
                    }
                    else if (m.Type == ArCommonType.Metas)
                    {
                        var mObjs = m.GetCommonMetas();
                        var nodeCurrent = node.Nodes.Add($"{m}");

                        nodeCurrent.Tag = (m, true);
                        foreach (var mChild in mObjs)
                        {
                            if (mChild.Role != null)
                            {
                                TreeNode nodeCurrent2;
                                if (m.Role.MultipleInterfaceTypes)
                                {
                                    nodeCurrent2 = nodeCurrent.Nodes.Add($"{mChild}");
                                }
                                else
                                {
                                    nodeCurrent2 = nodeCurrent.Nodes.Add($"{mChild}");
                                }
                                nodeCurrent2.Tag = (mChild, false);
                                if (first)
                                {
                                    ConstructTreeView(mChild, nodeCurrent2, false);
                                }
                            }
                        }
                    }
                    else if ((m.Type == ArCommonType.Meta) || (m.Type == ArCommonType.Reference))
                    {
                        var nodeCurrent = node.Nodes.Add($"{m}");
                        nodeCurrent.Tag = (m, false);
                        if (first)
                        {
                            ConstructTreeView(m, nodeCurrent, false);
                        }
                    }
                }
            }
        }

        private void ConstructAddDropItems(TreeNode node, ArCommon common, ToolStripDropDownItem container)
        {
            foreach (var c in common.GetCandidateMember())
            {
                if (c.IsMeta())
                {
                    var toolAdd = container.DropDownItems.Add($"{c.Name}");
                    if (toolAdd is ToolStripDropDownItem dropItem2)
                    {
                        if (c.MultipleInterfaceTypes)
                        {
                            List<ToolStripDropDownItem> items = new();
                            foreach (var c2 in common.RoleTypesFor(c.Name))
                            {
                                var item = new ToolStripMenuItem(c2.Name[1..])
                                {
                                    Tag = (node, common, c2)
                                };
                                item.Click += DropAddHandler;
                                items.Add(item);
                            }
                            if (items.Count < 20)
                            {
                                dropItem2.DropDownItems.AddRange(items.ToArray());
                            }
                            else
                            {
                                var count = 0;
                                ToolStripItem? stripItem3 = null;
                                foreach (var item in items)
                                {
                                    if (count % 20 == 0)
                                    {
                                        stripItem3 = dropItem2.DropDownItems.Add($"{count / 20 * 20} - {Math.Min(items.Count, count / 20 * 20 + 19)}");
                                    }
                                    if (stripItem3 is ToolStripDropDownItem dropItem3)
                                    {
                                        dropItem3.DropDownItems.Add(item);
                                    }
                                    count++;
                                }
                            }
                            toolAdd.Tag = (node, common, c);
                        }
                        else
                        {
                            toolAdd.Tag = (node, common, c);
                            toolAdd.Click += DropAddHandler;
                        }
                    }
                }
            }
        }

        private void AfterLabelEdit_tvContent(object? sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Node.Tag is (ArCommon c, bool _))
                {
                    if (c.GetMeta() is IReferrable referrable)
                    {
                        var regex = new Regex("^[a-zA-Z][a-zA-Z0-9_]*");
                        if (regex.IsMatch(e.Label))
                        {
                            referrable.ShortName = e.Label;
                            ConstructContent(c);
                        }
                        else
                        {
                            e.CancelEdit = true;
                        }
                    }
                }
            }
        }

        private void MouseClick_tvContent(object sender, MouseEventArgs e)
        {
            var nodeSelect = tvContent.GetNodeAt(new Point(e.X, e.Y));
            tvContent.SelectedNode = nodeSelect;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (nodeSelect.Tag is (ArCommon c, bool isExpand))
                        {
                            ConstructContent(c);
                            if (c.Role != null)
                            {
                                UpdateBrief(c.GetDesc());
                            }
                            tvContent.LabelEdit = c.IsReferrable();
                        }
                    }
                    break;

                case MouseButtons.Right:
                    {
                        cmMember.Items.Clear();
                        if (nodeSelect.Tag is (ArCommon c, bool isExpand))
                        {
                            if (c.Type == ArCommonType.Meta)
                            {
                                var itemAdd = cmMember.Items.Add("Add");

                                if (itemAdd is ToolStripDropDownItem dropItemAdd)
                                {
                                    ConstructAddDropItems(nodeSelect, c, dropItemAdd);
                                    if (dropItemAdd.DropDownItems.Count == 0)
                                    {
                                        cmMember.Items.Remove(itemAdd);
                                    }
                                }
                            }

                            if (c.Type == ArCommonType.Reference)
                            {
                                var itemEdit = cmMember.Items.Add("Edit");
                                if (itemEdit is ToolStripDropDownItem dropItemEdit)
                                {
                                    foreach (var reference in c.ReferenceExisting())
                                    {
                                        var itemEditSub = dropItemEdit.DropDownItems.Add(reference.Key.Name[1..]);

                                        if (itemEditSub is ToolStripDropDownItem dropItemEditSub)
                                        {
                                            if (reference.Value.Count <= 20)
                                            {
                                                foreach (var referrable in reference.Value)
                                                {
                                                    var item = dropItemEditSub.DropDownItems.Add(referrable.ShortName);
                                                    item.Tag = referrable;
                                                    item.Click += ItemEdit_MouseHover;
                                                }
                                            }
                                            else
                                            {
                                                var count = 0;
                                                ToolStripItem? stripItem = null;

                                                foreach (var referrable in reference.Value)
                                                {
                                                    if (count % 20 == 0)
                                                    {
                                                        stripItem = dropItemEditSub.DropDownItems.Add($"{count / 20 * 20} - {Math.Min(reference.Value.Count, count / 20 * 20 + 19)}");
                                                    }
                                                    if (stripItem is ToolStripDropDownItem dropItem2)
                                                    {
                                                        var item = dropItem2.DropDownItems.Add(referrable.ShortName);
                                                        item.Tag = referrable;
                                                        item.Click += ItemEdit_MouseHover;
                                                    }
                                                    count++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (c.IsReferrable())
                            {
                                var referencedFrom = c.GetReferencedFrom();

                                if (referencedFrom.Count > 0)
                                {
                                    var itemReferencedFrom = cmMember.Items.Add("Referenced From");
                                    if (itemReferencedFrom is ToolStripDropDownItem dropItemReferencedFrom)
                                    {
                                        foreach (var reference in referencedFrom)
                                        {
                                            dropItemReferencedFrom.DropDownItems.Add(reference.ShortName);
                                        }
                                    }
                                }
                            }

                            if (c.CanDelete())
                            {
                                var itemDel = cmMember.Items.Add("Del");
                                itemDel.Click += DropDelHandler;
                                itemDel.Tag = (nodeSelect, c);
                            }
                        }
                        cmMember.Show(tvContent, new Point(e.X, e.Y));
                    }
                    break;

                default:
                    break;
            }
        }

        private void ItemEdit_MouseHover(object? sender, EventArgs e)
        {
            if ((sender is ToolStripDropDownItem dropItem) && (tvContent.SelectedNode.Tag is (ArCommon c, bool _)))
            {
                if ((c.Type == ArCommonType.Reference) && (dropItem.Tag is IReferrable referrable))
                {
                    c.GetReference().DestType = referrable.IdType;
                    c.GetReference().Value = referrable.AsrPath;
                    tvContent.SelectedNode.Text = $"{referrable.ShortName}(R)";
                }
            }
        }

        private void DropAddHandler(object? sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem dropItem)
            {
                if (dropItem.Tag is (TreeNode nodeSelect, ArCommon c, Meta.Iface.IMetaRI role))
                {
                    c.Add(role);
                    nodeSelect.Nodes.Clear();
                    ConstructTreeView(c, nodeSelect, true);
                    nodeSelect.Expand();
                }
                else if (dropItem.Tag is (TreeNode, ArCommon, Type type2))
                {
                    if (dropItem.OwnerItem.Tag is (TreeNode nodeSelect3, ArCommon c3, Meta.Iface.IMetaRI role3))
                    {
                        c3.Add(role3, type2);
                        nodeSelect3.Nodes.Clear();
                        ConstructTreeView(c3, nodeSelect3, true);
                        nodeSelect3.Expand();
                    }
                    else if (dropItem.OwnerItem.OwnerItem.Tag is (TreeNode nodeSelect4, ArCommon c4, Meta.Iface.IMetaRI role4))
                    {
                        c4.Add(role4, type2);
                        nodeSelect4.Nodes.Clear();
                        ConstructTreeView(c4, nodeSelect4, true);
                        nodeSelect4.Expand();
                    }
                }
            }
        }

        private void DropDelHandler(object? sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem dropItem)
            {
                if (dropItem.Tag is (TreeNode nodeSelect, ArCommon c))
                {
                    var parentNode = nodeSelect.Parent;
                    var parentCommon = c.Parent;

                    if (c.Type == ArCommonType.Meta)
                    {
                        var meta = c.GetMeta();
                        meta.DeleteAndRemoveFromOwner();
                        nodeSelect.Remove();
                    }
                    else if ((c.Type == ArCommonType.Metas) && (c.Role != null))
                    {
                        if (nodeSelect.Parent.Tag is (ArCommon c2, bool))
                        {
                            if (c2.Type == ArCommonType.Meta)
                            {
                                c2.RemoveAllObject(c.Role);
                                nodeSelect.Remove();
                            }
                        }
                    }

                    while (parentCommon.Empty())
                    {
                        parentNode.Remove();
                        parentCommon = parentCommon.Parent;
                        parentNode = parentNode.Parent;
                    }
                }
            }
        }

        private void BeforeExpand_tvContent(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is TreeNode nodeSelect)
            {
                if (nodeSelect.Tag is (ArCommon c, bool isExpand))
                {
                    if ((!isExpand) && (c.Type == ArCommonType.Meta))
                    {
                        nodeSelect.Nodes.Clear();
                        ConstructTreeView(c, nodeSelect, true);
                        nodeSelect.Tag = (c, true);
                    }
                    else if ((!isExpand) && (c.Type == ArCommonType.Metas))
                    {
                        nodeSelect.Nodes.Clear();
                        var metas = c.GetCommonMetas();
                        foreach (var meta in metas)
                        {
                            ConstructTreeView(meta, nodeSelect, true);
                        }
                    }
                }
            }
        }

        private void ConstructContent(ArCommon c)
        {
            tpContent.Tag = c;
            c.Changed -= Common_Changed;
            c.Changed += Common_Changed;
            tpContent.Controls.Clear();

            foreach (var c2 in c.GetAllMember())
            {
                if (c2.Type == ArCommonType.Other)
                {
                    if (c2.Role != null)
                    {
                        var l = new ContentLabel(c2, tbBreif);
                        tpContent.Controls.Add(l);
                        var t = new ContentTextBox(c2);
                        tpContent.Controls.Add(t);
                    }
                }
                else if (c2.Type == ArCommonType.Others)
                {
                    if (c2.Role != null)
                    {
                        var l = new ContentLabel(c2, tbBreif);
                        tpContent.Controls.Add(l);
                        var t = new ContentListView(c2);
                        tpContent.Controls.Add(t);
                    }
                }
                else if (c2.Type == ArCommonType.Enum)
                {
                    if (c2.Role != null)
                    {
                        var l = new ContentLabel(c2, tbBreif);
                        tpContent.Controls.Add(l);
                        var t = new ContentComboBox(c2);
                        tpContent.Controls.Add(t);
                    }
                }
                else if (c2.Type == ArCommonType.Enums)
                {
                    if (c2.Role != null)
                    {
                        var l = new ContentLabel(c2, tbBreif);
                        tpContent.Controls.Add(l);
                        var t = new ContentListView(c2);
                        tpContent.Controls.Add(t);
                    }
                }
                else if (c2.Type == ArCommonType.Bool)
                {
                    if (c2.Role != null)
                    {
                        var l = new ContentLabel(c2, tbBreif);
                        tpContent.Controls.Add(l);
                        var t = new ContentCheckBox(c2);
                        tpContent.Controls.Add(t);
                    }
                }
                else if (c2.Type == ArCommonType.Integer)
                {
                    if (c2.Role != null)
                    {
                        var l = new ContentLabel(c2, tbBreif);
                        tpContent.Controls.Add(l);
                        var t = new ContentTextBox(c2);
                        tpContent.Controls.Add(t);
                    }
                }
            }
        }

        private void Common_Changed()
        {
            if (tpContent.Tag is ArCommon common)
            {
                ConstructContent(common);
            }
        }

        private void UpdateBrief(string brief)
        {
            tbBreif.Text = brief;
        }

        private void UpdateOutput(string message)
        {
            tbOutput.Text += Environment.NewLine;
            tbOutput.Text += message;
        }

        private void Click_miFileLoad(object sender, EventArgs e)
        {
            var load = false;

            if (!arFile.IsEmpty())
            {
                if (MessageBox.Show("Please confirm all changes have been saved", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    load = true;
                }
            }
            else
            {
                load = true;
            }

            if (load)
            {
                OpenFileDialog fileDialog = new()
                {
                    Title = "Please select arxml file want to open.",
                    Filter = "AUTOSAR(*.arxml)|*.arxml",
                    Multiselect = true
                };
                var result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    arFile.AddFile(fileDialog.FileNames);
                    RefreshUi();
                }
            }
        }

        private void Click_miFileClear(object sender, EventArgs e)
        {
            arFile.Clear();
            tvContent.Nodes.Clear();
        }

        private void Click_miFileSave(object sender, EventArgs e)
        {
            arFile.Save();
        }

        private void Click_miFileReload(object sender, EventArgs e)
        {
            var reload = false;

            if (!arFile.IsEmpty())
            {
                if (MessageBox.Show("Please confirm all changes have been saved", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    reload = true;
                }
            }
            else
            {
                reload = true;
            }

            if (reload)
            {
                arFile.Reload();
                RefreshUi();
            }
        }

        private void Click_miFileNew(object sender, EventArgs e)
        {
            var create = false;

            if (!arFile.IsEmpty())
            {
                if (MessageBox.Show("Please confirm all changes have been saved", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    create = true;
                }
            }
            else
            {
                create = true;
            }

            if (create)
            {
                var filter = "";
                foreach (var v in Enum.GetNames(typeof(AsrVersion)))
                {
                    filter += $"AUTOSAR-{v}(*.arxml)|*.arxml|";
                }
                SaveFileDialog fileDialog = new()
                {
                    Title = "Please input new filename",
                    Filter = filter[..^1]
                };

                var result = fileDialog.ShowDialog();
                if ((result == DialogResult.OK) && (fileDialog.FilterIndex > 0))
                {
                    var asrVersion = (AsrVersion)(fileDialog.FilterIndex - 1);
                    arFile.NewFile(fileDialog.FileName, asrVersion);
                    RefreshUi();
                }
            }
        }
    }
}
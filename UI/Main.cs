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
using GenTool_CsDataServerDomAsr4.Iface;
using Meta.Helper;
using Meta.Iface;
using System.Collections;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ArxmlEditor
{
    public partial class Main : Form
    {
        private ArFile? arFile;
        public Main()
        {
            InitializeComponent();
        }

        private void ConstructTreeView(ArCommon arObj, TreeNode node, bool first)
        {
            foreach (var m in arObj.GetExistingMember())
            {
                if (m.Type == ArCommonType.MetaObjects)
                {
                    var mObjs = m.GetCommonMetas();
                    var nodeCurrent = node.Nodes.Add($"{m}");
                    nodeCurrent.ToolTipText = $"Type: {m.Role.Name}{Environment.NewLine}" +
                                              $"Min: {m.Role.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Role.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m, true, true);
                    foreach (var mChild in mObjs)
                    {
                        TreeNode nodeCurrent2;
                        if (m.Role.MultipleInterfaceTypes)
                        {
                            nodeCurrent2 = nodeCurrent.Nodes.Add($"{mChild}");
                            nodeCurrent2.ToolTipText = $"Type: {mChild.Role.InterfaceType.Name[1..]}{Environment.NewLine}" +
                                                        $"Min: {m.Role.Min()}{Environment.NewLine}" +
                                                        $"Max: {m.Role.Max()}{Environment.NewLine}";
                        }
                        else
                        {
                            nodeCurrent2 = nodeCurrent.Nodes.Add($"{mChild}");
                            nodeCurrent2.ToolTipText = $"Type: {m.Role.Name}{Environment.NewLine}" +
                                                        $"Min: {m.Role.Min()}{Environment.NewLine}" +
                                                        $"Max: {m.Role.Max()}{Environment.NewLine}";
                        }
                        nodeCurrent2.Tag = (mChild, true, false);
                        if (first)
                        {
                            ConstructTreeView(mChild, nodeCurrent2, false);
                        }
                    }
                }
                else if (m.Type == ArCommonType.MetaObject)
                {
                    var nodeCurrent = node.Nodes.Add($"{m}");
                    nodeCurrent.ToolTipText = $"Type: {m.Role.Name}{Environment.NewLine}" +
                                                $"Min: {m.Role.Min()}{Environment.NewLine}" +
                                                $"Max: {m.Role.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m, true, false);
                    if (first)
                    {
                        ConstructTreeView(m, nodeCurrent, false);
                    }
                }
                else if (m.Type == ArCommonType.Enums)
                {
                    var enums = m.GetEnums();

                    var nodeCurrent = node.Nodes.Add($"{m}");
                    nodeCurrent.ToolTipText = $"Type: {m.Role.Name}{Environment.NewLine}" +
                                              $"Min: {m.Role.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Role.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m, false, false);
                    foreach (var e in enums)
                    {
                        var nodeCurrent2 = nodeCurrent.Nodes.Add($"{e.ToString()[1..]}: {m.Role.Name}");
                        nodeCurrent2.Tag = (e, false, false);
                        //                            var nodeCurrent2 = nodeCurrent.Nodes.Add($"{e}: {m.Key.Name}");
                        //                            nodeCurrent2.Tag = (arObj, e, m.Key, false, false);
                    }
                }
                else if (m.Type == ArCommonType.Others)
                {
                    var objs = m.GetCommonObjs();
                    var nodeCurrent = node.Nodes.Add($"{m}");

                    nodeCurrent.ToolTipText = $"Type: {m.Role.Name}{Environment.NewLine}" +
                                              $"Min: {m.Role.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Role.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m, false, false);
                    foreach (var o in objs)
                    {
                        var nodeCurrent2 = nodeCurrent.Nodes.Add($"{o}: {m.Role.Name}");
                        nodeCurrent2.Tag = (o, false, false);
                    }
                }
                else if (m.Type == ArCommonType.Enum)
                {
                    var nodeCurrent = node.Nodes.Add($"{m}: {m.Role.Name}");
                    nodeCurrent.Tag = (m, false, false);
                }
                else if (m.Type == ArCommonType.Other)
                {
                    var nodeCurrent = node.Nodes.Add($"{m}: {m.Role.Name}");
                    nodeCurrent.Tag = (m, false, false);
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            List<string> paths = new();
            foreach (var f in new DirectoryInfo("data/bswmd").GetFiles())
            {
                paths.Add(f.FullName);
            }
            arFile = new ArFile(paths);
            var rootNode = tvContent.Nodes.Add("Autosar");
            rootNode.Tag = arFile.root;
            tvContent.ShowNodeToolTips = true;

            ConstructTreeView(new ArCommon(arFile.root, null, null), rootNode, true);
        }

        private void ConstructAddDropItems(TreeNode node, ArCommon common, ToolStripDropDownItem container)
        {
            foreach (var c in common.GetCandidateMember())
            {
                var toolAdd = container.DropDownItems.Add($"{c.Name}");
                if (toolAdd is ToolStripDropDownItem dropItem2)
                {
                    if (c.MultipleInterfaceTypes)
                    {
                        List<ToolStripDropDownItem> items = new();
                        foreach (var c2 in common.RoleTypesFor(c.Name))
                        {
                            var item = new ToolStripMenuItem(c2.Name[1..]);
                            item.Tag = (node, common, c2);
                            item.Click += DropAddHandler;
                            items.Add(item);
                        }
                        dropItem2.DropDownItems.AddRange(items.ToArray());
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

        private void tvContent_MouseClick(object sender, MouseEventArgs e)
        {
            var nodeSelect = tvContent.GetNodeAt(new Point(e.X, e.Y));
            tvContent.SelectedNode = nodeSelect;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (nodeSelect.Tag is (ArCommon c, bool isMObj, bool isExpand))
                        {
                            if (true == c.CanEdit())
                            {
                                tvContent.LabelEdit = true;
                            }
                            else
                            {
                                tvContent.LabelEdit = false;
                            }
                        }
                    }
                    break;

                case MouseButtons.Right:
                    {
                        cmMember.Items.Clear();
                        if (nodeSelect.Tag is (ArCommon c, bool isMObj, bool isExpand))
                        {
                            if (c.Type == ArCommonType.MetaObject)
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

                            if (c.CanDelete())
                            {
                                var itemDel = cmMember.Items.Add("Del");
                                itemDel.Click += DropDelHandler;
                                itemDel.Tag = c;
                            }
                        }
                        cmMember.Show(tvContent, new Point(e.X, e.Y));
                    }
                    break;

                default:
                    break;
            }
        }

        private void DropAddHandler(object? sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem dropItem)
            {
                if (dropItem.Tag is (TreeNode nodeSelect, ArCommon c, IMetaRI role))
                {
                    c.Add(role);
                    nodeSelect.Nodes.Clear();
                    ConstructTreeView(c, nodeSelect, true);
                    nodeSelect.Expand();
                }
                else if (dropItem.Tag is (TreeNode nodeSelect2, ArCommon c2, Type type2))
                {
                    if (dropItem.OwnerItem.Tag is (TreeNode nodeSelect3, ArCommon c3, IMetaRI role3))
                    {
                        c3.Add(role3, type2);
                        nodeSelect3.Nodes.Clear();
                        ConstructTreeView(c3, nodeSelect3, true);
                        nodeSelect3.Expand();
                    }
                }
            }
        }

        private void DropDelHandler(object? sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem dropItem)
            {
                if (dropItem.Tag is (TreeNode nodeSelect, ArCommon c, IMetaRI role))
                {
                    if (c.Type == ArCommonType.MetaObject)
                    {
                        var mObj = c.GetMeta();
                        mObj.DeleteAndRemoveFromOwner();

                        if (nodeSelect.Parent.Tag is (ArCommon c2, bool isMObj, bool isExpand))
                        {
                            if (c2.Type == ArCommonType.MetaObjects)
                            {
                                var mObjs = c2.GetMetas();
                                if (mObjs.Count() == 0)
                                {
                                    nodeSelect.Parent.Remove();
                                }
                                else
                                {
                                    nodeSelect.Remove();
                                }
                            }
                            else
                            {
                                nodeSelect.Remove();
                            }
                        }
                    }
                    else if (c.Type == ArCommonType.Other)
                    {
                        if (nodeSelect.Tag is (ArCommon c2, bool isMObj, bool isExpand))
                        {
                            if (c2.Type == ArCommonType.MetaObjects)
                            {
                                var metas = c2.GetMetas();

                                c.RemoveAllObject(role);
                                var p = nodeSelect.Parent;
                                p.Nodes.Clear();
                                ConstructTreeView(c2, p, true);
                                p.Expand();
                            }
                        }
                    }
                }
                else if (dropItem.Tag is (TreeNode nodeSelect2, object obj2, IMetaRI role2))
                {
                    if (nodeSelect2.Tag is (ArCommon c3, bool isMObj, bool isExpand))
                    {
                        if (c3.Type == ArCommonType.MetaObjects)
                        {
                            c3.Parent.RemoveAllObject(c3.Role);
                            var p = nodeSelect2.Parent;
                            p.Nodes.Clear();
                            ConstructTreeView(c3.Parent, p, true);
                            p.Expand();
                        }
                        else if (obj2.GetType().IsClass)
                        {
                            c3.Parent.SetSpecified(c3.Role, false);
                            var p = nodeSelect2.Parent;
                            p.Nodes.Clear();
                            ConstructTreeView(c3.Parent, p, true);
                            p.Expand();
                        }
                        else
                        {
                            c3.Parent.RemoveObject(c3.Role, c3);
                            var p = nodeSelect2.Parent.Parent;
                            p.Nodes.Clear();
                            ConstructTreeView(c3.Parent, p, true);
                            p.Expand();
                        }
                    }
                }
            }
        }

        private void tvContent_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is TreeNode nodeSelect)
            {
                if (nodeSelect.Tag is (ArCommon c, bool isMObj, bool isExpand))
                {
                    if ((!isExpand) && (isMObj) && (c.Type == ArCommonType.MetaObject))
                    {
                        nodeSelect.Nodes.Clear();
                        ConstructTreeView(c, nodeSelect, true);
                        nodeSelect.Tag = (c, isMObj, true);
                    }
                    else if ((!isExpand) && (isMObj) && (c.Type == ArCommonType.MetaObjects))
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

        private void ConstructContent(IMetaObjectInstance mobj)
        {

        }
    }
}
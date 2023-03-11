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
                if (m.Value.Type == ArCommonType.MetaObjects)
                {
                    var mObjs = m.Value.GetMetas();
                    var nodeCurrent = node.Nodes.Add($"{m.Key.Name}(s)");
                    nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                              $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Key.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m.Value, true, true);
                    foreach (var mChild in mObjs)
                    {
                        TreeNode nodeCurrent2;
                        if (mChild is IReferrable refChild)
                        {
                            if (m.Key.MultipleInterfaceTypes)
                            {
                                nodeCurrent2 = nodeCurrent.Nodes.Add($"{refChild.ShortName}");
                                nodeCurrent2.ToolTipText = $"Type: {mChild.InterfaceType.Name[1..]}{Environment.NewLine}" +
                                                           $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                                           $"Max: {m.Key.Max()}{Environment.NewLine}";
                            }
                            else
                            {
                                nodeCurrent2 = nodeCurrent.Nodes.Add($"{refChild.ShortName}");
                                nodeCurrent2.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                                           $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                                           $"Max: {m.Key.Max()}{Environment.NewLine}";
                            }
                        }
                        else
                        {
                            nodeCurrent2 = nodeCurrent.Nodes.Add($"{mChild.InterfaceType.Name}");
                            nodeCurrent2.ToolTipText = $"Type: {mChild.InterfaceType.Name}{Environment.NewLine}" +
                                                       $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                                       $"Max: {m.Key.Max()}{Environment.NewLine}";
                        }
                        nodeCurrent2.Tag = (m.Value, true, false);
                        if (first)
                        {
                            ConstructTreeView(new ArCommon(mChild, m.Key, arObj), nodeCurrent2, false);
                        }
                    }
                }
                else if (m.Value.Type == ArCommonType.MetaObject)
                {
                    TreeNode nodeCurrent;
                    var mObj = m.Value.GetMeta();

                    if (mObj is IReferrable refObj)
                    {
                        nodeCurrent = node.Nodes.Add($"{refObj.ShortName}");
                        nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                                  $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                                  $"Max: {m.Key.Max()}{Environment.NewLine}";
                    }
                    else
                    {
                        nodeCurrent = node.Nodes.Add($"{m.Key.Name}");
                        nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                                  $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                                  $"Max: {m.Key.Max()}{Environment.NewLine}";
                    }
                    nodeCurrent.Tag = (m.Value, true, false);
                    if (first)
                    {
                        ConstructTreeView(m.Value, nodeCurrent, false);
                    }
                }
                else if (m.Value.Type == ArCommonType.Enums)
                {
                    var enums = m.Value.GetEnums();

                    var nodeCurrent = node.Nodes.Add($"{m.Key.Name}(s)");
                    nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                              $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Key.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m.Value, false, false);
                    foreach (var e in enums)
                    {
                        var nodeCurrent2 = nodeCurrent.Nodes.Add($"{e.ToString()[1..]}: {m.Key.Name}");
                        nodeCurrent2.Tag = (new ArCommon(e, m.Key, m.Value), false, false);
                        //                            var nodeCurrent2 = nodeCurrent.Nodes.Add($"{e}: {m.Key.Name}");
                        //                            nodeCurrent2.Tag = (arObj, e, m.Key, false, false);
                    }
                }
                else if (m.Value.Type == ArCommonType.Others)
                {
                    var objs = m.Value.GetObjs();

                    var nodeCurrent = node.Nodes.Add($"{m.Key.Name}(s)");
                    nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                              $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Key.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (m.Value, false, false);
                    foreach (var o in objs)
                    {
                        var nodeCurrent2 = nodeCurrent.Nodes.Add($"{o}: {m.Key.Name}");
                        nodeCurrent2.Tag = (new ArCommon(o, m.Key, m.Value), false, false);
                    }
                }
                else if (m.Value.Type == ArCommonType.Enum)
                {
                    var e = m.Value.GetEnum();

                    var nodeCurrent = node.Nodes.Add($"{e.ToString()[1..]}: {m.Key.Name}");
                    nodeCurrent.Tag = (m.Value, false, false);
                }
                else if (m.Value.Type == ArCommonType.Other)
                {
                    var o = m.Value.GetObj();
                    var nodeCurrent = node.Nodes.Add($"{m.Value}: {m.Key.Name}");
                    nodeCurrent.Tag = (m.Value, false, false);
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

        private void ConstructAddDropItems(TreeNode node, IMetaObjectInstance mObj, ToolStripDropDownItem container)
        {
            foreach (var c in mObj.GetCandidateMember())
            {
                var toolAdd = container.DropDownItems.Add($"{c.Name}");
                if (toolAdd is ToolStripDropDownItem dropItem2)
                {
                    if (c.MultipleInterfaceTypes)
                    {
                        List<ToolStripDropDownItem> items = new();
                        foreach (var c2 in mObj.RoleTypesFor(c.Name))
                        {
                            var item = new ToolStripMenuItem(c2.Name[1..]);
                            item.Tag = (node, mObj, c2);
                            item.Click += DropAddHandler;
                            items.Add(item);
                        }
                        dropItem2.DropDownItems.AddRange(items.ToArray());
                        toolAdd.Tag = (node, mObj, c);
                    }
                    else
                    {
                        toolAdd.Tag = (node, mObj, c);
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
                        if (nodeSelect.Tag is (object parent, object obj, IMetaRI mObjRole, bool isMObj, bool isExpand))
                        {
                            if (true == obj.CanEdit())
                            {
                                tvContent.LabelEdit = true;
                                if (obj is string)
                                {

                                }
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
                                var mObj = c.GetMeta();
                                var itemAdd = cmMember.Items.Add("Add");

                                if (itemAdd is ToolStripDropDownItem dropItemAdd)
                                {
                                    ConstructAddDropItems(nodeSelect, mObj, dropItemAdd);
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
                        if (c3.Type == ArCommonType.MetaObject)
                        {
                            var mObj3 = c3.GetMeta();
                            mObj3.AddNew(role3.Name, type2);
                            nodeSelect3.Nodes.Clear();
                            ConstructTreeView(c3, nodeSelect3, true);
                            nodeSelect3.Expand();
                        }
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
                    if (nodeSelect2.Tag is (IMetaObjectInstance parent, object obj, IMetaRI mObjRole, bool isMObj, bool isExpand))
                    {
                        if (obj is IMetaCollectionInstance metas)
                        {
                            parent.RemoveAllObject(mObjRole);
                            var p = nodeSelect2.Parent;
                            p.Nodes.Clear();
                            //ConstructTreeView(parent, p, true);
                            p.Expand();
                        }
                        else if (obj2.GetType().IsClass)
                        {
                            parent.SetSpecified(mObjRole.Name, false);
                            var p = nodeSelect2.Parent;
                            p.Nodes.Clear();
                            //ConstructTreeView(parent, p, true);
                            p.Expand();
                        }
                        else
                        {
                            parent.RemoveObject(mObjRole, obj);
                            var p = nodeSelect2.Parent.Parent;
                            p.Nodes.Clear();
                            //ConstructTreeView(parent, p, true);
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
                    if ((!isExpand) && (isMObj) && ((c.Type == ArCommonType.MetaObject) || (c.Type == ArCommonType.MetaObjects)))
                    {
                        nodeSelect.Nodes.Clear();
                        ConstructTreeView(c, nodeSelect, true);
                        nodeSelect.Tag = (c, isMObj, true);
                    }
                }
            }
        }

        private void ConstructContent(IMetaObjectInstance mobj)
        {

        }
    }
}
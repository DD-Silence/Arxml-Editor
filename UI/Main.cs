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

namespace ArxmlEditor
{
    public partial class Main : Form
    {
        private ArFile? arFile;
        public Main()
        {
            InitializeComponent();
        }

        private void ConstructTreeView(IMetaObjectInstance arObj, TreeNode node, bool first)
        {
            foreach (var m in arObj.GetExistingMember())
            {
                if (m.Value is IEnumerable<IMetaObjectInstance> mObjs)
                {
                    var nodeCurrent = node.Nodes.Add($"{m.Key.Name}");
                    nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                              $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                              $"Max: {m.Key.Max()}{Environment.NewLine}";
                    nodeCurrent.Tag = (arObj, mObjs, m.Key, true, true);
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
                        nodeCurrent2.Tag = (mObjs, mChild, m.Key, true, false);
                        if (first)
                        {
                            ConstructTreeView(mChild, nodeCurrent2, false);
                        }
                    }
                }
                else if (m.Value is IMetaObjectInstance mObj)
                {
                    TreeNode nodeCurrent;
                    if (m.Value is IReferrable refObj)
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
                    nodeCurrent.Tag = (arObj, mObj, m.Key, true, false);
                    if (first)
                    {
                        ConstructTreeView(mObj, nodeCurrent, false);
                    }
                }
                else if (m.Value is IEnumerable eMetas)
                {
                    if (m.Value is not string)
                    {
                        var nodeCurrent = node.Nodes.Add($"{m.Key.Name}");
                        nodeCurrent.ToolTipText = $"Type: {m.Key.Name}{Environment.NewLine}" +
                                                  $"Min: {m.Key.Min()}{Environment.NewLine}" +
                                                  $"Max: {m.Key.Max()}{Environment.NewLine}";
                        nodeCurrent.Tag = (arObj, m.Value, m.Key, false, false);
                        foreach (var e in eMetas)
                        {
                            if (e is Enum en)
                            {
                                var nodeCurrent2 = nodeCurrent.Nodes.Add($"{e.ToString()[1..]}: {m.Key.Name}");
                                nodeCurrent2.Tag = (arObj, e, m.Key, false, false);
                            }
                            else
                            {
                                var nodeCurrent2 = nodeCurrent.Nodes.Add($"{e}: {m.Key.Name}");
                                nodeCurrent2.Tag = (arObj, e, m.Key, false, false);
                            }
                        }
                    }
                    else
                    {
                        var nodeCurrent = node.Nodes.Add($"{m.Value}: {m.Key.Name}");
                        nodeCurrent.Tag = (arObj, m.Value, m.Key, false, false);
                    }
                }
                else if (m.Value is Enum eMeta)
                {
                    var nodeCurrent = node.Nodes.Add($"{eMeta.ToString()[1..]}: {m.Key.Name}");
                    nodeCurrent.Tag = (arObj, m.Value, m.Key, false, false);
                }
                else
                {
                    var nodeCurrent = node.Nodes.Add($"{m.Value}: {m.Key.Name}");
                    nodeCurrent.Tag = (arObj, m.Value, m.Key, false, false);
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

            ConstructTreeView(arFile.root, rootNode, true);
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
                case MouseButtons.Right:
                    {
                        cmMember.Items.Clear();
                        if (nodeSelect.Tag is (_ , object obj, IMetaRI mObjRole, bool isMObj, bool isExpand))
                        {
                            if (obj is IMetaObjectInstance mObj)
                            {
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
                            else if ((obj is not IEnumerable) || (obj is string))
                            {
                                var itemAdd = cmMember.Items.Add("Edit");
                            }

                            if (obj.CanDelete(mObjRole))
                            {
                                var itemDel = cmMember.Items.Add("Del");
                                itemDel.Click += DropDelHandler;
                                itemDel.Tag = (nodeSelect, obj, mObjRole);
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
                if (dropItem.Tag is (TreeNode nodeSelect, IMetaObjectInstance mObj, IMetaRI role))
                {
                    if (role.Single())
                    {  
                        if (role.Option())
                        {
                            mObj.SetSpecified(role.Name, true);
                            nodeSelect.Nodes.Clear();
                            ConstructTreeView(mObj, nodeSelect, true);
                            nodeSelect.Expand();
                        }
                    }
                    else if (role.Multiply())
                    {
                        var collection = mObj.GetCollectionValueRaw(role.Name);

                        if (collection is IEnumerable<IMetaObjectInstance>)
                        {
                            mObj.AddNew(role.Name, role.InterfaceType);
                            nodeSelect.Nodes.Clear();
                            ConstructTreeView(mObj, nodeSelect, true);
                            nodeSelect.Expand();
                        }
                        else
                        {
                            mObj.AddObject(role);
                            nodeSelect.Nodes.Clear();
                            ConstructTreeView(mObj, nodeSelect, true);
                            nodeSelect.Expand();
                        }
                    }
                }
                else if (dropItem.Tag is (TreeNode nodeSelect2, IMetaObjectInstance mObj2, Type type2))
                {
                    if (dropItem.OwnerItem.Tag is (TreeNode nodeSelect3, IMetaObjectInstance mObj3, IMetaRI role3))
                    {
                        mObj3.AddNew(role3.Name, type2);
                        nodeSelect3.Nodes.Clear();
                        ConstructTreeView(mObj3, nodeSelect3, true);
                        nodeSelect3.Expand();
                    }
                }
            }
        }

        private void DropDelHandler(object? sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem dropItem)
            {
                if (dropItem.Tag is (TreeNode nodeSelect, IMetaObjectInstance mObj, IMetaRI role))
                {
                    mObj.DeleteAndRemoveFromOwner();
                    if (nodeSelect.Parent.Tag is (IMetaObjectInstance parent, IEnumerable<IMetaObjectInstance> mObjs, IMetaRI mObjRole, bool isMObj, bool isExpand))
                    {
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
                else if (dropItem.Tag is (TreeNode nodeSelect2, object obj2, IMetaRI role2))
                {
                    if (nodeSelect2.Tag is (IMetaObjectInstance parent, object obj, IMetaRI mObjRole, bool isMObj, bool isExpand))
                    {
                        if (obj is IMetaCollectionInstance metas)
                        {
                            parent.RemoveAllObject(mObjRole);
                            var p = nodeSelect2.Parent;
                            p.Nodes.Clear();
                            ConstructTreeView(parent, p, true);
                            p.Expand();
                        }
                        else if (obj2.GetType().IsClass)
                        {
                            parent.SetSpecified(mObjRole.Name, false);
                            var p = nodeSelect2.Parent;
                            p.Nodes.Clear();
                            ConstructTreeView(parent, p, true);
                            p.Expand();
                        }
                        else
                        {
                            parent.RemoveObject(mObjRole, obj);
                            var p = nodeSelect2.Parent.Parent;
                            p.Nodes.Clear();
                            ConstructTreeView(parent, p, true);
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
                if (nodeSelect.Tag is (IMetaObjectInstance mObj, object obj, IMetaRI mObjRole, bool isMObj, bool isExpand))
                {
                    if ((!isExpand) && (isMObj) && (obj is IMetaObjectInstance m))
                    {
                        nodeSelect.Nodes.Clear();
                        ConstructTreeView(m, nodeSelect, true);
                        nodeSelect.Tag = (mObj, obj, mObjRole, isMObj, true);
                    }
                }
                else if (nodeSelect.Tag is (IEnumerable<IMetaObjectInstance> mObj2, object obj2, IMetaRI mObjRole2, bool isMObj2, bool isExpand2))
                {
                    if ((!isExpand2) && (isMObj2) && (obj2 is IMetaObjectInstance m))
                    {
                        nodeSelect.Nodes.Clear();
                        ConstructTreeView(m, nodeSelect, true);
                        nodeSelect.Tag = (mObj2, obj2, mObjRole2, isMObj2, true);
                    }
                }
            }
        }


    }
}
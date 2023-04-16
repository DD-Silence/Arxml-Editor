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
using System.Collections.Specialized;
using System.ComponentModel;

namespace ArxmlEditor.UI
{
    internal class ContentTreeNode : TreeNode
    {
        public ContentTreeNode(ArCommon common, bool expand)
        {
            Tag = (common, expand);
            if (common.Type == ArCommonType.Meta)
            {
                common.DelPropertyandler(MetaPropertyChangedHandler);
                common.AddPropertyHandler(MetaPropertyChangedHandler);
            }
            else if (common.Type == ArCommonType.Metas)
            {
                common.DelCollectionHandler(MetaCollectionChangedHandler);
                common.AddCollectionHandler(MetaCollectionChangedHandler);
            }
            else if (common.Type == ArCommonType.Reference)
            {
                common.DelPropertyandler(ReferencePropertyChangedHandler);
                common.AddPropertyHandler(ReferencePropertyChangedHandler);
            }
            else if (common.Type == ArCommonType.References)
            {
                common.DelCollectionHandler(ReferenceCollectionChangedHandler);
                common.AddCollectionHandler(ReferenceCollectionChangedHandler);
            }
            Text = common.ToString();
        }

        private void MetaPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            if (sender != null)
            {
                if (Tag is (ArCommon c, bool))
                {
                    Text = c.ToString();
                }
            }
        }

        private void MetaCollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                if (Tag is (ArCommon c, bool))
                {
                    if (c.Empty())
                    {
                        Remove();
                    }
                }
            }
        }

        private void ReferencePropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            if (sender != null)
            {
                if (Tag is (ArCommon c, bool))
                {
                    Text = c.ToString();
                }
            }
        }

        private void ReferenceCollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                if (Tag is (ArCommon c, bool))
                {
                    if (c.Empty())
                    {
                        Remove();
                    }
                }
            }
        }
    }
}

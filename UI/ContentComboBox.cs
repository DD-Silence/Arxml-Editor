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
    internal class ContentComboBox : ComboBox
    {
        public ContentComboBox(ArCommon common, int index = 0)
        {
            Tag = (common, index);
            Width = 250;
            Enabled = !common.IsNull();
            DropDownStyle = ComboBoxStyle.DropDownList;
            SelectedIndexChanged += ContentComboBox_SelectedIndexChanged;
            if (common.Parent.Type == ArCommonType.Meta)
            {
                Items.AddRange(common.EnumCanditate());
            }
            else if (common.Parent.Type == ArCommonType.Reference)
            {
                Items.AddRange(common.Parent.ReferenceCanditate());
                Enabled = false;
            }
            Text = common.ToString();
        }

        private void ContentComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (Tag is (ArCommon common, int index))
            {
                if (common.Type == ArCommonType.Enum)
                {
                    common.SetEnum(Text);
                }
                else if ((common.Type == ArCommonType.Enums) && (common.GetEnums() != null))
                {
                    if ((index >= 0) && (index < common.GetEnums().Count))
                    {
                        if (common.GetEnumsName(index) != Text)
                        {
                            common.SetEnums(index, Text);
                        }
                    }
                }
            }
        }

        public void IndexSet(int index)
        {
            if (Tag is (ArCommon common, int))
            {
                Tag = (common, index);
            }
        }
    }
}

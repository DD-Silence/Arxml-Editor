﻿/*  
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
    internal class ContentTextBox : TextBox
    {
        public ContentTextBox(ArCommon common)
        {
            Tag = common;
            Text = common.ToString();
            Width = 250;
            TextChanged += ContentTextBox_TextChanged;

            if (common.Role != null)
            {
                if (common.Role.Name == "ShortName")
                {
                    Enabled = false;
                }
                else
                {
                    Enabled = !common.IsNull();
                }
            }
            else
            {
                Enabled = false;
            }
        }

        private void ContentTextBox_TextChanged(object? sender, EventArgs e)
        {
            if (Tag is ArCommon c)
            {
                c.SetOther(Text);
            }
        }
    }
}

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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ArxmlEditor.UI
{
    internal class ContentCheckBox : CheckBox
    {
        public ContentCheckBox(ArCommon common)
        {
            Tag = common;
            Text = common.ToString();
            Width = 250;
            Enabled = !common.IsNull();
            Checked = common.GetBool();
            CheckedChanged += ContentCheckBox_CheckedChanged;
        }

        private void ContentCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (Tag is ArCommon c)
            {
                c.SetBool(Checked);
            }
        }
    }
}
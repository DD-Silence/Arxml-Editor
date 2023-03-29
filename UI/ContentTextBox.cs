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
using System.Text.RegularExpressions;

namespace ArxmlEditor.UI
{
    internal class ContentTextBox : TextBox
    {
        string lastCorrectText = "";

        public ContentTextBox(ArCommon common)
        {
            Tag = common;
            Text = common.ToString();
            lastCorrectText = Text;
            Width = 250;
            LostFocus += ContentTextBox_LostFocus;

            if (common.Role != null)
            {
                if (common.Parent.Type == ArCommonType.Reference)
                {
                    Enabled = false;
                }
                else if (common.Role.Name == "ShortName")
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

        private void ContentTextBox_LostFocus(object? sender, EventArgs e)
        {
            if (Tag is ArCommon c)
            {
                var expression = c.GetPrimitiveConstraints();
                if (expression == "")
                {
                    c.SetOther(Text);
                    lastCorrectText = Text;
                }
                else
                {
                    var regex = new Regex(expression);
                    if (regex.IsMatch(Text))
                    {
                        c.SetOther(Text);
                        lastCorrectText = Text;
                    }
                    else
                    {
                        Text = lastCorrectText;
                    }
                }
            }
        }
    }
}

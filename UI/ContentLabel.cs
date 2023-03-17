using ArxmlEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArxmlEditor.UI
{
    internal class ContentLabel : Label
    {
        public ContentLabel(ArCommon common)
        {
            Tag = common;
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
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip.Items.Clear();
                if (Tag is ArCommon c)
                {
                    if (((c.Type == ArCommonType.Enum) || (c.Type == ArCommonType.Other)) && (c.Role != null))
                    {
                        if (c.Role.Option())
                        {
                            if (c.IsNull())
                            {
                                ContextMenuStrip.Items.Add("Add");
                            }
                            else
                            {
                                ContextMenuStrip.Items.Add("Delete");
                            }
                        }
                    }
                }
                ContextMenuStrip.Show();
            }
        }
    }
}

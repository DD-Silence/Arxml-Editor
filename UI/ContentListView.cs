using ArxmlEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArxmlEditor.UI
{
    internal class ContentListView : ListView
    {
        private ComboBox? cbCandidate;

        public ContentListView(ArCommon c)
        {
            Tag = c;
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;
            LabelEdit = true;
            MouseClick += ContentListView_MouseClick;

            if (c.Role != null)
            {
                if (c.Type == ArCommonType.Others)
                {
                    Columns.Add(c.Role.Name, 200, HorizontalAlignment.Left);
                    var commons = c.GetCommonObjs();
                    foreach (var co in commons)
                    {
                        Items.Add(co.ToString());
                    }
                }
                else if(c.Type == ArCommonType.Enums)
                {
                    cbCandidate = new ComboBox
                    {
                        Visible = false
                    };
                    Controls.Add(cbCandidate);
                    Columns.Add(c.Role.Name, 200, HorizontalAlignment.Left);
                    var commons = c.GetCommonEnums();
                    foreach (var co in commons)
                    {
                        cbCandidate.Items.Add(co.ToString());
                        Items.Add(co.ToString());
                    }
                }
            }
        }

        private void ContentListView_MouseClick(object? sender, MouseEventArgs e)
        {
            var lvItem = GetItemAt(e.X, e.Y);

            if (lvItem != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (cbCandidate != null)
                    {
                        cbCandidate.Bounds = lvItem.Bounds;
                        cbCandidate.Visible = true;
                        cbCandidate.BringToFront();
                        cbCandidate.Focus();
                        cbCandidate.Text = lvItem.Text;
                    }
                }
            }
        }
    }
}

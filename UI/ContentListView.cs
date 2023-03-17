using ArxmlEditor.Model;

namespace ArxmlEditor.UI
{
    internal class ContentListView : ListView
    {
        private ContentComboBox? cbCandidate;

        public ContentListView(ArCommon common)
        {
            Tag = common;
            View = View.Details;
            FullRowSelect = true;
            GridLines = true;
            LabelEdit = true;
            Width = 250;
            Height = 250;
            Enabled = !common.IsNull();
            MouseClick += ContentListView_MouseClick;

            if (common.Role != null)
            {
                if (common.Type == ArCommonType.Others)
                {
                    Columns.Add(common.Role.Name, 200, HorizontalAlignment.Left);
                    var commons = common.GetCommonObjs();
                    foreach (var co in commons)
                    {
                        Items.Add(co.ToString());
                    }
                }
                else if(common.Type == ArCommonType.Enums)
                {
                    cbCandidate = new ContentComboBox(common)
                    {
                        Visible = false
                    };
                    Controls.Add(cbCandidate);
                    Columns.Add(common.Role.Name, 200, HorizontalAlignment.Left);
                    var commons = common.GetCommonEnums();
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
                        cbCandidate.IndexSet(lvItem.Index);
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

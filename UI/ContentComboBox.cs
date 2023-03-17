using ArxmlEditor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Items.AddRange(common.EnumCanditate());
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
                else if ((common.Type == ArCommonType.Enums) && (common.Enums != null))
                {
                    if ((index >= 0) && (index < common.Enums.Count))
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
            if (Tag is (ArCommon common, int i))
            {
                Tag = (common, index);
            }
        }
    }
}

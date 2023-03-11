using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class UShortEditor : NumericEditor<UInt16>
    {
        public override PageEditorType EditorType => PageEditorType.Short;

        public UShortEditor(ushort val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
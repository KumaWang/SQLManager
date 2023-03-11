using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class ULongEditor : NumericEditor<UInt64>
    {
        public override PageEditorType EditorType => PageEditorType.Short;

        public ULongEditor(ulong val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
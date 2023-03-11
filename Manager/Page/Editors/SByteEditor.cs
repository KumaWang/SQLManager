using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class SByteEditor : NumericEditor<SByte>
    {
        public override PageEditorType EditorType => PageEditorType.Long;

        public SByteEditor(sbyte val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class IntEditor : NumericEditor<Int32>
    {
        public override PageEditorType EditorType => PageEditorType.Int;

        public IntEditor(int val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
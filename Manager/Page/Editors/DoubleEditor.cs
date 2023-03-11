using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class DoubleEditor : NumericEditor<Double>
    {
        public override PageEditorType EditorType => PageEditorType.Double;

        public DoubleEditor(double val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
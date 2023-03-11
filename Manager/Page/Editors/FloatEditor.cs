using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manager.Page.Editor
{ 
    class FloatEditor : NumericEditor<Single>
    {
        public override PageEditorType EditorType => PageEditorType.Float;

        public FloatEditor(float val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
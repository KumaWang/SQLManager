using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class ShortEditor : NumericEditor<Int16>
    {
        public override PageEditorType EditorType => PageEditorType.Short;

        public ShortEditor(short val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
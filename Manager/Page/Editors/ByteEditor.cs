using System;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class ByteEditor : NumericEditor<Byte>
    {
        public override PageEditorType EditorType => PageEditorType.Byte;

        public ByteEditor(byte val, string name, string descrption)
            : base(val, name, descrption)
        {
        }      
    }
}

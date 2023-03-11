using System;

namespace Manager.Page.Editor
{
    class UIntEditor : NumericEditor<UInt32>
    {
        public override PageEditorType EditorType => PageEditorType.Short;

        public UIntEditor(uint val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
using System;

namespace Manager.Page.Editor
{
    class LongEditor : NumericEditor<Int64>
    {
        public override PageEditorType EditorType => PageEditorType.Long;

        public LongEditor(long val, string name, string descrption)
            : base(val, name, descrption)
        {
        }
    }
}
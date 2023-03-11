namespace Manager.Page.Editor
{
    class DecimalEditor : NumericEditor<decimal>
    {
        public DecimalEditor(decimal val, string name, string descrption) 
            : base(val, name, descrption)
        {
        }

        public override PageEditorType EditorType => PageEditorType.Decimal;
    }
}
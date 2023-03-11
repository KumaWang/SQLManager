using System;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{

    class BooleanEditor : BaseEditor<Boolean>
    {
        private CheckBoxExt mRenderer;

        public override PageItemFlags Flags => PageItemFlags.DrawNameBySelf;

        public override PageEditorType EditorType => PageEditorType.Bool;

        public override object Value 
        {
            get { return mRenderer.Checked; }
            set { mRenderer.Checked = (bool)value; }
        }

        public override Control Control => mRenderer;

        public override string Name => mRenderer.Text;

        public override string Descrption { get; }

        public BooleanEditor(bool val, string name, string descrption)
        {
            Descrption = descrption;

            mRenderer = new CheckBoxExt();
            mRenderer.Checked = val;
            mRenderer.Text = name;
            mRenderer.Size = TextRenderer.MeasureText("false", Font);
        }
    }
}

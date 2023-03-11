using System;
using System.Drawing;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class CharEditor : BaseEditor<Char>
    {
        public override PageItemFlags Flags => PageItemFlags.Default;

        public override PageItemType ItemType => PageItemType.Control;

        public override PageEditorType EditorType => PageEditorType.Char;

        private ColorTextBox mView;

        public override Control Control => mView;

        public override Font Font => mView.Font;

        public override object Value 
        {
            get { return mView.Text[0]; }
            set { mView.Text = value.ToString(); }
        }

        public override string Name { get; }

        public override string Descrption { get; }

        public CharEditor(char val, string name, string descrption)
        {
            Name = name;
            Descrption = descrption;

            mView = new ColorTextBox();
            mView.Text = val.ToString();
            mView.Font = new Font("微软雅黑", 11);
            mView.Size = TextRenderer.MeasureText("中 ", mView.Font);
            mView.PreviewKeyDown += MView_PreviewKeyDown;
        }

        private void MView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            mView.Text = string.Empty;
        }
    }
}

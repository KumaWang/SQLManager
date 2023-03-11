using System;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class ClickEditor : BaseEditor<Action>
    {
        public override PageItemFlags Flags => PageItemFlags.DrawNameBySelf;

        public override PageItemType ItemType => PageItemType.Control;

        public override PageEditorType EditorType => PageEditorType.Click;

        private ButtonExt mCtrl;
        private Action mAction;

        public override Control Control => mCtrl;

        public override object Value
        {
            get;
            set;
        }

        public override string Name { get; }

        public override string Descrption { get; }

        public ClickEditor(Action val, string name, string descrption)
        {
            Name = name;
            Descrption = descrption;

            mAction = val;

            mCtrl = new ButtonExt();
            mCtrl.AutoSize = true;
            mCtrl.Text = descrption;
            mCtrl.Click += MCtrl_Click1;
        }

        private void MCtrl_Click1(object sender, EventArgs e)
        {
            if (mAction != null)
                mAction();
        }
    }
}
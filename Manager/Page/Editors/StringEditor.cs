using System;
using System.Drawing;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class StringEditor : IPageEditor<String>
    {
        public PageItemFlags Flags => PageItemFlags.Default;

        public PageItemType ItemType => PageItemType.Control;

        public PageEditorType EditorType => PageEditorType.String;

        private TextBox mCtrl;

        public object Value 
        {
            get { return mCtrl.Text; }
            set { mCtrl.Text = (string)value; }
        }

        public string Name { get; }

        public string Descrption { get; }

        public Control Control => mCtrl;

        public StringEditor(string val, string name, string descrption)
        {
            Name = name;
            Descrption = descrption;

            mCtrl = new TextBox();
            mCtrl.Text = val;
            mCtrl.Size = new Size(200, 32);
        }

        public Size ComputeSize(Size maxSize)
        {
            return mCtrl.Size;
        }
    }
}

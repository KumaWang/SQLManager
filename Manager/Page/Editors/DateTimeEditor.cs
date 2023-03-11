using System;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class DateTimeEditor : BaseEditor<DateTime>
    {
        private DateTimePicker mCtrl;

        public override PageEditorType EditorType => PageEditorType.DateTime;

        public override Control Control => mCtrl;

        public override string Name { get; }

        public override string Descrption { get; }

        public override object Value 
        {
            get { return mCtrl.Value; }
            set 
            {
                var v = DateTime.Now;
                if (value != null && value != default)
                    mCtrl.Value = (DateTime)v;
            }
        }

        public DateTimeEditor(string name, string descrption, DateTime value)
        {
            mCtrl = new DateTimePicker();

            Name = name;
            Descrption = descrption;
            Value = value;
        }
    }
}

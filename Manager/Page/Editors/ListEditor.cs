using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class ListEditor : IPageEditor<IEnumerable<IPageItem>>
    {
        public PageItemFlags Flags => PageItemFlags.Default;

        public PageItemType ItemType => PageItemType.Control;

        public PageEditorType EditorType => PageEditorType.List;

        public IEnumerable<IPageItem> Value 
        {
            get;
            set;
        }

        public string Name { get; }

        public string Descrption { get; }

        public Control Control => throw new NotImplementedException();

        object IPageEditor.Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ListEditor(IEnumerable<IPageItem> value, string name, string descrption)
        {
            Value = value;
            Name = name;
            Descrption = descrption;
        }

        public Size ComputeSize(Size maxSize)
        {
            throw new NotImplementedException();
        }
    }
}

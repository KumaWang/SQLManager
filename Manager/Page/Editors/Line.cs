using System.Drawing;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class Line : IPageEditor
    {
        public PageItemType ItemType => PageItemType.Control;

        public PageItemFlags Flags => PageItemFlags.Default;

        public PageEditorType EditorType => PageEditorType.None;

        public Control Control => mLine;

        public string Name => string.Empty;

        public string Descrption => string.Empty;

        public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private HalvingLineExt mLine;

        public Line(int width) 
        {
            mLine = new HalvingLineExt();
            mLine.Size = new Size(width, 1);
        }

        public Size ComputeSize(Size maxSize)
        {
            return mLine.Size = new Size(maxSize.Width, 1);
        }
    }
}

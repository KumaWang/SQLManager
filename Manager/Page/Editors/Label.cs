using System.Drawing;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class Label : IPageItem
    {
        public PageItemFlags Flags => PageItemFlags.Default;

        public PageItemType ItemType => PageItemType.Control;

        private LabelExt mLabel;

        public string Text => mLabel.Text;

        public Control Control => mLabel;

        public Label(string text, Font font) 
        {
            mLabel = new LabelExt();
            mLabel.Text = text;
            mLabel.Font = font;
        }

        public Size ComputeSize(Size maxSize)
        {
            var size = TextRenderer.MeasureText(Text, mLabel.Font); 

            return new Size(size.Width + 4, size.Height + 4);
        }
    }
}

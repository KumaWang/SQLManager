using System.Drawing;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class Offset : IPageItem
    {
        public PageItemType ItemType => PageItemType.Layout;

        public PageItemFlags Flags => PageItemFlags.Default;

        public Size Value { get; }

        public Control Control => null;

        public Offset(int x, int y) 
        {
            Value = new Size(x, y);
        }

        public Size ComputeSize(Size maxSize)
        {
            return Value;
        }
    }
}

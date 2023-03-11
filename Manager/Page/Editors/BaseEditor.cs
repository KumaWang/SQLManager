using System.Drawing;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    abstract class BaseEditor<T> : IPageEditor<T>
    {
        private static readonly Font sDefaultFont = new Font("微软雅黑", 17f);

        public virtual Font Font => sDefaultFont;

        public virtual PageItemFlags Flags => PageItemFlags.Default;

        public virtual PageItemType ItemType => PageItemType.Control;

        public abstract PageEditorType EditorType { get; }

        public abstract Control Control { get; }

        public abstract object Value { get; set; }

        public abstract string Name { get; }

        public virtual string Descrption { get; }

        public virtual Size ComputeSize(Size maxSize)
        {
            return Control.Size;
        }
    }
}

using System.Windows.Forms;

namespace Manager.Page
{
    public interface IPageItem : IPageSizedItem
    {
        PageItemFlags Flags { get; }

        PageItemType ItemType { get; }

        Control Control { get; }
    }
}

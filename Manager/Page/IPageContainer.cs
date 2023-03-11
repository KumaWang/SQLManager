using System.Collections.Generic;
using System.Drawing;

namespace Manager.Page
{
    public interface IPageContainer : IPageItem
    {
        PageContainerType ContainerType { get; }

        PageContainerDirection Direction { get; }

        IEnumerable<IPageItem> Items { get; }

        Point GetItemLocation(IPageItem item);

        Size GetItemSize(IPageItem item);
    }
}

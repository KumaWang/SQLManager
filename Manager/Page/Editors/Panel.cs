using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Manager.Page.Editor
{
    class Panel : IPageContainer
    {
        private Dictionary<IPageItem, Rectangle> mItemBounds;

        public PageItemFlags Flags => PageItemFlags.Default;

        public PageItemType ItemType => PageItemType.Container;

        public PageContainerType ContainerType => PageContainerType.Panel;

        public PageContainerDirection Direction { get; }

        public IEnumerable<IPageItem> Items { get; }

        public Control Control => throw new NotImplementedException();

        public Panel(PageContainerDirection direction, IEnumerable<IPageItem> items)
        {
            Direction = direction;
            Items = items;
            mItemBounds = new Dictionary<IPageItem, Rectangle>();
        }

        public Size ComputeSize(Size maxSize)
        {
            var x = 0;
            var y = 0;
            var width = 0;
            var height = 0;
            if (Direction == PageContainerDirection.Horizontal)
            {
                foreach (var item in Items) 
                {
                    x = width;
                    var size = item.ComputeSize(maxSize);
                    width = width + size.Width;
                    height = Math.Max(height, size.Height);

                    mItemBounds.Add(item, new Rectangle(x, y, size.Width, size.Height));
                }
            }
            else 
            {
                foreach (var item in Items)
                {
                    y = height;
                    var size = item.ComputeSize(maxSize);
                    width = Math.Max(width, size.Width);
                    height = height + size.Height;

                    mItemBounds.Add(item, new Rectangle(x, y, size.Width, size.Height));
                }
            }


            return new Size(width, height);
        }

        public Point GetItemLocation(IPageItem item)
        {
            if (mItemBounds.ContainsKey(item))
                return mItemBounds[item].Location;

            return default;
        }

        public Size GetItemSize(IPageItem item)
        {
            if (mItemBounds.ContainsKey(item))
                return mItemBounds[item].Size;

            return default;
        }
    }
}

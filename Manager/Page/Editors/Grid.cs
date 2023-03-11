using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    class Grid : IPageContainer, IPageEditor
    {
        private ScrollableControl mControl;
        private IList<GridItem> mItems;
        private Dictionary<IPageItem, Rectangle> mItemCells;

        public PageItemFlags Flags => PageItemFlags.Default;

        public PageItemType ItemType => PageItemType.Container;

        public PageContainerType ContainerType => PageContainerType.Grid;

        public PageContainerDirection Direction => PageContainerDirection.Horizontal;

        public IEnumerable<IPageItem> Items => mItems.Select(x => x.Item);

        public IList<GridLength> Rows { get; }

        public IList<GridLength> Columns { get; }

        public PageEditorType EditorType => PageEditorType.None;

        public string Name => "Grid";

        public string Descrption => "Grid";

        public Control Control => mControl;

        public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Grid(IList<GridLength> rows, IList<GridLength> columns, IEnumerable<GridItem> items) 
        {
            mItemCells = new Dictionary<IPageItem, Rectangle>();

            Rows = rows;
            Columns = columns;
            mItems = items.ToArray();
            mControl = new ScrollableControl();
            mControl.Dock = DockStyle.Fill;

            foreach (var item in items)
                if (item.Item.Control != null)
                    mControl.Controls.Add(item.Item.Control);
        }

        public Size ComputeSize(Size maxSize)
        {
            return ComputeLayout(maxSize);
        }

        public Point GetItemLocation(IPageItem item)
        {
            var cell = GetItemCell(item);
            return cell.Location;
        }

        public Size GetItemSize(IPageItem item)
        {
            var cell = GetItemCell(item);
            return cell.Size;
        }

        public Rectangle GetItemCell(IPageItem item)
        {
            if (mItemCells.ContainsKey(item))
                return mItemCells[item];

            return default;
        }

        public Size ComputeLayout(Size maxSize)
        {
            mItemCells.Clear();

            var autoItems = mItems.Where(x => IsAutoRectangle(x)).ToArray();

            foreach (var item in autoItems)
            {
                if (Rows[item.Row].Type == GridType.Auto)
                    Rows[item.Row] = new GridLength(GridType.Auto, 0);

                if (Columns[item.Column].Type == GridType.Auto)
                    Columns[item.Column] = new GridLength(GridType.Auto, 0);
            }

            foreach (var item in autoItems)
            {
                var size = ComputeSize(item.Item, maxSize);

                if (Rows[item.Row].Type == GridType.Auto)
                    Rows[item.Row] = new GridLength(GridType.Auto, size.Height);

                if (Columns[item.Column].Type == GridType.Auto)
                    Columns[item.Column] = new GridLength(GridType.Auto, size.Width);
            }

            var rects = GetRectangles(maxSize);
            for (var i = 0; i < mItems.Count; i++)
            {
                var item = mItems[i];
                var rect = rects[item.Column + item.Row * Columns.Count];
                mItemCells[item.Item] = rect;
            }

            var right = rects.Max(x => x.Right);
            var bottom = rects.Max(x => x.Bottom);


            for (var i = 0; i < mItems.Count; i++)
            {
                var item = mItems[i];
                var rect = GetItemCell(item.Item);

                var x = rect.Location.X;
                var y = rect.Location.Y; // - (m?.Value ?? 0);

                if (item.Item.Control != null)
                {
                    item.Item.Control.Location = new Point(x, y);
                    item.Item.Control.Size = rect.Size;
                }
            }

            return mControl.Size = new Size(right, bottom);
        }

        private Rectangle[] GetRectangles(Size clientSize)
        {
            var list = new List<Rectangle>();
            var cr = Columns.Where(x => x.Type == GridType.Rate).Sum(x => x.Value);
            var rr = Rows.Where(x => x.Type == GridType.Rate).Sum(x => x.Value);

            var sw = clientSize.Width - Columns.Where(x => x.Type != GridType.Rate).Sum(x => x.Value);
            var sh = clientSize.Height - Rows.Where(x => x.Type != GridType.Rate).Sum(x => x.Value);

            var offsetY = 0.0;
            for (var y = 0; y < Rows.Count; y++)
            {
                var row = Rows[y];
                var height = row.Type == GridType.Rate ? (row.Value / rr) * sh : row.Value;
                double offsetX = 0.0;
                for (var x = 0; x < Columns.Count; x++)
                {
                    var column = Columns[x];
                    var width = column.Type == GridType.Rate ? (column.Value / cr) * sw : column.Value;

                    list.Add(new Rectangle((int)offsetX, (int)offsetY, (int)width, (int)height));
                    offsetX = offsetX + width;
                }

                offsetY = offsetY + height;
            }

            return list.ToArray();
        }

        private bool IsAutoRectangle(GridItem item)
        {
            return Columns[item.Column].Type == GridType.Auto || Rows[item.Row].Type == GridType.Auto;
        }

        private Size ComputeSize(IPageItem item, Size maxSize)
        {      
            var size = item.ComputeSize(maxSize);
            return new Size(Math.Min(size.Width, maxSize.Width), Math.Min(size.Height, maxSize.Height));
        }
    }
}

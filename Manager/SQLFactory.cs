using Manager.Page;
using Manager.Page.Editor;
using System.Collections.Generic;
using System.Drawing;

namespace Manager
{
    static class SQLFactory
    {
        public static SQLEditor CreateEditor(SQLTable table) 
        {
            var dict = new Dictionary<SQLField, IPageEditor>();
            var labelFont = new Font("微软雅黑", 14, FontStyle.Regular);
            var columnCount = 4;
            var rowCount = (table.Fields.Length - 1) / columnCount;
          
            var rows = new List<GridLength>();
            for (var i = 0; i <= rowCount; i++)
            {
                rows.Add(new GridLength(GridType.Pixel, 8));
                rows.Add(new GridLength(GridType.Pixel, 26));
                rows.Add(new GridLength(GridType.Pixel, 8));
            }

            var columns = new List<GridLength>();
            for (var i = 0; i <= columnCount; i++)
            {
                columns.Add(new GridLength(GridType.Rate, 32));
                columns.Add(new GridLength(GridType.Pixel, 4));
                columns.Add(new GridLength(GridType.Rate, 66));
            }

            var items = new List<GridItem>();
            for (var i = 1; i < table.Fields.Length; i++) 
            {
                var col = ((i - 1) % columnCount) * 3;
                var row = ((i - 1) / columnCount) * 3 + 1;
                var field = table.Fields[i];
                var editor = CreateEditor(field);

                if(editor is IPageEditor pageEditor)
                    if(pageEditor.Control != null)
                        dict[field] = pageEditor;

                items.Add(new GridItem(PageFactory.Label(field.Comment, labelFont), col, row));
                items.Add(new GridItem(editor, col + 2, row));
            }

            var grid = PageFactory.Grid(rows, columns, items);
            return new SQLEditor(table, grid, dict);
        }

        private static IPageItem CreateEditor(SQLField field) 
        {
            object defaultValue = null;
            try
            {
                defaultValue = System.Activator.CreateInstance(field.Type.CLRType);
            }
            catch 
            {
            }

            return PageFactory.Create(defaultValue, field.Name, string.Empty, field.Type.CLRType);
        }
    }
}

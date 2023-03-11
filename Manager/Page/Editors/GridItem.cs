namespace Manager.Page.Editor
{
    public struct GridItem
    {
        public IPageItem    Item    { get; }

        public int          Row     { get; }

        public int          Column  { get; }

        public GridItem(IPageItem item, int column, int row) 
        {
            Item = item;
            Row = row;
            Column = column;
        }
    }
}

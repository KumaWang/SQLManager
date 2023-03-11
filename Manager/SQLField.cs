namespace Manager
{
    public class SQLField : SQLExpression, ISQLField
    {
        private string mComment = string.Empty;

        public string Name
        {
            get { return Command; }
        }

        public string Comment 
        {
            get { return mComment; }
        }

        public int Capacity
        {
            get;
            private set;
        }

        public SQLFieldType Type
        {
            get;
            private set;
        }

        public SQLField(string cmd, string comment, string sqlTypeName, SQLFieldTypeKind type, int capacity)
            : this(cmd, comment, new SQLFieldType(sqlTypeName, type, capacity))
        {
        }

        public SQLField(string cmd, string comment, SQLFieldType type)
            : base(cmd)
        {
            Type = type;
            mComment = comment;
        }
    }

    public interface ISQLField
    {
        string              Name        { get; }
        SQLFieldType        Type        { get; }
    }
}

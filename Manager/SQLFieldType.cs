using System;
using System.Collections.Generic;

namespace Manager
{
    public enum SQLFieldTypeKind
    {
        None,
        Boolean,
        Char,
        Byte,
        SByte,
        UInt16,
        UInt32,
        UInt64,
        Int16,
        Int32,
        Int64,
        Float,
        Double,
        String,
        Binary,
        DateTime,
        DateTimeOffset,
        Decimal,
        TimeSpan,
        Guid
    }

    public class SQLFieldType
    {
        public SQLFieldTypeKind Kind            { get; private set; }
        public int              Capacity        { get; private set; }
        public bool             AutoIncrement   { get; set; }
        public bool             PrimaryKey      { get; set; }

        public string           TypeName         { get; }

        public Type             CLRType         
        {
            get { return ToClrType(TypeName); }
        }

        public string           FullName        
        {
            get
            {
                return CLRType.FullName;
            }
        }

        public SQLFieldType(string sqlTypeName, SQLFieldTypeKind kind, int capacity)
            : this(sqlTypeName, kind, capacity, false, false)
        {
        }

        public SQLFieldType(string sqlTypeName, SQLFieldTypeKind kind, int capacity, bool primaryKey, bool autoIncrement)
        {
            TypeName = sqlTypeName;
            Kind = kind;
            Capacity = capacity;
            PrimaryKey = primaryKey;
            AutoIncrement = autoIncrement;
        }

        private readonly static Dictionary<string, SQLFieldTypeKind> MappingKinds;
        private readonly static Dictionary<SQLFieldTypeKind, Type> MappingTypes;

        static SQLFieldType()
        {
            MappingKinds = new Dictionary<string, SQLFieldTypeKind>();
            MappingKinds.Add("bigint", SQLFieldTypeKind.Int64);
            MappingKinds.Add("binary", SQLFieldTypeKind.Binary);
            MappingKinds.Add("bit", SQLFieldTypeKind.Boolean);
            MappingKinds.Add("char", SQLFieldTypeKind.String);
            MappingKinds.Add("date", SQLFieldTypeKind.DateTime);
            MappingKinds.Add("datetime", SQLFieldTypeKind.DateTime);
            MappingKinds.Add("datetime2", SQLFieldTypeKind.DateTime);
            MappingKinds.Add("datetimeoffset", SQLFieldTypeKind.DateTimeOffset);
            MappingKinds.Add("decimal", SQLFieldTypeKind.Decimal);
            MappingKinds.Add("float", SQLFieldTypeKind.Double);
            MappingKinds.Add("image", SQLFieldTypeKind.Binary);
            MappingKinds.Add("int", SQLFieldTypeKind.Int32);
            MappingKinds.Add("money", SQLFieldTypeKind.Decimal);
            MappingKinds.Add("nchar", SQLFieldTypeKind.String);
            MappingKinds.Add("ntext", SQLFieldTypeKind.String);
            MappingKinds.Add("numeric", SQLFieldTypeKind.Decimal);
            MappingKinds.Add("nvarchar", SQLFieldTypeKind.String);
            MappingKinds.Add("real", SQLFieldTypeKind.Float);
            MappingKinds.Add("rowversion", SQLFieldTypeKind.Binary);
            MappingKinds.Add("smalldatetime", SQLFieldTypeKind.DateTime);
            MappingKinds.Add("smallint", SQLFieldTypeKind.Int16);
            MappingKinds.Add("smallmoney", SQLFieldTypeKind.Decimal);
            MappingKinds.Add("text", SQLFieldTypeKind.String);
            MappingKinds.Add("time", SQLFieldTypeKind.TimeSpan);
            MappingKinds.Add("timestamp", SQLFieldTypeKind.Binary);
            MappingKinds.Add("tinyint", SQLFieldTypeKind.Byte);
            MappingKinds.Add("uniqueidentifier", SQLFieldTypeKind.Guid);
            MappingKinds.Add("varbinary", SQLFieldTypeKind.Binary);
            MappingKinds.Add("varchar", SQLFieldTypeKind.String);

            MappingTypes = new Dictionary<SQLFieldTypeKind, Type>();
            MappingTypes[SQLFieldTypeKind.String] = typeof(string);
            MappingTypes[SQLFieldTypeKind.DateTime] = typeof(DateTime);
            MappingTypes[SQLFieldTypeKind.Int16] = typeof(Int16);
            MappingTypes[SQLFieldTypeKind.Decimal] = typeof(Decimal);
            MappingTypes[SQLFieldTypeKind.TimeSpan] = typeof(TimeSpan);
            MappingTypes[SQLFieldTypeKind.UInt32] = typeof(UInt32);
            MappingTypes[SQLFieldTypeKind.UInt64] = typeof(UInt64);
            MappingTypes[SQLFieldTypeKind.UInt16] = typeof(UInt16);
            MappingTypes[SQLFieldTypeKind.Boolean] = typeof(Boolean);
            MappingTypes[SQLFieldTypeKind.Binary] = typeof(byte[]);
            MappingTypes[SQLFieldTypeKind.Guid] = typeof(Guid);
            MappingTypes[SQLFieldTypeKind.Byte] = typeof(Byte);
            MappingTypes[SQLFieldTypeKind.Char] = typeof(Char);
            MappingTypes[SQLFieldTypeKind.SByte] = typeof(SByte);
            MappingTypes[SQLFieldTypeKind.DateTimeOffset] = typeof(DateTimeOffset);
            MappingTypes[SQLFieldTypeKind.Double] = typeof(Double);
            MappingTypes[SQLFieldTypeKind.Float] = typeof(float);
            MappingTypes[SQLFieldTypeKind.Int32] = typeof(Int32);
            MappingTypes[SQLFieldTypeKind.Int64] = typeof(Int64);
        }

        private static SQLFieldTypeKind ToFieldKind(string sqlType) 
        {
            SQLFieldTypeKind kind = SQLFieldTypeKind.None;
            if (MappingKinds.TryGetValue(sqlType, out kind))
                return kind;

            throw new TypeLoadException(string.Format("无法从SQL类型' {0}'中取得对应的SQLFieldTypeKind", sqlType));
        }

        private static Type ToClrType(string sqlType)
        {
            Type datatype = null;
            if (MappingTypes.TryGetValue(ToFieldKind(sqlType), out datatype))
                return datatype;

            throw new TypeLoadException(string.Format("无法从SQL类型' {0}'中取得对应的CLR Type", sqlType));
        }

        public static SQLFieldType Parse(string typeName) 
        {
            return new SQLFieldType(typeName, ToFieldKind(typeName), 0);
        }
    }
}

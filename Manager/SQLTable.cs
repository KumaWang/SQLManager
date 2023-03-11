using System;
using System.Collections.Generic;

namespace Manager
{
    public interface ISQLTable
    {
        string              Name        { get; }
        SQLField[]          Fields      { get; }
        SQLDatabase Database    { get; }
        ISQLValue CreateEmptyValue();
        ISQLValue CreateValue(Dictionary<string, SQLExpression> item);
    }

    public class SQLTable : ISQLTable
    {
        public string               Name
        {
            get;
        }

        public SQLDatabase  Database
        {
            get;
            private set;
        }

        public SQLField[]           Fields
        {
            get;
        }

        public SQLTable(SQLDatabase database, string tableName, SQLField[] fields)
        {
            Name = tableName;
            Database = database;
            Fields = fields;
        }

        public ISQLValue CreateEmptyValue()
        {
            Dictionary<string, SQLExpression> item = new Dictionary<string, SQLExpression>();

            foreach (var field in Fields)
            {
                switch (field.Type.Kind)
                {
                    case SQLFieldTypeKind.Boolean: item[field.Name] = new SQLExpression<Boolean>(true); break;
                    case SQLFieldTypeKind.Byte: item[field.Name] = new SQLExpression<Byte>(0); break;
                    case SQLFieldTypeKind.SByte: item[field.Name] = new SQLExpression<SByte>(0); break;
                    case SQLFieldTypeKind.Char: item[field.Name] = new SQLExpression<Char>(' '); break;
                    case SQLFieldTypeKind.Double: item[field.Name] = new SQLExpression<Double>(0); break;
                    case SQLFieldTypeKind.Float: item[field.Name] = new SQLExpression<Single>(0); break;
                    case SQLFieldTypeKind.Int16: item[field.Name] = new SQLExpression<Int16>(0); break;
                    case SQLFieldTypeKind.Int32: item[field.Name] = new SQLExpression<Int32>(0); break;
                    case SQLFieldTypeKind.Int64: item[field.Name] = new SQLExpression<Int64>(0); break;
                    case SQLFieldTypeKind.UInt16: item[field.Name] = new SQLExpression<UInt16>(0); break;
                    case SQLFieldTypeKind.UInt32: item[field.Name] = new SQLExpression<UInt32>(0); break;
                    case SQLFieldTypeKind.UInt64: item[field.Name] = new SQLExpression<UInt64>(0); break;
                    case SQLFieldTypeKind.String: item[field.Name] = new SQLExpression<char[]>(new char[field.Type.Capacity]); break;
                }
            }

            return CreateValue(item);
        }

        public ISQLValue CreateValue(Dictionary<string, SQLExpression> item)
        {
            return new SQLValue(item);
        }

    }
}

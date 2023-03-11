using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager
{
    public abstract class SQLStatement<T> where T : ISQLTable
    {
        public string               Command { get; private set; }
        public T                    Table   { get; private set; }
        public SQLStatement<T>      Last    { get; private set; }

        public SQLStatement(string cmd, T table, SQLStatement<T> last)                                                                  
        {
            Command = cmd;
            Table = table;
            Last = last;
        }
    }

    public class SQLFisrtStatement<T> : SQLStatement<T> where T : ISQLTable
    {
        public SQLFisrtStatement(string cmd, T table, SQLStatement<T> last)                                                             
            : base(cmd, table, last)
        {
        }
    }

    public class SQLNextStatement<T> : SQLFisrtStatement<T> where T : ISQLTable
    {
        public SQLNextStatement(string cmd, T table, SQLStatement<T> last)                                                              
            : base(cmd, table, last)
        {
        }
    }

    public class SQLEndStatement<T> : SQLNextStatement<T> where T : ISQLTable
    {
        public SQLEndStatement(string cmd, T table, SQLStatement<T> last)                                                               
            : base(cmd, table, last)
        {
        }
    }

    public class SQLSelectStatement<T> : SQLFisrtStatement<T> where T : ISQLTable
    {
        public SQLField[] Selector { get; private set; }

        public SQLSelectStatement(string cmd, T table, SQLStatement<T> last, params SQLField[] selector)                                
         : base(cmd, table, last)
        {
            Selector = selector;
        }
    }

    public static class SQLStatementLinq
    {
        public static IEnumerable<ISQLValue> GetResult<T>(this SQLStatement<T> statement)                                               where T : ISQLTable
        {
            using (var reader = statement.Table.Database.RunCommand(statement.Command))
            {
                var curr = statement;
                do
                {
                    if (curr is SQLSelectStatement<T>)
                    {
                        break;
                    }
                }
                while ((curr = curr.Last) != null);

                var selectStatement = curr as SQLSelectStatement<T>;
                if (selectStatement != null)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, SQLExpression> item = new Dictionary<string, SQLExpression>();
                        for (var i = 0; i < selectStatement.Selector.Length; i++)
                        {
                            var selector = selectStatement.Selector[i];
                            var field = statement.Table.Fields.FirstOrDefault(X => X.Name == selector.Name);
                            if (field.Equals(null))
                                throw new NotImplementedException(string.Format("{0} maybe value is empty", field.Name));

                            try
                            {
                                switch (field.Type.Kind)
                                {
                                    case SQLFieldTypeKind.Boolean: item[field.Name] = new SQLExpression<bool>(reader.GetBoolean(field.Name)); break;
                                    case SQLFieldTypeKind.Byte: item[field.Name] = new SQLExpression<byte>(reader.GetByte(field.Name)); break;
                                    case SQLFieldTypeKind.SByte: item[field.Name] = new SQLExpression<sbyte>(reader.GetSByte(field.Name)); break;
                                    case SQLFieldTypeKind.Char: item[field.Name] = new SQLExpression<char>(reader.GetChar(field.Name)); break;
                                    case SQLFieldTypeKind.Double: item[field.Name] = new SQLExpression<double>(reader.GetDouble(field.Name)); break;
                                    case SQLFieldTypeKind.Float: item[field.Name] = new SQLExpression<float>(reader.GetFloat(field.Name)); break;
                                    case SQLFieldTypeKind.Int16: item[field.Name] = new SQLExpression<Int16>(reader.GetInt16(field.Name)); break;
                                    case SQLFieldTypeKind.Int32: item[field.Name] = new SQLExpression<Int32>(reader.GetInt32(field.Name)); break;
                                    case SQLFieldTypeKind.Int64: item[field.Name] = new SQLExpression<Int64>(reader.GetInt64(field.Name)); break;
                                    case SQLFieldTypeKind.UInt16: item[field.Name] = new SQLExpression<UInt16>(reader.GetUInt16(field.Name)); break;
                                    case SQLFieldTypeKind.UInt32: item[field.Name] = new SQLExpression<UInt32>(reader.GetUInt32(field.Name)); break;
                                    case SQLFieldTypeKind.UInt64: item[field.Name] = new SQLExpression<UInt64>(reader.GetUInt64(field.Name)); break;
                                    case SQLFieldTypeKind.String: item[field.Name] = new SQLExpression<String>(reader.GetString(field.Name)); break;
                                }
                            }
                            catch (Exception e)
                            {
                                throw new NotImplementedException(string.Format("{0} maybe value is empty", field.Name));
                            }
                        }

                        yield return statement.Table.CreateValue(item);
                    }
                }

                reader.Close();
            }
        }

        public static SQLSelectStatement<T> Select<T>(this T table, params SQLField[] selector)                                         where T : ISQLTable
        {
            return new SQLSelectStatement<T>(string.Format(@"SELECT {1} FROM {0}", table.Name, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Name))), table, null, selector.Length == 0 ? table.Fields : selector);
        }

        public static SQLFisrtStatement<T> Delete<T>(this T table, params SQLField[] selector)                                          where T : ISQLTable
        {
            return new SQLFisrtStatement<T>(string.Format(@"DELETE {1} FROM {0}", table.Name, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Name))), table, null);
        }

        public static SQLFisrtStatement<T> Insert<T>(this T table, params SQLExpression[] selector)                                     where T : ISQLTable
        {
            return new SQLFisrtStatement<T>(string.Format(@"INSERT INTO {0} ({1}) VALUES ({2})",
                table.Name,
                string.Join(",", selector.Select(X => X.Command.Split('>').FirstOrDefault()).ToArray()),
                string.Join(",", selector.Select(X => X.Command.Split('>').LastOrDefault()).ToArray())), table, null);
        }

        public static SQLFisrtStatement<T> Update<T>(this T table, params SQLExpression[] selector)                                     where T : ISQLTable
        {
            return new SQLFisrtStatement<T>(string.Format(@"UPDATE {0} SET {1}", table.Name, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Command)).Replace(">", "=")), table, null);
        }

        public static SQLFisrtStatement<T> Replace<T>(this T table, params SQLExpression[] selector)                                    where T : ISQLTable
        {
            return new SQLFisrtStatement<T>(string.Format(@"REPLACE INTO {0} SET {1}", table.Name, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Command))), table, null);
        }

        public static SQLNextStatement<T> Where<T>(this SQLFisrtStatement<T> statement, Func<T, SQLExpression> selector)                where T : class, ISQLTable
        {
            var exp = selector(statement.Table as T);
            return new SQLNextStatement<T>(string.Format(@"{0} WHERE {1}", statement.Command, exp.Command), statement.Table, statement);
        }

        public static SQLEndStatement<T> OrderBy<T>(this SQLNextStatement<T> statement, params SQLField[] selector)                     where T : ISQLTable
        {
            return new SQLEndStatement<T>(string.Format(@"{0} ORDER BY {1} ASC", statement.Command, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Command))), statement.Table, statement);
        }

        public static SQLEndStatement<T> OrderByDescending<T>(this SQLNextStatement<T> statement, params SQLField[] selector)           where T : ISQLTable
        {
            return new SQLEndStatement<T>(string.Format(@"{0} ORDER BY {1} DESC", statement.Command, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Command))), statement.Table, statement);
        }

        public static SQLEndStatement<T> Limit<T>(this SQLNextStatement<T> statement, SQLExpression startRow, SQLExpression count)      where T : ISQLTable
        {
            return new SQLEndStatement<T>(string.Format(@"{0} LIMIT {1}", statement.Command, startRow.Command + ((count.Equals(null) ? string.Empty : "," + count.Command))), statement.Table, statement);
        }

        public static SQLEndStatement<T> GroupBy<T>(this SQLNextStatement<T> statement, params SQLField[] selector)                     where T : ISQLTable
        {
            return new SQLEndStatement<T>(string.Format(@"{0} GROUP BY {1}", statement.Command, selector.Length == 0 ? "*" : string.Join(",", selector.Select(X => X.Command))), statement.Table, statement);
        }

        public static SQLEndStatement<T> Having<T>(this SQLNextStatement<T> statement, Func<T, SQLExpression> selector)                 where T : class, ISQLTable
        {
            var exp = selector(statement.Table as T);
            return new SQLEndStatement<T>(string.Format(@"{0} HAVING {1}", statement.Command, exp.Command), statement.Table, statement);
        }
    }
}

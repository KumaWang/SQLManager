using System.Runtime.CompilerServices;

namespace Manager
{
    public class SQLExpression          
    {
        public virtual string Command { get; private set; }

        public SQLExpression(string cmd)                                                            
        {
            Command = cmd;
        }

        public SQLExpression()
        {
            Command = string.Empty;
        }

        public override bool Equals(object obj)                                                     
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()                                                           
        {
            return base.GetHashCode();
        }

        [SpecialName]
        public static SQLExpression op_RightShift(SQLExpression left, SQLExpression right)          
        {
            return new SQLExpression(string.Format("{0}={1}", left.Command, right.Command));
        }

        public static SQLExpression operator ==(SQLExpression left, SQLExpression right)            
        {
            return new SQLExpression(string.Format("{0}={1}", left.Command, right.Command));
        }

        public static SQLExpression operator !=(SQLExpression left, SQLExpression right)            
        {
            return new SQLExpression(string.Format("{0}!={1}", left.Command, right.Command));
        }

        public static SQLExpression operator >=(SQLExpression left, SQLExpression right)            
        {
            return new SQLExpression(string.Format("{0}>={1}", left.Command, right.Command));
        }

        public static SQLExpression operator <=(SQLExpression left, SQLExpression right)            
        {
            return new SQLExpression(string.Format("{0}<={1}", left.Command, right.Command));
        }

        public static SQLExpression operator >(SQLExpression left, SQLExpression right)             
        {
            return new SQLExpression(string.Format("{0}>{1}", left.Command, right.Command));
        }

        public static SQLExpression operator <(SQLExpression left, SQLExpression right)             
        {
            return new SQLExpression(string.Format("{0}<{1}", left.Command, right.Command));
        }

        public static SQLExpression operator +(SQLExpression left, SQLExpression right)             
        {
            return new SQLExpression(string.Format("{0}+{1}", left.Command, right.Command));
        }

        public static SQLExpression operator -(SQLExpression left, SQLExpression right)             
        {
            return new SQLExpression(string.Format("{0}-{1}", left.Command, right.Command));
        }

        public static SQLExpression operator *(SQLExpression left, SQLExpression right)             
        {
            return new SQLExpression(string.Format("{0}*{1}", left.Command, right.Command));
        }

        public static SQLExpression operator /(SQLExpression left, SQLExpression right)             
        {
            return new SQLExpression(string.Format("{0}/{1}", left.Command, right.Command));
        }

        public static SQLExpression operator &(SQLExpression left, SQLExpression right)
        {
            return new SQLExpression(string.Format("{0} AND {1}", left.Command, right.Command));
        }


        public static implicit operator SQLExpression(bool value)                                   
        {
            return new SQLExpression(value ? "true" : "false");
        }

        public static implicit operator SQLExpression(char value)                                   
        {
            return new SQLExpression("'" + value.ToString() + "'");
        }

        public static implicit operator SQLExpression(byte value)                                   
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(sbyte value)                                  
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(short value)                                  
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(ushort value)                                 
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(int value)                                    
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(uint value)                                   
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(long value)                                   
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(ulong value)                                  
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(float value)                                  
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(double value)                                 
        {
            return new SQLExpression(value.ToString());
        }

        public static implicit operator SQLExpression(string value)                                 
        {
            return new SQLExpression("'" + value + "'");
        }

        public static implicit operator bool(SQLExpression value)                                   
        {
            return bool.Parse(value.Command);
        }

        public static implicit operator char(SQLExpression value)                                   
        {
            return char.Parse(value.Command.Substring(1, value.Command.Length - 2));
        }

        public static implicit operator byte(SQLExpression value)                                   
        {
            return byte.Parse(value.Command);
        }

        public static implicit operator sbyte(SQLExpression value)                                  
        {
            return sbyte.Parse(value.Command);
        }

        public static implicit operator short(SQLExpression value)                                  
        {
            return short.Parse(value.Command);
        }

        public static implicit operator ushort(SQLExpression value)                                 
        {
            return ushort.Parse(value.Command);
        }

        public static implicit operator int(SQLExpression value)                                    
        {
            return int.Parse(value.Command);
        }

        public static implicit operator uint(SQLExpression value)                                   
        {
            return uint.Parse(value.Command);
        }

        public static implicit operator long(SQLExpression value)                                   
        {
            return long.Parse(value.Command);
        }

        public static implicit operator ulong(SQLExpression value)                                  
        {
            return ulong.Parse(value.Command);
        }

        public static implicit operator float(SQLExpression value)                                  
        {
            return float.Parse(value.Command);
        }

        public static implicit operator double(SQLExpression value)                                 
        {
            return double.Parse(value.Command);
        }

        public static implicit operator string(SQLExpression value)                                 
        {
            return value.Command.Substring(1, value.Command.Length - 2);
        }
    }

    public class SQLExpression<T>       : SQLExpression
    {
        public T Value { get; private set; }

        public SQLExpression(T value)                                                               
            : base(value is string ? "'" + value.ToString() + "'" : value.ToString())
        {
            Value = value;
        }

        public SQLExpression(string cmd, T value)                                                   
           : base(cmd)
        {
            Value = value;
        }
    }
}

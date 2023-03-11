using Manager.Page.Editor;
using System;
using System.Collections.Generic;
using System.Drawing;
using Label = Manager.Page.Editor.Label;

namespace Manager.Page
{
    public static class PageFactory
    {
        private static readonly Dictionary<Type, Delegate> sCreateMappings;

        static PageFactory() 
        {
            sCreateMappings = new Dictionary<Type, Delegate>();
            sCreateMappings[typeof(string)] = new Func<string, string, string, IPageEditor<string>>(String);
            sCreateMappings[typeof(byte)] = new Func<byte, string, string, IPageEditor<byte>>(Byte);
            sCreateMappings[typeof(sbyte)] = new Func<sbyte, string, string, IPageEditor<sbyte>>(SByte);
            sCreateMappings[typeof(short)] = new Func<short, string, string, IPageEditor<short>>(Short);
            sCreateMappings[typeof(int)] = new Func<int, string, string, IPageEditor<int>>(Int);
            sCreateMappings[typeof(long)] = new Func<long, string, string, IPageEditor<long>>(Long);
            sCreateMappings[typeof(ushort)] = new Func<ushort, string, string, IPageEditor<ushort>>(UShort);
            sCreateMappings[typeof(uint)] = new Func<uint, string, string, IPageEditor<uint>>(UInt);
            sCreateMappings[typeof(ulong)] = new Func<ulong, string, string, IPageEditor<ulong>>(ULong);
            sCreateMappings[typeof(float)] = new Func<float, string, string, IPageEditor<float>>(Single);
            sCreateMappings[typeof(double)] = new Func<double, string, string, IPageEditor<double>>(Double);
            sCreateMappings[typeof(decimal)] = new Func<decimal, string, string, IPageEditor<decimal>>(Decimal);
            sCreateMappings[typeof(DateTime)] = new Func<DateTime, string, string, IPageEditor<DateTime>>(DateTime);
            sCreateMappings[typeof(Action)] = new Func<Action, string, string, IPageEditor<Action>>(Click);
        }

        public static IPageItem Create(object value, string name, string descrption, Type type) 
        {
            Delegate func = null;
            if (!sCreateMappings.TryGetValue(type, out func))
                throw new TypeLoadException(string.Format("不支持的编辑器类型 {0}", type.Name));

            return func.DynamicInvoke(value, name, descrption) as IPageItem;
        }

        // ================== Layout ======================
        public static IPageItem Offset(int x, int y) 
        {
            return new Offset(x, y);
        }

        // ================= Drawable =====================

        public static IPageItem Label(string text, Font font) 
        {
            return new Label(text, font);
        }

        public static IPageItem Line() 
        {
            return new Line(1);
        }

        // ================= Container ====================

        public static IPageContainer Panel(PageContainerDirection direction, params IPageItem[] items)
        {
            return new Editor.Panel(direction, items);
        }

        public static IPageContainer Panel(PageContainerDirection direction, IEnumerable<IPageItem> items) 
        {
            return new Editor.Panel(direction, items);
        }
   
        public static IPageContainer Grid(IList<GridLength> rows, IList<GridLength> columns, IEnumerable<GridItem> items)
        {
            return new Editor.Grid(rows, columns, items);
        }
        
        // ================== Editor ======================


        public static IPageEditor<String> String(String val, string name, string descrption = null) 
        {
            return new StringEditor(val, name, descrption);
        }

        public static IPageEditor<Boolean> Boolean(Boolean val, string name, string descrption = null)
        {
            return new BooleanEditor(val, name, descrption);
        }

        public static IPageEditor<Char> Char(Char val, string name, string descrption = null)
        {
            return new CharEditor(val, name, descrption);
        }

        public static IPageEditor<Byte> Byte(Byte val, string name, string descrption = null)
        {
            return new ByteEditor(val, name, descrption);
        }

        public static IPageEditor<SByte> SByte(SByte val, string name, string descrption = null)
        {
            return new SByteEditor(val, name, descrption);
        }

        public static IPageEditor<Int16> Short(Int16 val, string name, string descrption = null)
        {
            return new ShortEditor(val, name, descrption);
        }

        public static IPageEditor<UInt16> UShort(UInt16 val, string name, string descrption = null)
        {
            return new UShortEditor(val, name, descrption);
        }

        public static IPageEditor<Int32> Int(Int32 val, string name, string descrption = null)
        {
            return new IntEditor(val, name, descrption);
        }

        public static IPageEditor<UInt32> UInt(UInt32 val, string name, string descrption = null)
        {
            return new UIntEditor(val, name, descrption);
        }

        public static IPageEditor<Int64> Long(Int64 val, string name, string descrption = null)
        {
            return new LongEditor(val, name, descrption);
        }

        public static IPageEditor<UInt64> ULong(UInt64 val, string name, string descrption = null)
        {
            return new ULongEditor(val, name, descrption);
        }

        public static IPageEditor<Single> Single(Single val, string name, string descrption = null)
        {
            return new FloatEditor(val, name, descrption);
        }

        public static IPageEditor<Double> Double(Double val, string name, string descrption = null)
        {
            return new DoubleEditor(val, name, descrption);
        }

        public static IPageEditor<Decimal> Decimal(Decimal val, string name, string descrption = null) 
        {
            return new DecimalEditor(val, name, descrption);
        }

        public static IPageEditor<Action> Click(Action val, string name, string descrption = null) 
        {
            return new ClickEditor(val, name, descrption);
        }

        public static IPageEditor<IEnumerable<string>> SingleSelect(IEnumerable<string> val, string name, string descrption = null)
        {
            throw new NotImplementedException();
        }

        public static IPageEditor<IEnumerable<string>> MuiltSelect(IEnumerable<string> val, string name, string descrption = null)
        {
            throw new NotImplementedException();
        }
        

        public static IPageEditor<IEnumerable<IPageItem>> List(IEnumerable<IPageItem> val, string name, string descrption = null) 
        {
            return new ListEditor(val, name, descrption);
        }

        public static IPageEditor<DateTime> DateTime(DateTime val, string name, string descrption = null)
        {
            return new DateTimeEditor(name, descrption, val);
        }
    }
}

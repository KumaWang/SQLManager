using System;
using System.ComponentModel;
using System.Windows.Forms;
using WinformControlLibraryExtension;

namespace Manager.Page.Editor
{
    abstract class NumericEditor<T> : BaseEditor<T> 
    {
        private TextBox mCtrl;

        private decimal mMax;
        private decimal mMin;

        public override object Value 
        {
            get 
            {
                return ConvertFromString(mCtrl.Text);
            }
            set 
            {
                if (!decimal.TryParse(value.ToString(), out decimal d)) 
                {
                    throw new Exception("输入值不是有效数据。");
                }

                mCtrl.Text = ConvertFromString(d.ToString()).ToString();
            }
        }

        public override Control Control => mCtrl;

        public override string Name { get; }

        public override string Descrption { get; }

        public NumericEditor(T val, string name, string descrption)
        {
            Name = name;
            Descrption = descrption;

            var type = typeof(T);

            mCtrl = new TextBox();
            //mCtrl.TextChanged += MCtrl_TextChanged;
            //mMax = decimal.Parse(type.GetProperty("MaxValue").GetValue(null).ToString());
            //mMin = decimal.Parse(type.GetProperty("MinValue").GetValue(null).ToString());
            var size = TextRenderer.MeasureText("11111 ", Font);
            mCtrl.Size = new System.Drawing.Size(200, 28);
            Value = val;
        }

        private void MCtrl_TextChanged(object sender, EventArgs e)
        {
            mCtrl.TextChanged -= MCtrl_TextChanged;
            if (!decimal.TryParse(mCtrl.Text, out decimal d))
            {
                throw new Exception("输入值不是有效数据。");
            }

            if (d < mMin) d = mMin;
            if (d > mMax) d = mMax;

            mCtrl.Text = d.ToString();
            mCtrl.TextChanged += MCtrl_TextChanged;
        }

        static T ConvertFromString(string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null && !string.IsNullOrEmpty(value))
            {
                try
                {
                    return (T)converter.ConvertFrom(value);
                }
                catch (Exception e) // Unfortunately Converter throws general Exception
                {
                    throw new Exception("输入值不是有效数据。");
                }
            }

            return default;
        }
    }
}

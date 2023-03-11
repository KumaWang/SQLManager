using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public interface ISQLValue
    {
    }

    public class SQLValue : ISQLValue
    {
        private Dictionary<string, SQLExpression> item;

        public SQLValue(Dictionary<string, SQLExpression> item)
        {
            this.item = item;
        }
    }
}

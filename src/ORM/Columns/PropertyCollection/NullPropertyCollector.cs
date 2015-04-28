using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyCollection
{
    public class NullPropertyCollector : PropertyCollector
    {
        public PropertyInfo[] Collect(Type type)
        {
            return new PropertyInfo[0];
        }
    }
}

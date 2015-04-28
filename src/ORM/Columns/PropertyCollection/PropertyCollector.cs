using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.PropertyCollection
{
    public interface PropertyCollector
    {
        PropertyInfo[] Collect(Type type);
    }
}

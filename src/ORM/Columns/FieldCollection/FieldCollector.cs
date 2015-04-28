using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.FieldCollection
{
    public interface FieldCollector
    {
        FieldInfo[] Collect(Type type);
    }
}

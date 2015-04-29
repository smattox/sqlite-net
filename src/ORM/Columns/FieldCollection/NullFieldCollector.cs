using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns.FieldCollection
{
    public class NullFieldCollector : FieldCollector
    {
        public FieldInfo[] Collect(Type type)
        {
            return new FieldInfo[0];
        }
    }
}

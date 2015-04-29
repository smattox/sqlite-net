using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public interface TableMappingColumnFactory
    {
        TableMappingColumn CreateColumnOnProperty(PropertyInfo property, CreateFlags flags);

        TableMappingColumn CreateColumnOnField(FieldInfo field, CreateFlags flags);
    }
}

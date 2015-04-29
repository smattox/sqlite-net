using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class BasicTableMappingColumnFactory : TableMappingColumnFactory
    {
        public TableMappingColumn[] CreateColumnsOnProperty(PropertyInfo property, CreateFlags createFlags)
        {
            return new TableMappingColumn[] { new PropertyTypeTableMappingColumn(property, createFlags) };
        }

        public TableMappingColumn[] CreateColumnsOnField(FieldInfo field, CreateFlags createFlags)
        {
            return new TableMappingColumn[] { new FieldTypeTableMappingColumn(field, createFlags) };
        }
    }
}

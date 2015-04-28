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
        public TableMappingColumn CreateColumnOnProperty(PropertyInfo property, CreateFlags createFlags)
        {
            return new PropertyTypeTableMappingColumn(property, createFlags);
        }
    }
}

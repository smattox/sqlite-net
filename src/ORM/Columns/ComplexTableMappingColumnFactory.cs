using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class ComplexTableMappingColumnFactory : TableMappingColumnFactory
    {
        public TableMappingColumn[] CreateColumnsOnProperty(PropertyInfo property, CreateFlags createFlags)
        {
            return Analyze(property, createFlags);
        }

        public TableMappingColumn[] CreateColumnsOnField(FieldInfo field, CreateFlags createFlags)
        {
            return Analyze(field, createFlags);
        }

        private TableMappingColumn[] Analyze(MemberInfo info, CreateFlags flags)
        {
            return new TableMappingColumn[0];
        }
    }
}

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
        public TableMappingColumn[] CreateColumnsOnMember(MemberInfo info, TableMappingConfiguration configuration, CreateFlags createFlags, string path)
        {
            if (info is PropertyInfo)
            {
                return new TableMappingColumn[] { new PropertyTypeTableMappingColumn(info as PropertyInfo, path, createFlags) };
            }
            if (info is FieldInfo)
            {
                return new TableMappingColumn[] { new FieldTypeTableMappingColumn(info as FieldInfo, path, createFlags) };
            }
            throw new InvalidOperationException("What is " + info.Name + "?");
        }
    }
}

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
        public TableMappingColumn[] CreateColumnsOnMember(MemberInfo info, CreateFlags createFlags)
        {
            if (info is PropertyInfo)
            {
                return new TableMappingColumn[] { new PropertyTypeTableMappingColumn(info as PropertyInfo, createFlags) };
            }
            if (info is FieldInfo)
            {
                return new TableMappingColumn[] { new FieldTypeTableMappingColumn(info as FieldInfo, createFlags) };
            }
            throw new InvalidOperationException("What is " + info.Name + "?");
        }
    }
}

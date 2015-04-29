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
        TableMappingColumn[] CreateColumnsOnMember(
            MemberInfo info,
            TableMappingConfiguration configuration,
            CreateFlags flags,
            string path);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public interface TableMappingColumn
    {
        string Name { get; }

        Type TargetType { get; }

        string TargetName { get; }

        IEnumerable<IndexedAttribute> Indices { get; set; }
    }
}

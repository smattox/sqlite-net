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

        bool IsAutoInc { get; }

        bool IsPK { get; }

        bool IsNullable { get; }

        bool IsAutoGuid { get; }

        string Collation { get; }

        int? MaxStringLength { get; }

        bool CanWrite { get; }

        void SetValue(object obj, object val);

        IEnumerable<IndexedAttribute> Indices { get; set; }

        object GetValue(object obj);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public abstract class AbstractTableMappingColumn : TableMappingColumn
    {
        public string Name { get; protected set; }

        public Type TargetType { get; protected set; }

        public string TargetName { get; protected set; }

        public bool IsAutoInc { get; protected set; }

        public bool IsPK { get; protected set; }

        public bool IsNullable { get; protected set; }

        public bool IsAutoGuid { get; protected set; }

        public string Collation { get; protected set; }

        public abstract int? MaxStringLength { get; protected set; }

        public IEnumerable<IndexedAttribute> Indices { get; set; }



        public abstract void SetValue(object obj, object val);

        public abstract object GetValue(object obj);
    }
}

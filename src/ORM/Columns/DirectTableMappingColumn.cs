using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public interface DirectTableMappingColumn : TableMappingColumn
    {
        bool IsAutoInc { get; }

        bool IsPK { get; }

        bool IsNullable { get; }

        bool IsAutoGuid { get; }

        string Collation { get; }

        int? MaxStringLength { get; }



        void SetValue(object obj, object val);

        object GetValue(object obj);
    }
}

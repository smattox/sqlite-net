using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public interface IndirectTableMappingColumn : TableMappingColumn
    {
        void SaveValueToDatabase(object obj);

        object LoadValueFromDatabase(object obj);
    }
}

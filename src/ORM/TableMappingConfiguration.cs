using SQLite.ORM.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public class TableMappingConfiguration
    {
        public TableMappingColumnFactory TableMappingColumnFactory { get; set; }

        public TableMappingConfiguration()
        {
            TableMappingColumnFactory = new BasicTableMappingColumnFactory();
        }
    }
}

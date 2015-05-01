using SQLite.ORM.Columns;
using SQLite.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public interface TableMapping : IDisposable
    {
        Type MappedType { get; }

        string TableName { get; }

        TableMappingColumn[] Columns { get; }

        DirectTableMappingColumn[] DirectColumns { get; }

        IndirectTableMappingColumn[] IndirectColumns { get; }

        DirectTableMappingColumn PrimaryKey { get; }

        string GetByPrimaryKeySql { get; }

        TableMappingColumn[] InsertColumns { get; }

        TableMappingColumn[] InsertOrReplaceColumns { get; }

        bool HasAutoIncPK { get; }

        bool HasAutoGuid { get; }


        TableMappingColumn FindColumn(string name);

        TableMappingColumn FindColumnWithTargetName(string targetName);

        PreparedSqlLiteInsertCommand GetInsertCommand(SQLiteConnection conn, string extra);
    }
}

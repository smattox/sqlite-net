using SQLite.ORM.Columns;
using SQLite.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !USE_SQLITEPCL_RAW
using System.Runtime.InteropServices;
#endif
#if NO_CONCURRENT
using ConcurrentStringDictionary = System.Collections.Generic.Dictionary<string, object>;
using SQLite.Extensions;
#else
using ConcurrentStringDictionary = System.Collections.Concurrent.ConcurrentDictionary<string, object>;
using SQLite.ORM.TableAttributeCollection;
using SQLite.ORM.Columns.PropertyCollection;
using SQLite.ORM.Columns.PropertyAttributeChecker;
#endif

namespace SQLite.ORM
{
    public class AbstractTableMapping : TableMapping
    {
        public Type MappedType { get; protected set; }

        public string TableName { get; protected set; }

        public TableMappingColumn[] Columns { get; protected set; }

        public DirectTableMappingColumn PrimaryKey { get; protected set; }

        public string GetByPrimaryKeySql { get; protected set; }

        protected ConcurrentStringDictionary _insertCommandMap;

        protected DirectTableMappingColumn _autoPk;

        protected TableMappingColumn[] _insertColumns;
        public TableMappingColumn[] InsertColumns
        {
            get
            {
                if (_insertColumns == null)
                {
                    _insertColumns = Columns.Where(c => c is DirectTableMappingColumn &&
                        !(c as DirectTableMappingColumn).IsAutoInc).ToArray();
                }
                return _insertColumns;
            }
        }


        protected TableMappingColumn[] _insertOrReplaceColumns;
        public TableMappingColumn[] InsertOrReplaceColumns
        {
            get
            {
                if (_insertOrReplaceColumns == null)
                {
                    _insertOrReplaceColumns = Columns.ToArray();
                }
                return _insertOrReplaceColumns;
            }
        }

        public DirectTableMappingColumn[] DirectColumns
        {
            get
            {
                return Columns.Where(col => col is DirectTableMappingColumn).Select(col => col as DirectTableMappingColumn).ToArray();
            }
        }

        public IndirectTableMappingColumn[] IndirectColumns
        {
            get
            {
                return Columns.Where(col => col is IndirectTableMappingColumn).Select(col => col as IndirectTableMappingColumn).ToArray();
            }
        }

        public bool HasAutoIncPK
        {
            get
            {
                return _autoPk != null;
            }
        }

        public bool HasAutoGuid
        {
            get
            {
                return HasAutoIncPK && _autoPk.IsAutoGuid;
            }
        }

        public TableMappingColumn FindColumn(string name)
        {
            return Columns.FirstOrDefault(column => column.Name.Equals(name));
        }

        public TableMappingColumn FindColumnWithTargetName(string name)
        {
            return Columns.FirstOrDefault(column => column.TargetName.Equals(name));
        }

        public PreparedSqlLiteInsertCommand GetInsertCommand(SQLiteConnection conn, string extra)
        {
            object prepCmdO;

            if (!_insertCommandMap.TryGetValue(extra, out prepCmdO))
            {
                var prepCmd = CreateInsertCommand(conn, extra);
                prepCmdO = prepCmd;
                if (!_insertCommandMap.TryAdd(extra, prepCmd))
                {
                    // Concurrent add attempt beat us.
                    prepCmd.Dispose();
                    _insertCommandMap.TryGetValue(extra, out prepCmdO);
                }
            }
            return (PreparedSqlLiteInsertCommand)prepCmdO;
        }

        PreparedSqlLiteInsertCommand CreateInsertCommand(SQLiteConnection conn, string extra)
        {
            var cols = InsertColumns.Where(column => column is DirectTableMappingColumn);
            string insertSql;
            if (!cols.Any() && DirectColumns.Count() == 1 && DirectColumns[0].IsAutoInc)
            {
                insertSql = string.Format("insert {1} into \"{0}\" default values", TableName, extra);
            }
            else
            {
                var replacing = string.Compare(extra, "OR REPLACE", StringComparison.OrdinalIgnoreCase) == 0;

                if (replacing)
                {
                    cols = InsertOrReplaceColumns.Where(column => column is DirectTableMappingColumn);
                }

                insertSql = string.Format("insert {3} into \"{0}\"({1}) values ({2})", TableName,
                                   string.Join(",", (from c in cols
                                                     select "\"" + c.Name + "\"").ToArray()),
                                   string.Join(",", (from c in cols
                                                     select "?").ToArray()), extra);

            }

            var insertCommand = new PreparedSqlLiteInsertCommand(conn);
            insertCommand.CommandText = insertSql;
            return insertCommand;
        }

        public void Dispose()
        {
            foreach (var pair in _insertCommandMap)
            {
                ((PreparedSqlLiteInsertCommand)pair.Value).Dispose();
            }
            _insertCommandMap = null;
        }
    }
}

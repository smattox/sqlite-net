using SQLite.ORM;
using SQLite.ORM.Columns;
using SQLite.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Exceptions
{
    public class NotNullConstraintViolationException : SQLiteException
    {
        public IEnumerable<TableMappingColumn> Columns { get; protected set; }

        protected NotNullConstraintViolationException(SQLite3.Result r, string message)
            : this(r, message, null, null)
        {

        }

        protected NotNullConstraintViolationException(SQLite3.Result r, string message, TableMapping mapping, object obj)
            : base(r, message)
        {
            if (mapping != null && obj != null)
            {
                this.Columns = from c in mapping.DirectColumns
                               where c.IsNullable == false && c.GetValue(obj) == null
                               select c;
            }
        }

        public static new NotNullConstraintViolationException New(SQLite3.Result r, string message)
        {
            return new NotNullConstraintViolationException(r, message);
        }

        public static NotNullConstraintViolationException New(SQLite3.Result r, string message, TableMapping mapping, object obj)
        {
            return new NotNullConstraintViolationException(r, message, mapping, obj);
        }

        public static NotNullConstraintViolationException New(SQLiteException exception, TableMapping mapping, object obj)
        {
            return new NotNullConstraintViolationException(exception.Result, exception.Message, mapping, obj);
        }
    }
}

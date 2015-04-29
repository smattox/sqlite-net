using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if USE_CSHARP_SQLITE
using Sqlite3Statement = Community.CsharpSqlite.Sqlite3.Vdbe;
#elif USE_WP8_NATIVE_SQLITE
using Sqlite3Statement = Sqlite.Statement;
#elif USE_SQLITEPCL_RAW
using Sqlite3Statement = SQLitePCL.sqlite3_stmt;
#else
using Sqlite3Statement = System.IntPtr;
#endif

namespace SQLite.SQLite
{
    public class SQLiteType
    {
        private Func<Type, bool> Condition;
            private Func<int?, bool, string> SQL;
            private Action<Sqlite3Statement, int, object, bool> BindFunc;
            private Func<Sqlite3Statement, int, bool, object> ReadFunc;

            public SQLiteType(Func<Type, bool> condition, string sql,
                Action<Sqlite3Statement, int, object, bool> bind,
                Func<Sqlite3Statement, int, bool, object> read) :
                this(condition, (len, date) => sql, bind, read)
            {
            }

            public SQLiteType(Func<Type, bool> condition,
                Func<int?, bool, string> sql,
                Action<Sqlite3Statement, int, object, bool> bind,
                Func<Sqlite3Statement, int, bool, object> read)
            {
                Condition = condition;
                SQL = sql;
                BindFunc = bind;
                ReadFunc = read;
            }

            public bool Satisfies(Type type)
            {
                return Condition(type);
            }

            public string GetSQL(int? maxLen, bool storeDateTimeAsTicks)
            {
                return SQL(maxLen, storeDateTimeAsTicks);
            }

            public void Bind(Sqlite3Statement statement, int index, object value, bool storeDateTimeAsTicks)
            {
                BindFunc(statement, index, value, storeDateTimeAsTicks);
            }

            public object Read(Sqlite3Statement statement, int index, bool storeDateTimeAsTicks)
            {
                return ReadFunc(statement, index, storeDateTimeAsTicks);
            }
    }
}
